using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;
using RestSharp;

namespace TestWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            CallByRestSharp();

            //dynamic carDynamic = new ExpandoObject();
            //carDynamic.OrderNumber = "PO1234";
            //PostCarAsync(carDynamic);

            Console.WriteLine(" - done- ");
            Console.ReadLine();
        }

        private static void CallByRestSharp()
        {
            string url = "http://localhost:61409/api";
            string resource = "/values";
            RestClient restClient = new RestClient(url);

            RestRequest restRequest = new RestRequest(resource, Method.POST);

            //Specifies request content type as Json
            restRequest.RequestFormat = DataFormat.Json;

            //Create a body with specifies parameters as json
#pragma warning disable 618
            restRequest.AddBody(PopulateOrderViewModel());

           IRestResponse restResponse = restClient.Execute(restRequest);
           Console.WriteLine(restResponse.Content);
        }

        private static OrderViewModel PopulateOrderViewModel()
        {
            return new OrderViewModel
            {
                OrderInfo = new OrderInfo
                {
                    OrderNumber = "PO1234",
                    CurrencyCode = "USD",
                    CustomerName = "admin@example.com",
                    WarehouseCode = "stockholmstore",
                    HandlingTotal = "300000",
                    OrderDate = DateTime.Now.ToString(),
                    SubTotal = "250000"
                },
                Shipment = new Shipment
                {
                    City = "Hanoi",
                    CountryCode = "VNM",
                    EveningPhoneNumber = "",
                    DaytimePhoneNumber = "09861121212",
                    RegionCode = "10001",
                    RegionName = "Viet Nam",
                    Email = "dangdd2@yahoo.com.vn",
                    Line1 = "165 Thai Ha",
                    Line2 = "",
                    ShipmentTrackingNumber = "",
                    ShippingAddress = "",
                    ShippingMethodName = "Express-USD",
                },
                LineItems = new List<LineItem>()
                {
                    new LineItem
                    {
                        Sku = "SKU-36127195",
                        DiscountedPrice = "9.9",
                        DiscountedTotal = "0.1",
                        ExtendedPrice = "9.9",
                        FullUrl = "",
                        PlacedPrice = "10",
                        ProductName = "Faded Glory Mens Canvas Twin Gore Slip-On Shoe",
                        Quantity = "1",
                        ThumbnailUrl = ""
                    },
                    new LineItem
                    {
                        Sku = "SKU-46127195",
                        DiscountedPrice = "9.9",
                        DiscountedTotal = "0.1",
                        ExtendedPrice = "9.9",
                        FullUrl = "",
                        PlacedPrice = "10",
                        ProductName = "Faded Glory Mens Canvas Twin Gore Slip-On Shoe",
                        Quantity = "1",
                        ThumbnailUrl = ""
                    }
                }
            };
        }


        static async Task PostCarAsync(dynamic car)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:61409/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(car), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("/api/values", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Data posted");
            }
            else
            {
                Console.WriteLine($"Failed to poste data. Status code:{response.StatusCode}");
            }
        }
    }
}
