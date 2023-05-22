using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace taskfp2
{
    public class Taskfp2
    {
        [FunctionName("Function1")]
        public async Task Run([ServiceBusTrigger("jqueue", Connection = "SB:ConnectionString")] string myQueueItem, ILogger log)
        {
            if (myQueueItem != null)
            {

               // HttpClient _client = new HttpClient();
                
                var keyVaultName = Environment.GetEnvironmentVariable("AzureKeyVaultName");
                var credential = new DefaultAzureCredential();
                var client = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"), credential);
                var secret = await client.GetSecretAsync("taskfp2");
                var secretValue = secret.Value.Value;
                string url = secretValue;
                log.LogInformation("the secret value from queue\n");
                log.LogInformation(url);
               /* try
                {
                    HttpResponseMessage response = await _client.GetAsync(url);

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

                } */
                log.LogInformation($"the resoponse from http trigger:\n {secretValue}");
                log.LogInformation(".......now the message from service bus stay tuned........");
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            }
        }
    }
}
