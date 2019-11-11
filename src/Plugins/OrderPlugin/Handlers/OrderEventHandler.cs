using System.Linq;
using Dc.EpiServerOrderPlugin.Extensions;
using EPiServer.Commerce.Order;
using EPiServer.Commerce.Order.Internal;
using EPiServer.Logging;
using RestSharp;

namespace Dc.EpiServerOrderPlugin.Handlers
{
    public class OrderEventHandler : IOrderEventHandler
    {

        private static readonly ILogger Logger = LogManager.GetLogger();
        /// <summary>
        /// Call OrderRest API
        /// </summary>
        /// <param name="order"></param>
        public void PostEvent(IPurchaseOrder order)
        {
            Logger.Information($"Order has been placed: {order.OrderNumber}");

            //basic info
            var orderNumber = order.OrderNumber;
            var orderDate = order.Created;
            var customerName = order.Name;
            var currencyCode = order.Currency.CurrencyCode;
            var currency = new Mediachase.Commerce.Currency(currencyCode);
            var form = order.Forms.FirstOrDefault();
            var subTotal = form.GetSubTotal(currency);
            var handlingTotal = form.GetHandlingTotal(currency);

            //var couponCodes = form.CouponCodes;
            //var formName = form.Name;
            //var payment = form.Payments.FirstOrDefault();

            //shipment info
            var shipment = form.Shipments.FirstOrDefault();
            var warehouseCode = shipment.WarehouseCode;
            var shipmentTrackingNumber = shipment.ShipmentTrackingNumber;
            var shippingMethodName = shipment.ShippingMethodName;

            var shipAdress = shipment.ShippingAddress;
            var city = shipAdress.City;
            var countryCode = shipAdress.CountryCode;
            var daytimePhoneNumber = shipAdress.DaytimePhoneNumber;
            var eveningPhoneName = shipAdress.EveningPhoneNumber;
            var email = shipAdress.Email;
            var line1 = shipAdress.Line1;
            var line2 = shipAdress.Line2;
            var organization = shipAdress.Organization;
            var regionCode = shipAdress.RegionCode;
            var regionName = shipAdress.RegionName;


            //lineItems
            var lineItems = order.GetAllLineItems().Select(lineItem => new
            {
                Code = lineItem.Code,
                DisplayName = lineItem.DisplayName,
                Quantity = lineItem.Quantity,
                PlacedPrice = lineItem.PlacedPrice,
                ThumbnailUrl = lineItem.GetThumbnailUrl(),
                DiscountedPrice = lineItem.GetDiscountedPrice(currency).ToString(),
                DiscountTotal = lineItem.GetDiscountTotal(currency).ToString(),
                OrderDiscountValue = lineItem.GetOrderDiscountValue(),
                FullUrl = lineItem.GetFullUrl(),
                ExtendedPrice = lineItem.GetExtendedPrice(currency).ToString()

            }).ToList();
            

            string url = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("EPi.OrderIntegration.Url");
            string resource = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("EPi.OrderIntegration.Resource");
            string apiKey = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("EPi.OrderIntegration.ApiKey");

            
            if (string.IsNullOrEmpty(url)) return;

            RestClient restClient = new RestClient(url);
            RestRequest restRequest = new RestRequest(resource, Method.POST);

            //Specifies request content type as Json
            restRequest.RequestFormat = DataFormat.Json;

            //Create a body with specifies parameters as json
            restRequest.AddJsonBody(new
            {
                OrderInfo = new
                {
                    OrderNumber = orderNumber,
                    CurrencyCode = currencyCode,
                    CustomerName = customerName,
                    WarehouseCode = warehouseCode,
                    HandlingTotal = handlingTotal.ToString(),
                    OrderDate = orderDate,
                    SubTotal = subTotal.ToString()
                },
                Shipment = new
                {
                    City = city,
                    CountryCode = currencyCode,
                    EveningPhoneNumber = eveningPhoneName,
                    DaytimePhoneNumber = daytimePhoneNumber,
                    RegionCode = regionCode,
                    RegionName = regionName,
                    Email = email,
                    Line1 = line1,
                    Line2 = line2,
                    ShipmentTrackingNumber = shipmentTrackingNumber,
                    ShippingMethodName = shippingMethodName
                },
                LineItems = lineItems
            });

            IRestResponse restResponse = restClient.Execute(restRequest);
        }

    }
}