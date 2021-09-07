using System;
using System.Collections.Concurrent;
using ASC.Common.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.IO;
using Google.Protobuf;
using System.Diagnostics;
using NLog.Fluent;

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

        public void Insert(string key, T value, TimeSpan sligingExpiration)
        {
            var sw = Stopwatch.StartNew();


            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = sligingExpiration
            };

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

        public void Insert(string key, T value, DateTime absolutExpiration)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = absolutExpiration
            };

            try
            {
                value.CustomSer();
                cache.Set(key, value.ToByteArray(), options);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void Insert(string key, byte[] value, TimeSpan sligingExpiration)
        {
            var options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = sligingExpiration
            };

            try
            {
                cache.Set(key, value, options);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void Insert(string key, byte[] value, DateTime absolutExpiration)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = absolutExpiration
            };

            try
            {
                cache.Set(key, value, options);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public void Remove(Regex pattern)
        {
            throw new NotImplementedException();
        }
    }
}