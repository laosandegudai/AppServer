using System.Linq;
using ASC.Notify.Recipients;
using ASC.Projects.Core.BusinessLogic.Notifications.Interfaces;

namespace ASC.Projects.Core.BusinessLogic.Notifications
{
    public class DirectRecipientBuilder : IDirectRecipientBuilder
    {
        private string _id;

        private string _name;

        private string[] _addresses;

        private bool _isActivationCheckRequired;

        public IDirectRecipientBuilder WithId(string id)
        {
            _id = id;

            return this;
        }

        public IDirectRecipientBuilder WithName(string name)
        {
            _name = name;

            return this;
        }

        public IDirectRecipientBuilder WithAddresses(string[] addresses)
        {
            _addresses = addresses;

            return this;
        }

        public IDirectRecipientBuilder WithCheckActivation(bool isActivationCheckRequired)
        {
            _isActivationCheckRequired = isActivationCheckRequired;

            return this;
        }

        public DirectRecipient Build()
        {
            var directRecipient = new DirectRecipient(_id,
                _name ?? string.Empty,
                _addresses?.ToArray())
            {
                CheckActivation = _isActivationCheckRequired
            };

            return directRecipient;
        }
    }
}
