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
using System.Linq;

using ASC.Common;
using ASC.Common.Caching;
using ASC.Core.Data;
using ASC.Core.Tenants;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ASC.Core.Caching
{
    [Scope]
    class QuotaServiceCache
    {
        internal const string KEY_QUOTA = "quota";
        internal const string KEY_QUOTA_ROWS = "quotarows";

        internal TrustInterval Interval { get; set; }
        internal DistributedCache Cache { get; }

        internal bool QuotaCacheEnabled { get; }

        public QuotaServiceCache(IConfiguration Configuration, 
            DistributedCache cache)
        {
            if (Configuration["core:enable-quota-cache"] == null)
            {
                QuotaCacheEnabled = true;
            }
            else
            {
                QuotaCacheEnabled = !bool.TryParse(Configuration["core:enable-quota-cache"], out var enabled) || enabled;
            }

            Cache = cache;
            Interval = new TrustInterval();
        }
    }

    [Scope]
    class ConfigureCachedQuotaService : IConfigureNamedOptions<CachedQuotaService>
    {
        private IOptionsSnapshot<DbQuotaService> Service { get; }
        private QuotaServiceCache QuotaServiceCache { get; }

        public ConfigureCachedQuotaService(
            IOptionsSnapshot<DbQuotaService> service,
            QuotaServiceCache quotaServiceCache)
        {
            Service = service;
            QuotaServiceCache = quotaServiceCache;
        }

        public void Configure(string name, CachedQuotaService options)
        {
            Configure(options);
            options.Service = Service.Get(name);
        }

        public void Configure(CachedQuotaService options)
        {
            options.Service = Service.Value;
            options.QuotaServiceCache = QuotaServiceCache;
            options.Cache = QuotaServiceCache.Cache;
        }
    }

    [Scope]
    class CachedQuotaService : IQuotaService
    {
        internal IQuotaService Service { get; set; }
        internal DistributedCache Cache { get; set; }
        internal TrustInterval Interval { get; set; }

        internal TimeSpan CacheExpiration { get; set; }
        internal QuotaServiceCache QuotaServiceCache { get; set; }

        public CachedQuotaService()
        {
            Interval = new TrustInterval();
            CacheExpiration = TimeSpan.FromMinutes(10);
        }

        public CachedQuotaService(DbQuotaService service, QuotaServiceCache quotaServiceCache) : this()
        {
            Service = service ?? throw new ArgumentNullException("service");
            QuotaServiceCache = quotaServiceCache;
            Cache = QuotaServiceCache.Cache;
        }

        public IEnumerable<TenantQuota> GetTenantQuotas()
        {
            var quotas = Cache.Get<TenantQuotaList>(QuotaServiceCache.KEY_QUOTA);
            if (quotas == null)
            {
                quotas = new TenantQuotaList();
                quotas.Quotas.AddRange(Service.GetTenantQuotas());

                if (QuotaServiceCache.QuotaCacheEnabled)
                {
                    Cache.Insert(QuotaServiceCache.KEY_QUOTA, quotas, DateTime.UtcNow.Add(CacheExpiration));
                }
            }
            return quotas;
        }

        public TenantQuota GetTenantQuota(int tenant)
        {
            return GetTenantQuotas().SingleOrDefault(q => q.Id == tenant);
        }

        public TenantQuota SaveTenantQuota(TenantQuota quota)
        {
            var q = Service.SaveTenantQuota(quota);

            var quotas = GetTenantQuotas().ToList();
            var pastQuota = quotas.SingleOrDefault(qu => qu.Id == quota.Id);

            if (pastQuota != null) quotas.Remove(pastQuota);

            quotas.Add(quota);

            var quotasList = new TenantQuotaList();
            quotasList.Quotas.AddRange(quotas);

            Cache.Insert(QuotaServiceCache.KEY_QUOTA, quotasList, DateTime.UtcNow.Add(CacheExpiration));

            return q;
        }

        public void RemoveTenantQuota(int tenant)
        {
            throw new NotImplementedException();
        }


        public void SetTenantQuotaRow(TenantQuotaRow row, bool exchange)
        {
            Service.SetTenantQuotaRow(row, exchange);

            var rows = FindTenantQuotaRows(row.Tenant).ToList();
            var pastRow = rows.SingleOrDefault(r => r.Tenant == row.Tenant);

            if (pastRow != null) rows.Remove(pastRow);

            pastRow.Counter = exchange ? pastRow.Counter + row.Counter : row.Counter;

            rows.Add(pastRow);

            var rowsList = new TenantQuotaRowList();
            rowsList.QuotaRows.AddRange(rows);

            Cache.Insert(GetKey(row.Tenant), rowsList, DateTime.UtcNow.Add(CacheExpiration));
        }

        public IEnumerable<TenantQuotaRow> FindTenantQuotaRows(int tenantId)
        {
            var key = GetKey(tenantId);
            var result = Cache.Get<TenantQuotaRowList>(key);

            if (result == null)
            {
                result = new TenantQuotaRowList();
                result.QuotaRows.AddRange(Service.FindTenantQuotaRows(tenantId));

                Cache.Insert(key, result, DateTime.UtcNow.Add(CacheExpiration));
            }

            return result;
        }

        public string GetKey(int tenant)
        {
            return QuotaServiceCache.KEY_QUOTA_ROWS + tenant;
        }
    }
}
