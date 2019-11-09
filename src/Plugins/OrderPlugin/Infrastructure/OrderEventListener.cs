using Dc.EpiServerOrderPlugin.Handlers;
using EPiServer.Commerce.Order;
using EPiServer.Logging;

namespace Dc.EpiServerOrderPlugin.Infrastructure
{
    public class OrderEventListener : OrderEventHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderEvents _orderEvents;
        private ILogger _logger = LogManager.GetLogger(typeof(OrderEventListener));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="orderEvents"></param>
        public OrderEventListener(IOrderRepository orderRepository, IOrderEvents orderEvents)
        {
            _orderRepository = orderRepository;
            _orderEvents = orderEvents;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddEvents()
        {
            _orderEvents.SavedOrder += OrderEventsOnSavedOrder;
            _orderEvents.DeletingOrder += OrderEventsOnDeletingOrder;
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="orderEventArgs"></param>
        private void OrderEventsOnDeletingOrder(object sender, OrderEventArgs orderEventArgs)
        {
            var cart = _orderRepository.Load<ICart>(orderEventArgs.OrderLink.OrderGroupId);
            if (cart != null)
            {
                _logger.Information($"Cart '{cart.Name}' for user '{cart.CustomerId}' is being deleted.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveEvents()
        {
            _orderEvents.DeletingOrder -= OrderEventsOnDeletingOrder;
            _orderEvents.SavedOrder -= OrderEventsOnSavedOrder;
        }
    }
}