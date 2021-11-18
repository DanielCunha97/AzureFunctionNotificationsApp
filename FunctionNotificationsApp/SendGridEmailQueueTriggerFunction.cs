using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Mail;

namespace FunctionNotificationsApp
{
    public static class SendGridEmailQueueTriggerFunction
    {
        [FunctionName("SendGridEmailQueueTriggerFunction")]
        public static void Run([QueueTrigger("email-queue", Connection = "CloudStorageAccount")] string myQueueItem,
            [SendGrid(ApiKey = "SendgridAPIKey")] out SendGridMessage sendGridMessage,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            try
            {
                var queueItem = myQueueItem.ToString();

                dynamic jsonData = JObject.Parse(queueItem);
                string emailBody = jsonData.Content;

                sendGridMessage = new SendGridMessage
                {
                    From = new EmailAddress("cunhadaniel3197@gmail.com", "AzureFuncApps"),
                };
                sendGridMessage.AddTo("diogocunha97@gmail.com");
                sendGridMessage.SetSubject("Awesome Azure Function app");
                sendGridMessage.AddContent("text/html", emailBody);
                log.LogInformation($"Email Sent!!! With the body: {emailBody}");
            }
            catch (Exception ex)
            {
                sendGridMessage = new SendGridMessage();
                log.LogError($"Error occured while processing QueueItem {myQueueItem} , Exception - {ex.InnerException}");
            }
        }
    }
}
