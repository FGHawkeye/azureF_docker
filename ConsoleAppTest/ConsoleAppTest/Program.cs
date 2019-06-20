using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static IConfigurationRoot config;
        static void Main(string[] args)
        {
            //obtain the appsetings configs
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            config = builder.Build();

            InitMessage();
            var calculatorOn = true;
            while (calculatorOn)
            {
                calculatorOn = CalculatorOn();
            }
        }

        private static void InitMessage()
        {
            Console.WriteLine("This is simple calculator to test the app in a docker container, please enter any operation you would" +
             "\nlike to perform using any of the four operators and any integer number."+
             "\nIf you want to exit type 'EXIT' to leave.");
        }

        private static bool CalculatorOn()
        {
            var operation = Console.ReadLine();

            if (String.IsNullOrEmpty(operation))
            {
                const string message = "Type something! :)";
                Console.WriteLine(message);
                return true;
            }

            if (operation.ToLower() == "exit")
                return false;

            var result = CalculateAsync(operation);
            Console.WriteLine(result.Result);
            return true;
        }

        private static async Task<string> CalculateAsync(string operation)
        {
            string baseUrl = config["Base_Url"];

            //sentence 'using' is to prevent memory leaks.
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(baseUrl, new StringContent(operation)))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return await Task.Run(() => Convert.ToString(content));
                }
            }

        }
    }
}