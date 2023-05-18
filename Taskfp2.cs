using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace taskfp2
{
    public class Taskfp2
    {
        [FunctionName("Function1")]
        public async Task Run([ServiceBusTrigger("jqueue", Connection = "SB:ConnectionString")] string myQueueItem, ILogger log)
        {
            if (myQueueItem != null)
            {

                HttpClient client = new HttpClient();
                string url = Environment.GetEnvironmentVariable("Endpoint");
                try
                {

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        log.LogInformation($"the resoponse from http trigger:\n {responseBody}");

                    }
                    else
                    {
                        log.LogInformation($"Request failed with status code: {response.StatusCode}");
                    }

                }
                catch (Exception ex)
                {
                    log.LogInformation($"Error: {ex.Message}");
                }
                log.LogInformation(".......now the message from service bus stay tuned........");
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            }
        }
    }
}
