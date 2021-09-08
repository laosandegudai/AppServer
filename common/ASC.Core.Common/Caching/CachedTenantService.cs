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
using System.Collections.Generic;

using ASC.Common;
using ASC.Common.Caching;
using ASC.Core.Data;
using ASC.Core.Tenants;

using Microsoft.Extensions.Options;

namespace ASC.Core.Tenants
{
    public partial class TenantStore : ICustomSer<TenantStore>
    {
        private readonly object locker = new object();

        public void CustomDeSer()
        {
            foreach (var pair in ById)
            {
                pair.Value.CustomDeSer();
            }

            foreach (var pair in ByDomain)
            {
                pair.Value.CustomDeSer();
            }
        }

        public void CustomSer()
        {
            foreach (var pair in ById)
            {
                pair.Value.CustomSer();
            }

            foreach (var pair in ByDomain)
            {
                pair.Value.CustomSer();
            }
        }

        public Tenant Get(int id)
        {
            Tenant t;
            lock (locker)
            {
                ById.TryGetValue(id, out t);
            }
            return t;
        }

        public Tenant Get(string domain)
        {
            if (string.IsNullOrEmpty(domain)) return null;

            Tenant t;
            lock (locker)
            {
                ByDomain.TryGetValue(domain, out t);
            }
            return t;
        }

        public void Insert(Tenant t, string ip = null)
        {
            if (t == null)
            {
                return;
            }

            Remove(t.TenantId);
            lock (locker)
            {
                ById[t.TenantId] = t;
                ByDomain[t.TenantAlias] = t;
                if (!string.IsNullOrEmpty(t.MappedDomain)) ByDomain[t.MappedDomain] = t;
                if (!string.IsNullOrEmpty(ip)) ByDomain[ip] = t;
            }
        }

        public void Remove(int id)
        {
            var t = Get(id);
            if (t != null)
            {
                lock (locker)
                {
                    ById.Remove(id);
                    ByDomain.Remove(t.TenantAlias);
                    if (!string.IsNullOrEmpty(t.MappedDomain))
                    {
                        ByDomain.Remove(t.MappedDomain);
                    }
                }
            }
        }

        internal void Clear(CoreBaseSettings coreBaseSettings)
        {
            if (!coreBaseSettings.Standalone) return;
            lock (locker)
            {
                ById.Clear();
                ByDomain.Clear();
            }
        }
    }
}


namespace ASC.Core.Caching
{
    [Scope]
    class TenantServiceCache
    {
        public const string KEY = "tenants";
        public TimeSpan CacheExpiration { get; set; }
        internal DistributedCache<TenantStore> CacheTenantStore { get; }

        public TenantServiceCache(
            CoreBaseSettings coreBaseSettings,
            DistributedCache<TenantStore> cacheTenantStore)
        {
            CacheTenantStore = cacheTenantStore;
            CacheExpiration = TimeSpan.FromMinutes(2);
        }

        internal TenantStore GetTenantStore()
        {
            var store = CacheTenantStore.Get(KEY);
            if (store == null)
            {
                CacheTenantStore.Insert(KEY, store = new TenantStore(), DateTime.UtcNow.Add(CacheExpiration));
            }
            return store;
        }
    }

    [Scope]
    class ConfigureCachedTenantService : IConfigureNamedOptions<CachedTenantService>
    {
        private IOptionsSnapshot<DbTenantService> Service { get; }
        private TenantServiceCache TenantServiceCache { get; }

        public ConfigureCachedTenantService(
            IOptionsSnapshot<DbTenantService> service,
            TenantServiceCache tenantServiceCache)
        {
            Service = service;
            TenantServiceCache = tenantServiceCache;
        }

        public void Configure(string name, CachedTenantService options)
        {
            Configure(options);
            options.Service = Service.Get(name);
        }

        public void Configure(CachedTenantService options)
        {
            options.Service = Service.Value;
            options.TenantServiceCache = TenantServiceCache;
            options.CacheTenantStore = TenantServiceCache.CacheTenantStore;
        }
    }

    [Scope]
    class CachedTenantService : ITenantService
    {
        internal ITenantService Service { get; set; }
        internal DistributedCache<TenantStore> CacheTenantStore { get; set; }

