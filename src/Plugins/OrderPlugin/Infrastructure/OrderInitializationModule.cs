using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Dc.EpiServerOrderPlugin.Infrastructure
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class OrderInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            context.Locate.Advanced.GetInstance<OrderEventListener>().AddEvents();
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.Locate.Advanced.GetInstance<OrderEventListener>().RemoveEvents();
        }
    }
}