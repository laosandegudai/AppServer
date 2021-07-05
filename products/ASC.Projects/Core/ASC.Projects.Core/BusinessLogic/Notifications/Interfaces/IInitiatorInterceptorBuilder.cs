using ASC.Notify;

namespace ASC.Projects.Core.BusinessLogic.Notifications.Interfaces
{
    public interface IInitiatorInterceptorBuilder
    {
        InitiatorInterceptor Build(string id,
            string name = null,
            string[] addresses = null,
            bool isActivationCheckRequired = false);
    }
}
