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
using System.Linq.Expressions;
using System.Threading;

using ASC.Common;
using ASC.Common.Caching;
using ASC.Common.Logging;
using ASC.Core.Common.EF;
using ASC.Core.Data;
using ASC.Core.Tenants;
using ASC.Core.Users;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace ASC.Core.Caching
{
    [Scope]
    public class UserServiceCache
    {
        public const string USERS = "users";
        private const string GROUPS = "groups";
        public const string REFS = "refs";

        public TrustInterval TrustInterval { get; set; }
        public DistributedCache<UserGroupRefStore> CacheUserGroupRefStore { get; }
        public DistributedCache<UserInfo> CacheUserInfo { get; }
        public DistributedCache<UserInfoList> CacheUserInfoList { get; }
        public DistributedCache<GroupList> CacheGroupList { get; }
        internal CoreBaseSettings CoreBaseSettings { get; }

        public UserServiceCache(
            CoreBaseSettings coreBaseSettings,
            DistributedCache<UserGroupRefStore> cacheUserGroupRefStore,
            DistributedCache<UserInfo> cacheUserInfo,
            DistributedCache<UserInfoList> cacheUserInfoList,
            DistributedCache<GroupList> cacheGroupList,
            IOptionsMonitor<ILog> options)
        {
            TrustInterval = new TrustInterval();
            CoreBaseSettings = coreBaseSettings;
            CacheUserGroupRefStore = cacheUserGroupRefStore;
            CacheUserInfo = cacheUserInfo;
            CacheUserInfoList = cacheUserInfoList;
            CacheGroupList = cacheGroupList;
        }

        public void InvalidateCache()
        {
            InvalidateCache(null);
        }

        private void InvalidateCache(UserInfo userInfo)
        {
            if (CoreBaseSettings.Personal && userInfo != null)
            {
                var key = GetUserCacheKeyForPersonal(userInfo.Tenant, userInfo.ID);
                CacheUserInfo.Remove(key);
            }

            TrustInterval.Expire();
        }

        private void UpdateUserGroupRefCache(UserGroupRef r, bool remove)
        {
            var key = GetRefCacheKey(r.Tenant);
            var refs = CacheUserGroupRefStore.Get(key);
            if (!remove && refs != null)
            {
                lock (refs)
                {
                    refs[r.CreateKey()] = r;
                }
            }
            else
            {
                InvalidateCache();
            }
        }
        public static string GetUserPhotoCacheKey(int tenant, Guid userId)
        {
            return tenant.ToString() + "userphoto" + userId.ToString();
        }

        public static string GetGroupCacheKey(int tenant)
        {
            return tenant.ToString() + GROUPS;
        }

        public static string GetRefCacheKey(int tenant)
        {
            return tenant.ToString() + REFS;
        }

        public static string GetUserCacheKey(int tenant)
        {
            return tenant.ToString() + USERS;
        }

        public static string GetUserCacheKeyForPersonal(int tenant, Guid userId)
        {
            return tenant.ToString() + USERS + userId;
        }
    }

    [Scope]
    class ConfigureCachedUserService : IConfigureNamedOptions<CachedUserService>
    {
        internal IOptionsSnapshot<EFUserService> Service { get; }
        internal UserServiceCache UserServiceCache { get; }
        internal CoreBaseSettings CoreBaseSettings { get; }

        public ConfigureCachedUserService(
            IOptionsSnapshot<EFUserService> service,
            UserServiceCache userServiceCache,
            CoreBaseSettings coreBaseSettings)
        {
            Service = service;
            UserServiceCache = userServiceCache;
            CoreBaseSettings = coreBaseSettings;
        }

        public void Configure(string name, CachedUserService options)
        {
            Configure(options);
            options.Service = Service.Get(name);
        }

        public void Configure(CachedUserService options)
        {
            options.Service = Service.Value;
            options.CoreBaseSettings = CoreBaseSettings;
            options.UserServiceCache = UserServiceCache;
            options.CacheUserGroupRefStore = UserServiceCache.CacheUserGroupRefStore;
            options.CacheGroupList = UserServiceCache.CacheGroupList;
            options.CacheUserInfo = UserServiceCache.CacheUserInfo;
            options.CacheUserInfoList = UserServiceCache.CacheUserInfoList;
            options.TrustInterval = UserServiceCache.TrustInterval;
            options.TrustInterval = UserServiceCache.TrustInterval;
        }
    }

    [Scope]
    public class CachedUserService : IUserService, ICachedService
    {
        internal IUserService Service { get; set; }

        protected UserInfoList LocalUserInfoList;

        internal TrustInterval TrustInterval { get; set; }
        private int getchanges;

        public DistributedCache<UserGroupRefStore> CacheUserGroupRefStore { get; set; }
        public DistributedCache<UserInfo> CacheUserInfo { get; set; }
        public DistributedCache<UserInfoList> CacheUserInfoList { get; set; }
        public DistributedCache<GroupList> CacheGroupList { get; set; }
        private TimeSpan CacheExpiration { get; set; }
        private TimeSpan DbExpiration { get; set; }
        private TimeSpan PhotoExpiration { get; set; }
        internal CoreBaseSettings CoreBaseSettings { get; set; }
        internal UserServiceCache UserServiceCache { get; set; }

        public CachedUserService()
        {
            CacheExpiration = TimeSpan.FromMinutes(20);
            DbExpiration = TimeSpan.FromMinutes(1);
            PhotoExpiration = TimeSpan.FromMinutes(10);
        }

        public CachedUserService(
            EFUserService service,
            CoreBaseSettings coreBaseSettings,
            UserServiceCache userServiceCache
            ) : this()
        {
            Service = service ?? throw new ArgumentNullException("service");
            CoreBaseSettings = coreBaseSettings;
            UserServiceCache = userServiceCache;
            CacheUserGroupRefStore = userServiceCache.CacheUserGroupRefStore;
            CacheUserInfo = userServiceCache.CacheUserInfo;
            CacheUserInfoList = userServiceCache.CacheUserInfoList;
            CacheUserInfo = userServiceCache.CacheUserInfo;
            CacheGroupList = userServiceCache.CacheGroupList;
            TrustInterval = userServiceCache.TrustInterval;
        }

        public IDictionary<Guid, UserInfo> GetUsers(int tenant, DateTime from)
        {
            var users = GetUsers(tenant);
            lock (users)
            {
                return (from == default ? users.Values : users.Values.Where(u => u.LastModified >= from)).ToDictionary(u => u.ID);
            }
        }

        public IQueryable<UserInfo> GetUsers(
            int tenant,
            bool isAdmin,
            EmployeeStatus? employeeStatus,
            List<List<Guid>> includeGroups,
            List<Guid> excludeGroups,
            EmployeeActivationStatus? activationStatus,
            string text,
            string sortBy,
            bool sortOrderAsc,
            long limit,
            long offset,
            out int total,
            out int count)
        {
            return Service.GetUsers(tenant, isAdmin, employeeStatus, includeGroups, excludeGroups, activationStatus, text, sortBy, sortOrderAsc, limit, offset, out total, out count);
        }

        public UserInfo GetUser(int tenant, Guid id)
        {
            if (CoreBaseSettings.Personal)
            {
                return GetUserForPersonal(tenant, id);
            }

            var keyForUser = UserServiceCache.GetUserCacheKeyForPersonal(tenant, id);
            var user = CacheUserInfo.Get(keyForUser);

            if (user != null) return user;

            else
            {
                var usersList = GetUsersProto(tenant);
                user = usersList.UserInfoListProto.Where(u =>  u.ID == id).FirstOrDefault();
            }

            if (user != null) CacheUserInfo.Insert(keyForUser, user, CacheExpiration);

            return user;
        }

        public UserInfo GetUser(int tenant, string email)
        {
            return Service.GetUser(tenant, email);
        }

        public UserInfo GetUserByUserName(int tenant, string userName)
        {
            return Service.GetUserByUserName(tenant, userName);
        }


        /// <summary>
        /// For Personal only
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private UserInfo GetUserForPersonal(int tenant, Guid id)
        {
            if (!CoreBaseSettings.Personal) return GetUser(tenant, id);

            var key = UserServiceCache.GetUserCacheKeyForPersonal(tenant, id);
            var user = CacheUserInfo.Get(key);

            if (user == null)
            {
                user = Service.GetUser(tenant, id);

                if (user != null)
                {
                    CacheUserInfo.Insert(key, user, CacheExpiration);
                }
            }

            return user;
        }

        public UserInfo GetUserByPasswordHash(int tenant, string login, string passwordHash)
        {
            return Service.GetUserByPasswordHash(tenant, login, passwordHash);
        }

        public UserInfo SaveUser(int tenant, UserInfo user)
        {
            var users = GetUsersProto(tenant);
            var keyForUsers = UserServiceCache.GetUserCacheKey(tenant);

            CacheUserInfo.Remove(keyForUsers);

            var pastUser = users.UserInfoListProto.SingleOrDefault(u => u.ID ==  user.ID);

            if (pastUser != null) users.UserInfoListProto.Remove(pastUser);

            users.UserInfoListProto.Add(user);

            var options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = CacheExpiration
            };

            CacheUserInfoList.cache.SetString(keyForUsers + "hash", users.UserInfoListProto.GetHashCode().ToString(), options);
            CacheUserInfoList.Insert(keyForUsers, users, CacheExpiration);

            LocalUserInfoList = users;

            var keyForUser = UserServiceCache.GetUserCacheKeyForPersonal(tenant, user.ID);

            CacheUserInfo.Remove(keyForUser);
            CacheUserInfo.Insert(keyForUser, user, CacheExpiration);

            user = Service.SaveUser(tenant, user);
            return user;
        }

        public void RemoveUser(int tenant, Guid id)
        {
            var key = UserServiceCache.GetUserCacheKey(tenant);
            var users = GetUsersProto(tenant);
            var user = users.UserInfoListProto.Where(x => x.ID == id).FirstOrDefault();

            if (user == null) return;

            CacheUserInfo.Remove(key);

            var options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = CacheExpiration
            };

            CacheUserInfo.cache.SetString(key + "hash", users.UserInfoListProto.GetHashCode().ToString(), options);

            users.UserInfoListProto.Remove(user);
            CacheUserInfoList.Insert(key, users, CacheExpiration);

            LocalUserInfoList = users;

            key = UserServiceCache.GetUserCacheKeyForPersonal(tenant, user.ID);
            CacheUserInfo.Remove(key);

            Service.RemoveUser(tenant, id);
        }

        public byte[] GetUserPhoto(int tenant, Guid id)
        {
            var photo = CacheUserInfo.GetClean(UserServiceCache.GetUserPhotoCacheKey(tenant, id));

            if (photo == null)
            {
                photo = Service.GetUserPhoto(tenant, id);
                CacheUserInfo.Insert(UserServiceCache.GetUserPhotoCacheKey(tenant, id), photo, PhotoExpiration);
            }

            return photo;
        }

        public void SetUserPhoto(int tenant, Guid id, byte[] photo)
        {
            var key = UserServiceCache.GetUserPhotoCacheKey(tenant, id);
            CacheUserInfo.Remove(key);

            Service.SetUserPhoto(tenant, id, photo);
        }

        public DateTime GetUserPasswordStamp(int tenant, Guid id)
        {
            return Service.GetUserPasswordStamp(tenant, id);
        }

        public void SetUserPasswordHash(int tenant, Guid id, string passwordHash)
        {
            var key = UserServiceCache.GetUserCacheKey(tenant);
            CacheUserInfo.Remove(key);

            key = UserServiceCache.GetUserCacheKeyForPersonal(tenant, id);
            CacheUserInfo.Remove(key);

            Service.SetUserPasswordHash(tenant, id, passwordHash);
        }

        public IDictionary<Guid, Group> GetGroups(int tenant, DateTime from)
        {
            var groups = GetGroups(tenant);
            lock (groups)
            {
                return (from == default ? groups.Values : groups.Values.Where(g => g.LastModified >= from)).ToDictionary(g => g.Id);
            }
        }

        public Group GetGroup(int tenant, Guid id)
        {
            var groups = GetGroups(tenant);
            lock (groups)
            {
                groups.TryGetValue(id, out var g);
                return g;
            }
        }

        public Group SaveGroup(int tenant, Group group)
        {
            var key = UserServiceCache.GetGroupCacheKey(tenant);
            CacheGroupList.Remove(key);

            group = Service.SaveGroup(tenant, group);
            return group;
        }

        public void RemoveGroup(int tenant, Guid id)
        {
            var key = UserServiceCache.GetGroupCacheKey(tenant);
            CacheGroupList.Remove(key);

            Service.RemoveGroup(tenant, id);
        }


        public IDictionary<string, UserGroupRef> GetUserGroupRefs(int tenant, DateTime from)
        {
            if (CoreBaseSettings.Personal)
            {
                return new Dictionary<string, UserGroupRef>();
            }

            GetChangesFromDb();

            var key = UserServiceCache.GetRefCacheKey(tenant);
            if (!(CacheUserGroupRefStore.Get(key) is IDictionary<string, UserGroupRef> refs))
            {
                refs = Service.GetUserGroupRefs(tenant, default);
                CacheUserGroupRefStore.Insert(key, new UserGroupRefStore(refs), CacheExpiration);
            }
            lock (refs)
            {
                return from == default ? refs : refs.Values.Where(r => r.LastModified >= from).ToDictionary(r => r.CreateKey());
            }
        }

        public UserGroupRef SaveUserGroupRef(int tenant, UserGroupRef r)
        {
            var key = UserServiceCache.GetRefCacheKey(tenant);
            CacheUserGroupRefStore.Remove(key);

            r = Service.SaveUserGroupRef(tenant, r);
            return r;
        }

        public void RemoveUserGroupRef(int tenant, Guid userId, Guid groupId, UserGroupRefType refType)
        {
            var key = UserServiceCache.GetRefCacheKey(tenant);
            CacheUserGroupRefStore.Remove(key);

            Service.RemoveUserGroupRef(tenant, userId, groupId, refType);
        }


        private IDictionary<Guid, UserInfo> GetUsers(int tenant)
        {
            return GetUsersProto(tenant).UserInfoListProto.ToDictionary(r => r.ID, r => r);
        }

        private IDictionary<Guid, Group> GetGroups(int tenant)
        {
            GetChangesFromDb();

            var key = UserServiceCache.GetGroupCacheKey(tenant);
            var groups = CacheGroupList.Get(key);

            if (groups == null)
            {
                groups = new GroupList();

                groups.GroupListProto.AddRange(Service.GetGroups(tenant, default).Values);
                CacheGroupList.Insert(key, groups, CacheExpiration);
            }

            return groups.GroupListProto.ToDictionary(r => r.Id, r => r);
        }

        private void GetChangesFromDb()
        {
            if (!TrustInterval.Expired)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref getchanges, 1, 0) == 0)
            {
                try
                {
                    if (!TrustInterval.Expired)
                    {
                        return;
                    }

                    var starttime = TrustInterval.StartTime;
                    if (starttime != default)
                    {
                        var correction = TimeSpan.FromTicks(DbExpiration.Ticks * 3);
                        starttime = TrustInterval.StartTime.Subtract(correction);
                    }

                    TrustInterval.Start(DbExpiration);

                    //get and merge changes in cached tenants
                    //foreach (var tenantGroup in Service.GetUsers(Tenant.DEFAULT_TENANT, starttime).Values.GroupBy(u => u.Tenant))
                    //{
                    //    var users = Cache.Get<IDictionary<Guid, UserInfo>>(UserServiceCache.GetUserCacheKey(tenantGroup.Key));
                    //    if (users != null)
                    //    {
                    //        lock (users)
                    //        {
                    //            foreach (var u in tenantGroup)
                    //            {
                    //                users[u.ID] = u;
                    //            }
                    //        }
                    //    }
                    //}

                    //foreach (var tenantGroup in Service.GetGroups(Tenant.DEFAULT_TENANT, starttime).Values.GroupBy(g => g.Tenant))
                    //{
                    //    var groups = Cache.Get<IDictionary<Guid, Group>>(UserServiceCache.GetGroupCacheKey(tenantGroup.Key));
                    //    if (groups != null)
                    //    {
                    //        lock (groups)
                    //        {
                    //            foreach (var g in tenantGroup)
                    //            {
                    //                groups[g.Id] = g;
                    //            }
                    //        }
                    //    }
                    //}

                    foreach (var tenantGroup in Service.GetUserGroupRefs(Tenant.DEFAULT_TENANT, starttime).Values.GroupBy(r => r.Tenant))
                    {
                        var refs = CacheUserGroupRefStore.Get(UserServiceCache.GetRefCacheKey(tenantGroup.Key));
                        if (refs != null)
                        {
                            lock (refs)
                            {
                                foreach (var r in tenantGroup)
                                {
                                    refs[r.CreateKey()] = r;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    Volatile.Write(ref getchanges, 0);
                }
            }
        }


        public void InvalidateCache()
        {
            UserServiceCache.InvalidateCache();
        }

        public UserInfo GetUser(int tenant, Guid id, Expression<Func<User, UserInfo>> exp)
        {
            return Service.GetUser(tenant, id, exp);
        }

        private UserInfoList GetUsersProto(int tenant)
        {
            var key = UserServiceCache.GetUserCacheKey(tenant);
            var hashString = CacheUserInfoList.cache.GetString(key + "hash");

            if (int.TryParse(hashString, out int result) && LocalUserInfoList != null && result == LocalUserInfoList.UserInfoListProto.GetHashCode())
            {
                return LocalUserInfoList;
            }

            var usersList = CacheUserInfoList.Get(key);

            if (usersList == null)
            {
                usersList = new UserInfoList();
                var users = Service.GetUsers(tenant, default).Values;
                usersList.UserInfoListProto.AddRange(users);

                var options = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = CacheExpiration
                };

                CacheUserInfoList.cache.SetString(key + "hash", usersList.UserInfoListProto.GetHashCode().ToString(), options);
                CacheUserInfoList.Insert(key, usersList, CacheExpiration);
            }

            LocalUserInfoList = usersList;

            return usersList;
        }

        [Serializable]
        class UserPhoto
        {
            public string Key { get; set; }
        }
    }
}
