using ASC.Notify.Recipients;

namespace ASC.Projects.Core.BusinessLogic.Notifications.Interfaces
{
    public interface IDirectRecipientBuilder
    {
        IDirectRecipientBuilder WithId(string id);

        IDirectRecipientBuilder WithName(string name);

        IDirectRecipientBuilder WithAddresses(string[] addresses);

        IDirectRecipientBuilder WithCheckActivation(bool isActivationCheckRequired);

        DirectRecipient Build();
    }
}
