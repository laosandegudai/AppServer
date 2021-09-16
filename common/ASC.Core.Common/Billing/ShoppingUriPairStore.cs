using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ASC.Common.Caching;

namespace ASC.Core.Billing
{
    public partial class ShoppingUriPairStore : ICustomSer<ShoppingUriPairStore>,
        IDictionary<string, Tuple<Uri, Uri>>
    {
        public IDictionary<string, Tuple<Uri, Uri>> ShoppingUriPairs { get; private set; } =
            new Dictionary<string, Tuple<Uri, Uri>>();

        public ICollection<string> Keys => ShoppingUriPairs.Keys;

        public ICollection<Tuple<Uri, Uri>> Values => ShoppingUriPairs.Values;

        public int Count => ShoppingUriPairs.Count;

        public bool IsReadOnly => ShoppingUriPairs.IsReadOnly;

        public Tuple<Uri, Uri> this[string key] { get => ShoppingUriPairs[key]; set => ShoppingUriPairs[key] = value; }

        public ShoppingUriPairStore(IDictionary<string, Tuple<Uri, Uri>> dict)
        {
            foreach (var pair in dict)
            {
                ShoppingUriPairs.Add(pair.Key, pair.Value);
            }
        }

        public void CustomDeSer()
        {
            foreach (var pair in UriPairs)
            {
                pair.Value.CustomDeSer();
                ShoppingUriPairs.Add(pair.Key, pair.Value.ShoppingUris);
            }
        }

        public void CustomSer()
        {
            UriPairs.Clear();

            foreach (var pair in ShoppingUriPairs)
            {
                var uriPair = new ShoppingUriPair(pair.Value);
                UriPairs.Add(pair.Key, uriPair);
            }
        }

        public void Add(string key, Tuple<Uri, Uri> value)
        {
            ShoppingUriPairs.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return ShoppingUriPairs.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return ShoppingUriPairs.Remove(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out Tuple<Uri, Uri> value)
        {
            return ShoppingUriPairs.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, Tuple<Uri, Uri>> item)
        {
            ShoppingUriPairs.Add(item);
        }

        public void Clear()
        {
            ShoppingUriPairs.Clear();
        }

        public bool Contains(KeyValuePair<string, Tuple<Uri, Uri>> item)
        {
            return ShoppingUriPairs.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Tuple<Uri, Uri>>[] array, int arrayIndex)
        {
            ShoppingUriPairs.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Tuple<Uri, Uri>> item)
        {
            return ShoppingUriPairs.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, Tuple<Uri, Uri>>> GetEnumerator()
        {
            return ShoppingUriPairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)ShoppingUriPairs).GetEnumerator();
        }
    }

    public partial class ShoppingUriPair : ICustomSer<ShoppingUriPair>
    {
        public Tuple<Uri, Uri> ShoppingUris { get; private set; } = new Tuple<Uri, Uri>(null, null);

        public ShoppingUriPair(Tuple<Uri, Uri> tuple)
        {
            ShoppingUris = new Tuple<Uri, Uri>(tuple.Item1, tuple.Item2);
        }

        public void CustomDeSer()
        {
            ShoppingUris = new Tuple<Uri, Uri>(new Uri(Uri1), new Uri(Uri2));
        }

        public void CustomSer()
        {
            Uri1 = ShoppingUris.Item1.OriginalString;
            Uri2 = ShoppingUris.Item2.OriginalString;
        }
    }
}
