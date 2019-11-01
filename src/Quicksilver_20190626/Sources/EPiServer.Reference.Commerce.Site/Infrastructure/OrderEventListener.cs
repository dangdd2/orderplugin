using EPiServer.Commerce.Order;
using EPiServer.Logging;
using Mediachase.Commerce.Catalog.Dto;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
    public class OrderEventListener
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderEvents _orderEvents;
        private readonly ILogger _logger = LogManager.GetLogger(typeof(OrderEventListener));

        public OrderEventListener(IOrderRepository orderRepository, IOrderEvents orderEvents)
        {
            _orderRepository = orderRepository;
            _orderEvents = orderEvents;
        }

        public void AddEvents()
        {
            _orderEvents.SavedOrder += OrderEventsOnSavedOrder;
            _orderEvents.DeletingOrder += OrderEventsOnDeletingOrder;
        }

        private void OrderEventsOnSavedOrder(object sender, OrderEventArgs orderEventArgs)
        {
            var po = orderEventArgs.OrderGroup as IPurchaseOrder;
            if (po != null)
            {
                _logger.Information($"Order {po.OrderNumber} was saved");
            }
        }


        /*
             *Create a EpiServer Ecommerce plugin which
                1. reads the order events
                2. for each order placed, find order number, order date, shipping date and
                a. find order details ( article number, sku number, product name, quantity, color, size, photo (url)
                b. who ordered? customer email or unique identification Id, address
                c. shipping method, shipping partner
                d. delivery date
                e. Purchase price per product

                3. Call a http://external.website.name.here.com/feed/order/<order-id>/ as a POST call with the above data packed as json data. 

             *
             */

        private void PopulateInfo(IPurchaseOrder order)
        {
            var orderNumber = order.OrderNumber;
            var orderDate = order.Created;
            var customerName = order.Name;
            var currencyCode = order.Currency.CurrencyCode;

        }

        private void OrderEventsOnDeletingOrder(object sender, OrderEventArgs orderEventArgs)
        {
            var cart = _orderRepository.Load<ICart>(orderEventArgs.OrderLink.OrderGroupId);
            if (cart != null)
            {
                _logger.Information($"Cart '{cart.Name}' for user '{cart.CustomerId}' is being deleted.");
            }
        }

        public void RemoveEvents()
        {
            _orderEvents.DeletingOrder -= OrderEventsOnDeletingOrder;
            _orderEvents.SavedOrder -= OrderEventsOnSavedOrder;
        }
    }
}