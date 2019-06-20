using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your name");
            string name = Console.ReadLine();
            try
            {
                GetData(name);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught ArgumentException: {ex.Message}");
            }

        }

        private static async void GetData(string name)
        {
            string baseUrl = "http://localhost:8080/api/test_cli?name=" + name;

            //sentence 'using' is to prevent memory leaks.
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseUrl))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(await Task.Run(() => Convert.ToString(content)));
                }
            }

        }
    }
}