using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }


        public IHttpActionResult Post(OrderViewModel order)
        {
            return Ok(order);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }


    public class OrderViewModel
    {
        public string OrderNumber { get; set; }

        /*
         *basic info
            var orderNumber = order.OrderNumber;
            var orderDate = order.Created;
            var customerName = order.Name;
            var currencyCode = order.Currency.CurrencyCode;
            var currency = new Currency(currencyCode);

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
         *
         */
    }
}



/*
			//Mediachase.Commerce.Orders.PurchaseOrder
			
			
             *Create a EpiServer Ecommerce plugin which
                1. reads the order events
                2. for each order placed, find order number, order date, shipping date and
                a. find order details ( article number, sku number, product name, quantity, color, size, photo (url)
                b. who ordered? customer email or unique identification Id, address
                c. shipping method, shipping partner
                d. delivery date
                e. Purchase price per product

                3. Call a http://external.website.name.here.com/feed/order/<order-id>/ as a POST call with the above data packed as json data. 
                CustomerName	"admin@example.com"	string
		        BillingCurrency	"USD"	string

        -		EPiServer.Commerce.Order.IOrderGroup.Forms

			        DisCountAmount
			        Payment
						        CardType	"Credit card"	string
						        CreditCardNumber	"4662519843660534"	string
						        CreditCardSecurityCode	"212"	string
						        ExpirationMonth	11	int
						        ExpirationYear	2019	int


			        Shipment
						        ShippingTotal	20.000000000	decimal
						        ShippingMethodName	"Express-USD"	string
						        WarehouseCode	"stockholmstore"	string
						        -		EPiServer.Commerce.Order.IShipment.ShippingAddress	{Mediachase.Commerce.Orders.OrderAddress}	EPiServer.Commerce.Order.IOrderAddress {Mediachase.Commerce.Orders.OrderAddress}

									        City	"Ha noi"	string
									        CountryCode	"VNM"	string
									        EveningPhoneNumber	""	string
									        CountryName	"Viet Nam"	string
									        Email	"dangdd2@yahoo.com.vn"	string
									        Line1	"hanoi"	string
									        Organization	""	string
									        RegionCode	"ha noi"	string
									        PostalCode	"10001"	string
									        State	""	string




			        CouponCode
			        LineItems ({Mediachase.Commerce.Orders.LineItem})
						        Catalog	"Fashion"	string
						        CatalogNode	"shoes"	string
					            Code	"SKU-36127195"	string
						        DisplayName	"Faded Glory Mens Canvas Twin Gore Slip-On Shoe"
											"Puma Guida Moc SF Chase Mens Size 14 Red Leather Loafers Shoes UK   EU 48.5"
						        Quantity	1.0	decimal
						        ExtendedPrice	11.60	decimal
						        
						        
			        -		SystemFieldStorage[]
										        +		["BackorderQuantity"]	0	
										        +		["MinQuantity"]	1	
										        +		["PreorderQuantity"]	0	
										        +		["PlacedPrice"]	14.500000000	
										        +		["ExtendedPrice"]	11.60	
										        +		["ReturnQuantity"]	0	
										        +		["ListPrice"]	0	
										        +		["ReturnReason"]	null	
										        +		["ShippingAddressId"]	""	
										        +		["ShippingMethodName"]	null	
										        +		["OrigLineItemId"]	null	
										        +		["CatalogNode"]	"shoes"	
										        +		["Quantity"]	1.0	
										        +		["IsInventoryAllocated"]	true	
										        +		["InStockQuantity"]	316	
										        +		["Status"]	null	
										        +		["ParentCatalogEntryId"]	"P-36127195"	
										        +		["LineItemDiscountAmount"]	2.900000000	
										        +		["LineItemId"]	2221	
										        +		["ProviderId"]	null	
										        +		["MaxQuantity"]	100	
										        +		["LineItemOrdering"]	{11/5/2019 06:10:22}	
										        +		["ConfigurationId"]	null	
										        +		["ShippingMethodId"]	{00000000-0000-0000-0000-000000000000}	
										        +		["Epi_SalesTax"]	0.00	
										        +		["Description"]	null	
										        +		["OrderFormId"]	1006	
										        +		["Epi_TaxCategoryId"]	1	
										        +		["OrderGroupId"]	1006	
										        +		["CatalogEntryId"]	"SKU-36127195"	
										        +		["AllowBackordersAndPreorders"]	false	
										        +		["DisplayName"]	"Faded Glory Mens Canvas Twin Gore Slip-On Shoe"	
										        +		["WarehouseCode"]	null	
										        +		["OrderLevelDiscountAmount"]	0	
										        +		["Catalog"]	"Fashion"	
										        +		["InventoryStatus"]	1	

             *
             */
