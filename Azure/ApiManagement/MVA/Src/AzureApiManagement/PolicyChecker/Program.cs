using Sychev.AzureApiManagement.Api.Models;
using Sychev.AzureApiManagement.DataModel;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PolicyChecker
{
    class Program
    {
        private const int Quota = 100;

        private const string Uri =
            "https://mvaapimanagement.azure-api.net/Document?subscription-key=b7833375a1fb4df6bcc146b0b99241cf";

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }
        static async Task RunAsync()
        {
            for (int i = 0; i < Quota; i++)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Uri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("");
                    if (response.IsSuccessStatusCode)
                    {
                        var responce = await response.Content.ReadAsAsync<GenericResponce<Document>>();
                        Console.WriteLine("Call number: {0}, id= {1}", i, responce.Data.First().Id);   
                    }
                    else
                    {
                        Console.WriteLine("Request status code : {0}", response.StatusCode);
                        Console.Read();     
                    }
                }
            }
        }
    }
}
