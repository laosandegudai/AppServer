using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ASC.Common.Caching;

namespace ASC.Web.Core.Sms
{
    public partial class Counter : ICustomSer<Counter>
    {
        public Counter(string value)
        {
            Value = value;
        }

        public void CustomDeSer() { }

        public void CustomSer() { }
    }
}
