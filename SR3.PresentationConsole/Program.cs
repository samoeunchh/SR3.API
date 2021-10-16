using SR3.DaraLayer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SR3.PresentationConsole
{
    class Program
    {
        static readonly HttpClient client = new();
        static void Main()
        {
            client.BaseAddress = new Uri("http://localhost:48459/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine("===Starting====");
            //Post Customer data
            var customer = new Customer
            {
                CustomerName="Mr.John",
                Gender="Male",
                Address="Paris",
                Mobile="1112222"
            };
            PostCustomerAsync(customer).Wait();
            //Get Customer Data
            GetCustomerAsync().Wait();
            Console.WriteLine("=====Finished=====");
            Console.ReadKey();
        }
        static async Task GetCustomerAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/customers");
            if (response.IsSuccessStatusCode)
            {
                var customers = await response.Content.ReadAsAsync<List<Customer>>();
                if (customers == null) Console.WriteLine("No record");
                foreach(var item in customers)
                {
                    Console.WriteLine("---Customer Name:{0}",item.CustomerName);
                    
                }
            }
            else
            {
                var errormsg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine(errormsg);
            }
        }
        static async Task PostCustomerAsync(Customer customer)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<Customer>("api/customers", customer);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("=====Record was saved=====");
            }
            else
            {
                var errormsg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine(errormsg);
            }
        }
    }
}
