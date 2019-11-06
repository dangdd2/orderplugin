using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace TestWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {

            CallByRestSharp();


            dynamic carDynamic = new ExpandoObject();
            carDynamic.OrderNumber = "PO1234";
            
            PostCarAsync(carDynamic);


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
            restRequest.AddBody(new
            {
                OrderNumber = "1234"
            });

            IRestResponse restResponse = restClient.Execute(restRequest);


            Console.WriteLine(restResponse);
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
