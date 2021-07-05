using System;

using ASC.Notify.Recipients;

namespace ASC.Projects.Core.BusinessLogic.Converters.Interfaces
{
    public interface IRecipientConverter
    {
        IRecipient Convert(Guid userId);
    }
}
