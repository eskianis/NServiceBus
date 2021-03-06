using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus.Config;
using NServiceBus.Hosting.Roles;
using NServiceBus.Unicast.Config;

namespace NServiceBus.Hosting.Azure.Roles.Handlers
{
    /// <summary>
    /// Handles configuration related to the server role
    /// </summary>
    public class WorkerRoleHandler : IConfigureRole<AsA_Worker>, IWantTheEndpointConfig
    {
        /// <summary>
        /// Configures the UnicastBus with typical settings for a server on azure
        /// </summary>
        /// <param name="specifier"></param>
        /// <returns></returns>
        public ConfigUnicastBus ConfigureRole(IConfigureThisEndpoint specifier)
        {
            var instance = Configure.Instance;

            if (RoleEnvironment.IsAvailable && !IsHostedIn.ChildHostProcess())
            {
                instance.AzureConfigurationSource();
            }

            return instance
                .JsonSerializer()
                .IsTransactional(true)
                .Sagas()
                .DoNotUseDistributors()
                .UnicastBus()
                    .ImpersonateSender(false);
        }


        public IConfigureThisEndpoint Config { get; set; }
    }
}