using System.Linq;
using Dc.EpiServerOrderPlugin.Extensions;
using EPiServer.Commerce.Order;
using EPiServer.Commerce.Order.Internal;
using RestSharp;

namespace Dc.EpiServerOrderPlugin.Handlers
{
    public class OrderEventHandler
    {
        /// <summary>
        /// Call OrderRest API
        /// </summary>
        /// <param name="order"></param>
        protected void CallOrderRestAPI(IPurchaseOrder order)
        {
            //basic info
            var orderNumber = order.OrderNumber;
            var orderDate = order.Created;
            var customerName = order.Name;
            var currencyCode = order.Currency.CurrencyCode;
            var currency = new Mediachase.Commerce.Currency(currencyCode);

            var form = order.Forms.FirstOrDefault();

            var formName = form.Name;
            var subTotal = form.GetSubTotal(currency);
            var handlingTotal = form.GetHandlingTotal(currency);
            var couponCodes = form.CouponCodes;

            var payment = form.Payments.FirstOrDefault();

            //shipment info
            var shipment = form.Shipments.FirstOrDefault();
            var warehouseCode = shipment.WarehouseCode;
            var shipmentTrackingNumber = shipment.ShipmentTrackingNumber;
            var shippingMethodName = shipment.ShippingMethodName;

            var shipAdress = shipment.ShippingAddress;
            var city = shipAdress.City;
            var countryCode = shipAdress.CountryCode;
            var dayPhoneName = shipAdress.DaytimePhoneNumber;
            var eveningPhoneName = shipAdress.EveningPhoneNumber;
            var email = shipAdress.Email;
            var line1 = shipAdress.Line1;
            var line2 = shipAdress.Line2;
            var organization = shipAdress.Organization;
            var regionCode = shipAdress.RegionCode;
            var regionName = shipAdress.RegionName;


            //lineItems
            var lineItems = order.GetAllLineItems();
            var lineItem = lineItems.FirstOrDefault();

            var sku = lineItem.Code;
            var productName = lineItem.DisplayName;
            var quantity = lineItem.Quantity;
            var placedPrice = lineItem.PlacedPrice;

            var thumbnailUrl = lineItem.GetThumbnailUrl();
            var discountedPrice = lineItem.GetDiscountedPrice(currency);
            var discountedTotal = lineItem.GetDiscountTotal(currency);
            var discountedValue = lineItem.GetOrderDiscountValue();
            var fullUrl = lineItem.GetFullUrl();
            var extendedPrice = lineItem.GetExtendedPrice(currency);

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
                    CurrencyCode = currencyCode
                }
            });

            IRestResponse restResponse = restClient.Execute(restRequest);
        }

    }
}