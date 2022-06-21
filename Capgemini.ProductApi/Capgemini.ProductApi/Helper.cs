using Azure;
using Azure.Messaging.EventGrid;
using Azure.Storage.Blobs;
using Capgemini.ProductApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.ProductApi
{
    public class Helper
    {
        private static object ticket;

        public static async Task<bool> UploadBlob(
        IConfiguration config,
        Product product)
        {
            string blobConnString = config.GetConnectionString("StorAccConnString");
            BlobServiceClient client = new BlobServiceClient(blobConnString);
            string container = config.GetValue<string>("Container");
            var containerClient = client.GetBlobContainerClient(container);



            string fileName = "pms.product." + Guid.NewGuid().ToString() + ".json";
            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);



            //memorystream
            using (var stream = new MemoryStream())
            {
                var serializer = JsonSerializer.Create(new JsonSerializerSettings());



                // Use the 'leave open' option to keep the memory stream open after the stream writer is disposed
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    // Serialize the job to the StreamWriter
                    serializer.Serialize(writer, ticket);
                }



                // Rewind the stream to the beginning
                stream.Position = 0;



                // Upload the job via the stream
                await blobClient.UploadAsync(stream, overwrite: true);
            }



            await PublishToEventGrid(config, product);
            return true;
        }

        private static Task PublishToEventGrid(IConfiguration config, object ticket)
        {
            throw new NotImplementedException();
        }

        private static async Task PublishToEventGrid(
        IConfiguration config, Product product)
        {
            var endpoint = config.GetValue<string>("EventGridTopicEndpoint");
            var accessKey = config.GetValue<string>("EventGridAccessKey");



            EventGridPublisherClient client = new EventGridPublisherClient(
            new Uri(endpoint),
            new AzureKeyCredential(accessKey));



            var event1 = new EventGridEvent(
            "PMS",
            "PMS.ProductEvent",
            "1.0",
            JsonConvert.SerializeObject(product));
            event1.Id = (new Guid()).ToString();
            event1.EventTime = DateTime.Now;
            //event1.Topic = "/subscriptions/73d972cd-c4c3-4ec5-9443-661a57525a5d/resourceGroups/rg-training/providers/Microsoft.EventGrid/topics/omsegt";
            event1.Topic = config.GetValue<string>("EventGridTopic");
            List<EventGridEvent> eventsList = new List<EventGridEvent>
{
event1
};



            // Send the events
            await client.SendEventsAsync(eventsList);
        }

        internal static Task UploadBlob(object config, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
