using Dc.EpiServerOrderPlugin.Handlers;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;


namespace Dc.EpiServerOrderPlugin.Infrastructure
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class PluginInitialization : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            context.Locate.Advanced.GetInstance<OrderEventListener>().AddEvents();
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.Locate.Advanced.GetInstance<OrderEventListener>().RemoveEvents();
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;

            services.AddSingleton<IOrderEventHandler, OrderEventHandler>();
        }
    }
}