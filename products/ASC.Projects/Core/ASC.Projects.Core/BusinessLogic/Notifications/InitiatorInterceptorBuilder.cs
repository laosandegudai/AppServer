using ASC.Core.Common.Utils;
using ASC.Notify;
using ASC.Projects.Core.BusinessLogic.Notifications.Interfaces;

namespace ASC.Projects.Core.BusinessLogic.Notifications
{
    public class InitiatorInterceptorBuilder : IInitiatorInterceptorBuilder
    {
        private readonly IDirectRecipientBuilder _directRecipientBuilder;

        public InitiatorInterceptorBuilder(IDirectRecipientBuilder directRecipientBuilder)
        {
            _directRecipientBuilder = directRecipientBuilder.NotNull(nameof(directRecipientBuilder));
        }

        public InitiatorInterceptor Build(string id,
            string name = null,
            string[] addresses = null,
            bool isActivationCheckRequired = false)
        {
            var directRecipient = _directRecipientBuilder
                .WithId(id)
                .WithName(name)
                .WithAddresses(addresses)
                .WithCheckActivation(isActivationCheckRequired)
                .Build();

            var interceptor = new InitiatorInterceptor(directRecipient);

            return interceptor;
        }
    }
}
