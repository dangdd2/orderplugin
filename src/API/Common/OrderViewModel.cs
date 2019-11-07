using System.Collections.Generic;

namespace Common
{
    public class OrderViewModel
    {

        public OrderInfo OrderInfo { get; set; }

        public Shipment Shipment { get; set; }

        public List<LineItem> LineItems { get; set; }

    }

    public class OrderInfo
    {
        public string OrderNumber { get; set; }

        public string OrderDate { get; set; }

        public string CustomerName { get; set; }

        public string CurrencyCode { get; set; }

        public string SubTotal { get; set; }

        public string HandlingTotal { get; set; }

        public string WarehouseCode { get; set; }
    }

    public class Shipment
    {
        public string ShipmentTrackingNumber { get; set; }

        public string ShippingMethodName { get; set; }

        public string ShippingAddress { get; set; }

        public string WarehouseCode { get; set; }

        public string City { get; set; }

        public string CountryCode { get; set; }

        public string DaytimePhoneNumber { get; set; }

        public string EveningPhoneNumber { get; set; }

        public string Email { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string RegionCode { get; set; }

        public string RegionName { get; set; }
    }

    public class LineItem
    {
        //product info
        public string Sku { get; set; }

        public string ProductName { get; set; }

        public string Quantity { get; set; }

        public string PlacedPrice { get; set; }

        public string ThumbnailUrl { get; set; }

        public string DiscountedPrice { get; set; }

        public string DiscountedTotal { get; set; }

        public string DiscountedValue { get; set; }

        public string FullUrl { get; set; }

        public string ExtendedPrice { get; set; }
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
