using System.Collections.Generic;
using System.Linq;

using ASC.Common.Caching;

namespace ASC.Core
{
    public partial class SubscriptionsStore : ICustomSer<SubscriptionsStore>
    {
        private IDictionary<string, List<SubscriptionRecord>> recordsByRec;
        private IDictionary<string, List<SubscriptionRecord>> recordsByObj;
        private IDictionary<string, List<SubscriptionMethod>> methodsByRec;

        public SubscriptionsStore(IEnumerable<SubscriptionRecord> records, IEnumerable<SubscriptionMethod> methods)
        {
            Records.AddRange(records);
            Methods.AddRange(methods);
            BuildSubscriptionsIndex(records);
            BuildMethodsIndex(methods);
        }

        public IEnumerable<SubscriptionRecord> GetSubscriptions()
        {
            return Records.ToList();
        }

        public IEnumerable<SubscriptionRecord> GetSubscriptions(string recipientId, string objectId)
        {
            return recipientId != null ?
                recordsByRec.ContainsKey(recipientId) ? recordsByRec[recipientId].ToList() : new List<SubscriptionRecord>() :
                recordsByObj.ContainsKey(objectId ?? string.Empty) ? recordsByObj[objectId ?? string.Empty].ToList() : new List<SubscriptionRecord>();
        }

        public SubscriptionRecord GetSubscription(string recipientId, string objectId)
        {
            return recordsByRec.ContainsKey(recipientId) ?
                recordsByRec[recipientId].Where(s => s.ObjectId == objectId).FirstOrDefault() :
                null;
        }

        public void SaveSubscription(SubscriptionRecord s)
        {
            var old = GetSubscription(s.RecipientId, s.ObjectId);
            if (old != null)
            {
                old.Subscribed = s.Subscribed;
            }
            else
            {
                Records.Add(s);
                BuildSubscriptionsIndex(Records);
            }
        }

        public void RemoveSubscriptions()
        {
            Records.Clear();
            BuildSubscriptionsIndex(Records);
        }

        public void RemoveSubscriptions(string objectId)
        {
            Records.RemoveAll(s => s.ObjectId == objectId);
            BuildSubscriptionsIndex(Records);
        }

        public IEnumerable<SubscriptionMethod> GetSubscriptionMethods(string recipientId)
        {
            return string.IsNullOrEmpty(recipientId) ?
                Methods.ToList() :
                methodsByRec.ContainsKey(recipientId) ? methodsByRec[recipientId].ToList() : new List<SubscriptionMethod>();
        }

        public void SetSubscriptionMethod(SubscriptionMethod m)
        {
            Methods.RemoveAll(r => r.Tenant == m.Tenant && r.SourceId == m.SourceId && r.ActionId == m.ActionId && r.RecipientId == m.RecipientId);
            if (m.Methods != null && 0 < m.Methods.Length)
            {
                Methods.Add(m);
            }
            BuildMethodsIndex(Methods);
        }

        private void BuildSubscriptionsIndex(IEnumerable<SubscriptionRecord> records)
        {
            recordsByRec = records.GroupBy(r => r.RecipientId).ToDictionary(g => g.Key, g => g.ToList());
            recordsByObj = records.GroupBy(r => r.ObjectId ?? string.Empty).ToDictionary(g => g.Key, g => g.ToList());
        }

        private void BuildMethodsIndex(IEnumerable<SubscriptionMethod> methods)
        {
            methodsByRec = methods.GroupBy(r => r.RecipientId).ToDictionary(g => g.Key, g => g.ToList());
        }

        public void CustomSer()
        {
            foreach (var method in Methods)
            {
                method.CustomSer();
            }
        }

        public void CustomDeSer()
        {
            foreach (var method in Methods)
            {
                method.CustomDeSer();
            }
        }
    }
}
