using System;
using System.Linq;
using Dc.EpiServerOrderPlugin.Extensions;
using EPiServer.Commerce.Order;
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
            // check setting enable/disable
            bool.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings.Get("EPi.OrderIntegration.IsActive"), out var isActive);
            if (!isActive)
            {
                return;
            }

            Logger.Information($"Order has been placed: {order.OrderNumber}");

            //basic info
            var customerId = order.CustomerId;
            var orderNumber = order.OrderNumber;
            var orderDate = order.Created;
            var customerName = order.Name;
            var currencyCode = order.Currency.CurrencyCode;
            var currency = new Mediachase.Commerce.Currency(currencyCode);


            var form = order.Forms.FirstOrDefault();
            //var subTotal = form.GetSubTotal(currency);
            //var handlingTotal = form.GetHandlingTotal(currency);
            //var couponCodes = form.CouponCodes;
            //var formName = form.Name;

            var payment = form.Payments.FirstOrDefault();
            string billingCustomerName = "",
                    billingLine1 = "",
                    billingLine2 = "",
                    billingCity = "",
                    billingCountryCode = "",
                    billingEmail = "",
                    billingRegionCode = "";

            if (payment != null)
            {
                billingCustomerName = payment.CustomerName;
                var billingAddress = payment.BillingAddress;
                if (billingAddress != null)
                {
                    billingCity = billingAddress.City;
                    billingCountryCode = billingAddress.CountryCode;
                    billingEmail = billingAddress.Email;
                    billingLine1 = billingAddress.Line1;
                    billingLine2 = billingAddress.Line2;
                    billingRegionCode = billingAddress.RegionCode;
                }
            }

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
            var allListItems = order.GetAllLineItems();

            var lineItems = allListItems.Select(lineItem =>
            {
                var extraInfo = lineItem.GetExtraInfo();

                var catList = extraInfo.Item3;
                string category1 = string.Empty,
                    category2 = string.Empty,
                    category3 = string.Empty;
                if (catList.Length > 0) category1 = catList[0];
                if (catList.Length > 1) category2 = catList[1];
                if (catList.Length > 2) category3 = catList[2];
                string size = extraInfo.Item1;
                string color = extraInfo.Item2;

                return new
                {
                    sku_number = lineItem.Code,
                    original_price = lineItem.PlacedPrice,
                    selling_price = lineItem.GetExtendedPrice(currency).ToString(),
                    product_id = lineItem.LineItemId,
                    product_name = lineItem.DisplayName,
                    model = "",
                    color = color,
                    size = size,
                    quantity = lineItem.Quantity,
                    category_1 = category1,
                    category_2 = category2,
                    category_3 = category3,
                    content_url = lineItem.GetFullUrl(),
                    description = "",
                    extra = "",
                    barcode_number = "",
                    product_page_url = "",
                };
            }).ToList();

            //string url = "http://localhost:61409/api";
            //string resource = "/values";

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
                ext_order_id = orderNumber,
                currency_code = currencyCode,
                order_date = orderDate,
                ship_date = "",
                email = email,
                billing_customer_obj = new
                {
                    ext_customer_id = customerId,
                    name = billingCustomerName,
                    address = billingLine1,
                    city = billingCity,
                    country_code = billingCountryCode,
                    email = billingEmail,
                    zipcode = billingRegionCode
                },
                shipping_customer_obj = new
                {
                    ext_customer_id = customerId,
                    name = customerName,
                    address = line1,
                    city = city,
                    country_code = countryCode,
                    email = email,
                    zipcode = regionCode
                },

                details_obj_list = lineItems
            });

            IRestResponse restResponse = null;

            try
            {
                restResponse = restClient.Execute(restRequest);
            }
            catch (Exception ex)
            {
                Logger.Debug(" REST API Error Message : " + ex.Message);
                if (ex.InnerException != null) Logger.Debug(" REST API Inner Exception : " + ex.InnerException.Message);
            }
            finally
            {
                if (restResponse != null) Logger.Debug(" REST API response content : " + restResponse.Content);
            }
        }

    }
}

/*
 *
 * {
    "ext_order_id": "OGrandpa_2100002",
    "currency_code": "SEK",
    "order_date": "2019-11-03 23:00:00",
    "ship_date": "2019-11-03 23:00:00",
    "billing_customer_obj": {
      "ext_customer_id": "137435",
      "name": "John Palm Wennerberg",
      "address": "F\u00e5gelstav\u00e4gen 23 ",
      "city": "Stockholm",
      "country_code": "SE",
      "email": "john@wilhlm.com",
      "zipcode": "12433"
    },
    "shipping_customer_obj": {
      "ext_customer_id": "137435",
      "name": "John Palm Wennerberg",
      "address": "F\u00e5gelstav\u00e4gen 23 ",
      "city": "Stockholm",
      "country_code": "SE",
      "email": "john@wilhlm.com",
      "zipcode": "12433"
    },
    "details_obj_list": [
      {
        "sku_number": "100382",
        "original_price": 200.0,
        "selling_price": 200.0,
        "product_id": "109335",
        "product_name": "Cotton Rib Socks 2-pack",
        "model": "Grandpa Soft Goods",
        "color": "Black",
        "size": "40-45",
        "quantity": 1,
        "category_1": "Accessoarer",
        "category_2": "Strumpor",
        "category_3": "Grandpa Soft Goods",
        "content_url": "http://www.grandpastore.se/bilder/artiklar/109335_DarkNavy.jpg",
        "description": "H\u00f6gkvalikativa strumpor fr\u00e5n Grandpa Soft Goods. 2-packEkologisk ribbstickad bomullTillverkade i Europa",
        "extra": "Cotton Rib Socks 2-pack Dark Navy, 40-45 Strumpor/Strumpbyxor Accessoarer &gt; Strumpor/Strumpbyxor",
        "barcode_number": "7340191300286",
        "product_page_url": "http://www.grandpastore.se/sv/cotton-rib-socks-2-pack-dark-navy"
      }
    ],
    "email": "john@wilhlm.com"
  },

 */
