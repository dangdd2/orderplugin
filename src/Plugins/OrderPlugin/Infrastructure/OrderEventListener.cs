using Dc.EpiServerOrderPlugin.Handlers;
using EPiServer.Commerce.Order;

namespace Dc.EpiServerOrderPlugin.Infrastructure
{
    public class OrderEventListener : OrderEventHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderEvents _orderEvents;

        /// <summary>
        /// Order Event Listener
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="orderEvents"></param>
        public OrderEventListener(IOrderRepository orderRepository, IOrderEvents orderEvents)
        {
            _orderRepository = orderRepository;
            _orderEvents = orderEvents;
        }

        /// <summary>
        /// AddEvents
        /// </summary>
        public void AddEvents()
        {
            _orderEvents.SavedOrder += OrderEventsOnSavedOrder;
            _orderEvents.DeletingOrder += OrderEventsOnDeletingOrder;
        }

        /// <summary>
        /// OrderEventsOnSavedOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="orderEventArgs"></param>
        private void OrderEventsOnSavedOrder(object sender, OrderEventArgs orderEventArgs)
        {
            var po = orderEventArgs.OrderGroup as IPurchaseOrder;
            if (po != null)
            {
                base.CallOrderRestAPI(po);
            }
        }
        
        /// <summary>
        /// OrderEventsOnDeletingOrder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="orderEventArgs"></param>
        private void OrderEventsOnDeletingOrder(object sender, OrderEventArgs orderEventArgs)
        {
            var cart = _orderRepository.Load<ICart>(orderEventArgs.OrderLink.OrderGroupId);
            if (cart != null)
            {
                
            }
        }

        /// <summary>
        /// Remove Events
        /// </summary>
        public void RemoveEvents()
        {
            _orderEvents.DeletingOrder -= OrderEventsOnDeletingOrder;
            _orderEvents.SavedOrder -= OrderEventsOnSavedOrder;
        }
    }
}