        private TimeSpan SettingsExpiration { get; set; }
        internal TenantServiceCache TenantServiceCache { get; set; }

        public CachedTenantService()
        {
            SettingsExpiration = TimeSpan.FromMinutes(2);
        }

        public CachedTenantService(DbTenantService service, TenantServiceCache tenantServiceCache,
            DistributedCache<TenantStore> cacheTenantStore) : this()
        {
            Service = service ?? throw new ArgumentNullException("service");
            TenantServiceCache = tenantServiceCache;
            CacheTenantStore = cacheTenantStore;
        }

        public void ValidateDomain(string domain)
        {
            Service.ValidateDomain(domain);
        }

        public IEnumerable<Tenant> GetTenants(string login, string passwordHash)
        {
            return Service.GetTenants(login, passwordHash);
        }

        public IEnumerable<Tenant> GetTenants(DateTime from, bool active = true)
        {
            return Service.GetTenants(from, active);
        }

        public Tenant GetTenant(int id)
        {
            var tenants = TenantServiceCache.GetTenantStore();
            var t = tenants.Get(id);
            if (t == null)
            {
                t = Service.GetTenant(id);
                if (t != null)
                {
                    tenants.Insert(t);
                    CacheTenantStore.Insert(TenantServiceCache.KEY, tenants, DateTime.UtcNow.Add(TenantServiceCache.CacheExpiration));
                }
            }
            return t;
        }

        public Tenant GetTenant(string domain)
        {
            var tenants = TenantServiceCache.GetTenantStore();
            var t = tenants.Get(domain);
            if (t == null)
            {
                t = Service.GetTenant(domain);
                if (t != null)
                {
                    tenants.Insert(t);
                    CacheTenantStore.Insert(TenantServiceCache.KEY, tenants, DateTime.UtcNow.Add(TenantServiceCache.CacheExpiration));
                }
            }
            return t;
        }

        public Tenant GetTenantForStandaloneWithoutAlias(string ip)
        {
            var tenants = TenantServiceCache.GetTenantStore();
            var t = tenants.Get(ip);
            if (t == null)
            {
                t = Service.GetTenantForStandaloneWithoutAlias(ip);
                if (t != null)
                {
                    tenants.Insert(t, ip);
                    CacheTenantStore.Insert(TenantServiceCache.KEY, tenants, DateTime.UtcNow.Add(TenantServiceCache.CacheExpiration));
                }
            }
            return t;
        }

        public Tenant SaveTenant(CoreSettings coreSettings, Tenant tenant)
        {
            tenant = Service.SaveTenant(coreSettings, tenant);

            var tenants = TenantServiceCache.GetTenantStore();

            tenants.Insert(tenant);
            CacheTenantStore.Insert(TenantServiceCache.KEY, tenants, DateTime.UtcNow.Add(TenantServiceCache.CacheExpiration));
            
            return tenant;
        }

        public void RemoveTenant(int id, bool auto = false)
        {
            Service.RemoveTenant(id, auto);

            var tenants = TenantServiceCache.GetTenantStore();
            var t = tenants.Get(id);

            if (t == null) return;

            tenants.Remove(id);
            CacheTenantStore.Insert(TenantServiceCache.KEY, tenants, DateTime.UtcNow.Add(TenantServiceCache.CacheExpiration));
        }

        public IEnumerable<TenantVersion> GetTenantVersions()
        {
            return Service.GetTenantVersions();
        }

        public byte[] GetTenantSettings(int tenant, string key)
        {
            var cacheKey = string.Format("settings/{0}/{1}", tenant, key);
            var data = CacheTenantStore.GetClean(cacheKey);

            if (data == null)
            {
                data = Service.GetTenantSettings(tenant, key);
                CacheTenantStore.Insert(cacheKey, data ?? new byte[0], DateTime.UtcNow + SettingsExpiration);
            }

            return data == null ? null : data.Length == 0 ? null : data;
        }

        public void SetTenantSettings(int tenant, string key, byte[] data)
        {
            Service.SetTenantSettings(tenant, key, data);
            var cacheKey = string.Format("settings/{0}/{1}", tenant, key);

            CacheTenantStore.Insert(cacheKey, data, DateTime.UtcNow + SettingsExpiration);
        }
    }
}
