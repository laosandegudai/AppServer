using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;

namespace ASC.Common.Caching
{
    public abstract class ProtoSerializeMap<TSuccessor, TKey, TValue>
        : ICustomSer<TSuccessor>, IDictionary<TKey, TValue>
        where TSuccessor : IMessage<TSuccessor>, new()
    {
        public ICollection<TKey> Keys => GetMap().Keys;

        public ICollection<TValue> Values => GetMap().Values;

        public int Count => GetMap().Count;

        public bool IsReadOnly => GetMap().IsReadOnly;

        public TValue this[TKey key]
        {
            get => GetMap()[key];
            set => GetMap()[key] = value;
        }

        protected abstract IDictionary<TKey, TValue> GetMap();

        public void Add(TKey key, TValue value)
        {
            GetMap().Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return GetMap().ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return GetMap().Remove(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return GetMap().TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            GetMap().Add(item.Key, item.Value);
        }

        public void Clear()
        {
            GetMap().Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return GetMap().Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            GetMap().CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return GetMap().Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return GetMap().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetMap().GetEnumerator();
        }

        public abstract void CustomSer();

        public abstract void CustomDeSer();
    }
}
