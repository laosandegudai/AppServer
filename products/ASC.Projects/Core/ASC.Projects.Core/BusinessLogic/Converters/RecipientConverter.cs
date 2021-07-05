using System;
using ASC.Core.Common.Utils;
using ASC.Notify.Model;
using ASC.Notify.Recipients;
using ASC.Projects.Core.BusinessLogic.Converters.Interfaces;

namespace ASC.Projects.Core.BusinessLogic.Converters
{
    public class RecipientConverter : IRecipientConverter
    {
        private readonly INotifySource _notifySource;

        public RecipientConverter(INotifySource notifySource)
        {
            _notifySource = notifySource.NotNull(nameof(notifySource));
        }

        public IRecipient Convert(Guid userId)
        {
            var result = _notifySource
                .GetRecipientsProvider()
                .GetRecipient(userId.ToString());

            return result;
        }
    }
}
