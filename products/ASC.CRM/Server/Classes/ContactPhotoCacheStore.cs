using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

using ASC.Common.Caching;

namespace ASC.Web.CRM.Classes
{
    public partial class ContactPhotoCacheStore : ICustomSer<ContactPhotoCacheStore>
    {
        public IDictionary<Size, string> ContactPhotoItems {  get; private set; } =
            new Dictionary<Size, string>();

        private Regex pattern = new Regex("\\d+", RegexOptions.Compiled);

        public void CustomDeSer()
        {
            foreach (var pair in Items)
            {
                var matches = pattern.Matches(pair.Key);
                var size = new Size(int.Parse(matches[0].Value), int.Parse(matches[1].Value));
                ContactPhotoItems.Add(size, pair.Value);
            }
        }

        public void CustomSer()
        {
            Items.Clear();

            foreach (var pair in ContactPhotoItems)
            {
                var key = $"{pair.Key.Width}:{pair.Key.Height}";
                Items.Add(key, pair.Value);
            }
        }
    }
}
