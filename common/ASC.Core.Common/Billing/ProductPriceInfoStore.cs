using System.Collections.Generic;

using ASC.Common.Caching;

namespace ASC.Core.Billing
{
    public partial class ProductPriceInfoStore : ICustomSer<ProductPriceInfoStore>
    {
        public IDictionary<string, Dictionary<string, decimal>> Infos { get; } =
            new Dictionary<string, Dictionary<string, decimal>>();

        public ProductPriceInfoStore(IDictionary<string, Dictionary<string, decimal>> dict)
        {
            foreach (var pair in dict)
            {
                Infos.Add(pair.Key, pair.Value);
            }
        }

        public void CustomDeSer()
        {
            foreach (var info in ProductPriceInfos)
            {
                info.Value.CustomDeSer();
                Infos.Add(info.Key, info.Value.Infos);
            }
        }

        public void CustomSer()
        {
            ProductPriceInfos.Clear();

            foreach (var info in Infos)
            {
                var priceInfo = new ProductPriceInfo(info.Value);
                priceInfo.CustomSer();
                ProductPriceInfos.Add(info.Key, priceInfo);
            }
        }
    }

    public partial class ProductPriceInfo : ICustomSer<ProductPriceInfo>
    {
        public Dictionary<string, decimal> Infos { get; } = new Dictionary<string, decimal>();

        public ProductPriceInfo(IDictionary<string, decimal> dict)
        {
            foreach (var pair in dict)
            {
                Infos.Add(pair.Key, pair.Value);
            }
        }

        public void CustomDeSer()
        {
            foreach (var pair in PriceInfos)
            {
                Infos.Add(pair.Key, (decimal)pair.Value);
            }
        }

        public void CustomSer()
        {
            PriceInfos.Clear();

            foreach (var pair in Infos)
            {
                PriceInfos.Add(pair.Key, (DecimalValue)pair.Value);
            }
        }
    }
}
