using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ASC.FederatedLogin.Helpers;

namespace ASC.Files.Core.Core.Thirdparty
{
    public class CachedEntities
    {
        private Dictionary<string, object> cache
            = new Dictionary<string, object>();

        public T Get<T>(string key)
        {
            cache.TryGetValue(key, out var value);

            return (T)value;
        }

        public void Insert<T>(string key, T data)
        {
            if (cache.ContainsKey(key)) cache.Remove(key);

            cache.Add(key, data);
        }
    }
}
