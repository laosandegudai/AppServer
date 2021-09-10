using System;
using System.Collections.Generic;
using System.Diagnostics;

using ASC.Common.Logging;

using Google.Protobuf;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace ASC.Common.Caching
{
    public interface ICustomSer<T> where T : IMessage<T>
    {
        void CustomSer();
        void CustomDeSer();
    }


    [Scope]
    public class DistributedCache<T> where T : IMessage<T>, ICustomSer<T>, new()
    {
        public readonly IDistributedCache cache;
        public readonly IDictionary<string, T> localCache;

        private ILog _logger;

        public DistributedCache(IDistributedCache cache, IOptionsMonitor<ILog> options)
        {
            this.cache = cache;
            localCache = new Dictionary<string, T>();
            _logger = options.Get("ASC.Web.Api");

            _logger.Info("DistributedCache loaded.");
        }

        public T Get(string key)
        {
            var sw = Stopwatch.StartNew();

            if (localCache.ContainsKey(key)) return localCache[key];

            var binaryData = cache.Get(key);

            if (binaryData != null)
            {
                try
                {
                    var sw1 = Stopwatch.StartNew();

                    var parser = new MessageParser<T>(() => new T());

                    var result = parser.ParseFrom(binaryData);
                    result.CustomDeSer();

                    localCache.Add(key, result);

                    sw1.Stop();
                    _logger.Info($"DistributedCache: Key {key} parsed in {sw1.Elapsed.TotalMilliseconds} ms.");

                    sw.Stop();
                    _logger.Info($"DistributedCache: Key {key} found in {sw.Elapsed.TotalMilliseconds} ms.");

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);

                    sw.Stop();
                    _logger.Info($"DistributedCache: Key {key} rised Exception in {sw.Elapsed.TotalMilliseconds} ms.");

                    return default;
                }
            }

            sw.Stop();
            _logger.Info($"DistributedCache: Key {key} not found in {sw.Elapsed.TotalMilliseconds} ms.");

            return default;
        }

        public byte[] GetClean(string key)
        {
            return cache.Get(key);
        }

        public string GetString(string key)
        {
            return cache.GetString(key);
        }

        public void Insert(string key, T value, TimeSpan sligingExpiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = sligingExpiration
            };

            Insert(key, value, options);
        }

        public void Insert(string key, T value, DateTime absolutExpiration)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = absolutExpiration
            };

            Insert(key, value, options);
        }

        public void Insert(string key, byte[] value, TimeSpan sligingExpiration)
        {
            var options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = sligingExpiration
            };

            Insert(key, value, options);
        }

        public void Insert(string key, byte[] value, DateTime absolutExpiration)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = absolutExpiration
            };

            Insert(key, value, options);
        }

        public void InsertString(string key, string value, DateTime absolutExpiration)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = absolutExpiration
            };

            InsertString(key, value, options);
        }

        public void InsertString(string key, string value, TimeSpan sligingExpiration)
        {
            var options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = sligingExpiration
            };

            InsertString(key, value, options);
        }

        public void Remove(string key)
        {
            var sw = Stopwatch.StartNew();

            cache.Remove(key);

            sw.Stop();
            _logger.Info($"DistributedCache: Key {key} Remove in {sw.Elapsed.TotalMilliseconds} ms.");
        }

        private void Insert(string key, T value, DistributedCacheEntryOptions options)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                value.CustomSer();
                cache.Set(key, value.ToByteArray(), options);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            sw.Stop();
            _logger.Info($"DistributedCache: Key {key} Insert in {sw.Elapsed.TotalMilliseconds} ms.");
        }

        private void Insert(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                cache.Set(key, value, options);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            sw.Stop();
            _logger.Info($"DistributedCache: Key {key} Insert in {sw.Elapsed.TotalMilliseconds} ms.");
        }

        private void InsertString(string key, string value, DistributedCacheEntryOptions options)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                cache.SetString(key, value, options);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            sw.Stop();
            _logger.Info($"DistributedCache: Key {key} Insert in {sw.Elapsed.TotalMilliseconds} ms.");
        }
    }
}