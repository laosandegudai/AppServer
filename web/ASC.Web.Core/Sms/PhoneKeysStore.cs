using System;
using System.Collections.Generic;

using ASC.Common.Caching;

namespace ASC.Web.Core.Sms
{
    public partial class PhoneKeysStore : ICustomSer<PhoneKeysStore>
    {
        public IDictionary<string, DateTime> PhoneKeys { get; private set; } = new Dictionary<string, DateTime>();

        public PhoneKeysStore(IDictionary<string, DateTime> phoneKeys)
        {
            PhoneKeys = phoneKeys;
        }

        public void CustomDeSer()
        {
            foreach (var pair in KeysProto)
            {
                PhoneKeys.Add(pair.Key, pair.Value.ToDateTime());
            }
        }

        public void CustomSer()
        {
            KeysProto.Clear();

            foreach (var pair in PhoneKeys)
            {
                KeysProto.Add(pair.Key, Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(pair.Value.ToUniversalTime()));
            }
        }
    }
}
