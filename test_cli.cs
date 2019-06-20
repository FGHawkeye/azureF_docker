using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureCLI
{
    public static class test_cli
    {
        [FunctionName("Calculate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            //Find the numbers with regular exp in the recieved body
            var numbers = Regex.Matches(requestBody, "[0-9]+").Cast<Match>().Select(m => m.Value).ToList();
            //Find the operators with regular exp in the recieved body
            var operators = Regex.Matches(requestBody, @"[+-\/*]").Cast<Match>().Select(m => m.Value).ToList();

            //Casting list to the correct data type
            var numbersToBeCalculated = numbers.ConvertAll(x => int.Parse(x));

            //Checking if math operators are equal or more than the numbers to be calculated
            if (operators.Count >= numbersToBeCalculated.Count)
            {
                return new BadRequestObjectResult("Wrong entry. Try again using one or more operations");
            }

            double result = numbersToBeCalculated.FirstOrDefault();

            var j = 0;
            foreach(int number in numbersToBeCalculated.Skip(1))
            {
                switch (operators[j])
                {
                    case "+":
                        {
                            result += number;
                            break;
                        }
                    case "-":
                        {
                            result -= number;
                            break;
                        }
                    case "*":
                        {
                            result *= number;
                            break;
                        }
                    case "/":
                        {
                            result /= number;
                            break;
                        }
                    default:
                        break;
                }
                j++;
            }
            return (ActionResult)new OkObjectResult($"Total: {result}");
        }
    }
}
