using EPiServer.Commerce.Order;

namespace Dc.EpiServerOrderPlugin.Handlers
{
    public interface IOrderEventHandler
    {
        void PostEvent(IPurchaseOrder order);
    }
}