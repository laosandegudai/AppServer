/*
 *
 * (c) Copyright Ascensio System Limited 2010-2018
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/


using System;
using System.Linq;
using System.Text.Json;

using ASC.Common;
using ASC.Common.Caching;
using ASC.Common.Logging;
using ASC.Core.Common.EF;
using ASC.Core.Common.EF.Context;
using ASC.Core.Common.EF.Model;
using ASC.Core.Common.Settings;
using ASC.Core.Tenants;

using Google.Protobuf;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ASC.Core.Data
{
    [Singletone]
    public class DbSettingsManagerCache
    {
        public DistributedCache Cache { get; }

        public DbSettingsManagerCache(DistributedCache cache)
        {
            Cache = cache;
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }
    }

    [Scope]
    class ConfigureDbSettingsManager : IConfigureNamedOptions<DbSettingsManager>
    {
        private IServiceProvider ServiceProvider { get; }
        private DbSettingsManagerCache DbSettingsManagerCache { get; }
        private IOptionsMonitor<ILog> ILog { get; }
        private AuthContext AuthContext { get; }
        private IOptionsSnapshot<TenantManager> TenantManager { get; }
        private DbContextManager<WebstudioDbContext> DbContextManager { get; }

        public ConfigureDbSettingsManager(
            IServiceProvider serviceProvider,
            DbSettingsManagerCache dbSettingsManagerCache,
            IOptionsMonitor<ILog> iLog,
            AuthContext authContext,
            IOptionsSnapshot<TenantManager> tenantManager,
            DbContextManager<WebstudioDbContext> dbContextManager
            )
        {
            ServiceProvider = serviceProvider;
            DbSettingsManagerCache = dbSettingsManagerCache;
            ILog = iLog;
            AuthContext = authContext;
            TenantManager = tenantManager;
            DbContextManager = dbContextManager;
        }

        public void Configure(string name, DbSettingsManager options)
        {
            Configure(options);

            options.TenantManager = TenantManager.Get(name);
            options.LazyWebstudioDbContext = new Lazy<WebstudioDbContext>(() => DbContextManager.Get(name));
        }

        public void Configure(DbSettingsManager options)
        {
            options.ServiceProvider = ServiceProvider;
            options.DbSettingsManagerCache = DbSettingsManagerCache;
            options.AuthContext = AuthContext;
            options.Log = ILog.CurrentValue;

            options.TenantManager = TenantManager.Value;
            options.LazyWebstudioDbContext = new Lazy<WebstudioDbContext>(() => DbContextManager.Value);
        }
    }

    [Scope(typeof(ConfigureDbSettingsManager))]
    public class DbSettingsManager
    {
        private readonly TimeSpan expirationTimeout = TimeSpan.FromMinutes(5);

        internal ILog Log { get; set; }
        internal DistributedCache Cache { get; set; }
        internal IServiceProvider ServiceProvider { get; set; }
        internal DbSettingsManagerCache DbSettingsManagerCache { get; set; }
        internal AuthContext AuthContext { get; set; }
        internal TenantManager TenantManager { get; set; }
        internal WebstudioDbContext WebstudioDbContext { get => LazyWebstudioDbContext.Value; }
        internal Lazy<WebstudioDbContext> LazyWebstudioDbContext { get; set; }

        public DbSettingsManager()
        {

        }

        public DbSettingsManager(
            IServiceProvider serviceProvider,
            DbSettingsManagerCache dbSettingsManagerCache,
            IOptionsMonitor<ILog> option,
            AuthContext authContext,
            TenantManager tenantManager,
            DbContextManager<WebstudioDbContext> dbContextManager)
        {
            ServiceProvider = serviceProvider;
            DbSettingsManagerCache = dbSettingsManagerCache;
            AuthContext = authContext;
            TenantManager = tenantManager;
            Cache = dbSettingsManagerCache.Cache;
            Log = option.CurrentValue;
            LazyWebstudioDbContext = new Lazy<WebstudioDbContext>(() => dbContextManager.Value);
        }

        private int tenantID;
        private int TenantID
        {
            get { return tenantID != 0 ? tenantID : (tenantID = TenantManager.GetCurrentTenant().TenantId); }
        }
        //
        private Guid? currentUserID;
        private Guid CurrentUserID
        {
            get { return ((Guid?)(currentUserID ??= AuthContext.CurrentAccount.ID)).Value; }
        }

        public bool SaveSettings<TEntity, TCachedEntity>(TEntity settings, int tenantId)
            where TEntity : ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return SaveSettingsFor<TEntity,TCachedEntity>(settings, tenantId, Guid.Empty);
        }

        public TEntity LoadSettings<TEntity, TCachedEntity>(int tenantId) 
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return LoadSettingsFor<TEntity, TCachedEntity>(tenantId, Guid.Empty);
        }

        public void ClearCache<TEntity, TCachedEntity>(int tenantId)
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            var settings = LoadSettings<TEntity, TCachedEntity>(tenantId);
            var key = settings.ID.ToString() + tenantId + Guid.Empty;
            DbSettingsManagerCache.Remove(key);
        }


        public bool SaveSettingsFor<TEntity, TCachedEntity>(TEntity settings, int tenantId, Guid userId) 
            where TEntity : ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            if (settings == null) throw new ArgumentNullException("settings");
            try
            {
                var key = settings.ID.ToString() + tenantId + userId;
                var data = Serialize(settings);

                var def = (TEntity)settings.GetDefault(ServiceProvider);

                var defaultData = Serialize(def);

                if (data.SequenceEqual(defaultData))
                {
                    using var tr = WebstudioDbContext.Database.BeginTransaction();
                    // remove default settings
                    var s = WebstudioDbContext.WebstudioSettings
                        .Where(r => r.Id == settings.ID)
                        .Where(r => r.TenantId == tenantId)
                        .Where(r => r.UserId == userId)
                        .FirstOrDefault();

                    if (s != null)
                    {
                        WebstudioDbContext.WebstudioSettings.Remove(s);
                    }

                    WebstudioDbContext.SaveChanges();
                    tr.Commit();
                }
                else
                {
                    var s = new DbWebstudioSettings
                    {
                        Id = settings.ID,
                        UserId = userId,
                        TenantId = tenantId,
                        Data = data
                    };

                    WebstudioDbContext.AddOrUpdate(r => r.WebstudioSettings, s);

                    WebstudioDbContext.SaveChanges();
                }

                DbSettingsManagerCache.Remove(key);
                Cache.Insert(key, settings.WrapIn(), expirationTimeout);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        internal TEntity LoadSettingsFor<TEntity, TCachedEntity>(int tenantId, Guid userId) 
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity: ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            var settingsInstance = ActivatorUtilities.CreateInstance<TEntity>(ServiceProvider);
            var key = settingsInstance.ID.ToString() + tenantId + userId;
            var def = (TEntity)settingsInstance.GetDefault(ServiceProvider);

            try
            {
                var cachedSettings = Cache.Get<TCachedEntity>(key);
                if (cachedSettings != null) return cachedSettings.WrapIn();

                var result = WebstudioDbContext.WebstudioSettings
                        .Where(r => r.Id == settingsInstance.ID)
                        .Where(r => r.TenantId == tenantId)
                        .Where(r => r.UserId == userId)
                        .Select(r => r.Data)
                        .FirstOrDefault();

                TEntity settings;

                if (result != null)
                {
                    settings = Deserialize<TEntity>(result);
                }
                else
                {
                    settings = def;
                }

                Cache.Insert(key, settings.WrapIn(), expirationTimeout);
                return settings;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return def;
        }

        public TEntity Load<TEntity, TCachedEntity>()
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return LoadSettings<TEntity, TCachedEntity>(TenantID);
        }

        public TEntity LoadForCurrentUser<TEntity, TCachedEntity>()
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return LoadForUser<TEntity, TCachedEntity>(CurrentUserID);
        }

        public TEntity LoadForUser<TEntity, TCachedEntity>(Guid userId)
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return LoadSettingsFor<TEntity, TCachedEntity>(TenantID, userId);
        }

        public TEntity LoadForDefaultTenant<TEntity, TCachedEntity>()
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return LoadForTenant<TEntity, TCachedEntity>(Tenant.DEFAULT_TENANT);
        }

        public TEntity LoadForTenant<TEntity, TCachedEntity>(int tenantId)
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return LoadSettings<TEntity, TCachedEntity>(tenantId);
        }

        public virtual bool Save<TEntity, TCachedEntity>(TEntity data)
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return SaveSettings<TEntity, TCachedEntity>(data, TenantID);
        }

        public bool SaveForCurrentUser<TEntity, TCachedEntity>(TEntity data)
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return SaveForUser<TEntity, TCachedEntity>(data, CurrentUserID);
        }

        public bool SaveForUser<TEntity, TCachedEntity>(TEntity data, Guid userId)
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return SaveSettingsFor<TEntity, TCachedEntity>(data, TenantID, userId);
        }

        public bool SaveForDefaultTenant<TEntity, TCachedEntity>(TEntity data)
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return SaveForTenant<TEntity, TCachedEntity>(data, Tenant.DEFAULT_TENANT);
        }

        public bool SaveForTenant<TEntity, TCachedEntity>(TEntity data, int tenantId)
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            return SaveSettings<TEntity, TCachedEntity>(data, tenantId);
        }

        public void ClearCache<TEntity, TCachedEntity>()
            where TEntity : class, ISettings, ICacheWrapped<TCachedEntity>
            where TCachedEntity : ICustomSer<TCachedEntity>, IMessage<TCachedEntity>, ICacheWrapped<TEntity>, new()
        {
            ClearCache<TEntity, TCachedEntity>(TenantID);
        }

        private T Deserialize<T>(string data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }

        private string Serialize<T>(T settings)
        {
            return JsonSerializer.Serialize(settings);
        }

    }
}