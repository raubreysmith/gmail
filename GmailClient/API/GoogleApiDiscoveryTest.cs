using System;
using System.Threading.Tasks;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Services;
using NUnit.Framework;

namespace GmailClient
{
    [TestFixture]
    [Category("API")]
    public class GoogleApiDiscoveryTest
    {
        private string apiKey = "key-goes-here"; 

        [Test]
        public async Task Discovery_GetApiList()
        {
            Console.WriteLine("Discovery API Sample");
            Console.WriteLine("====================");
            try
            {
                // Create the service.
                var service = new DiscoveryService(new BaseClientService.Initializer
                {
                    ApplicationName = "Discovery Sample",
                    ApiKey = apiKey,
                });

                // Run the request.
                var result = await service.Apis.List().ExecuteAsync();

                // Display the results.
                if (result.Items != null)
                {
                    foreach (DirectoryList.ItemsData api in result.Items)
                    {
                        Console.WriteLine(api.Id + " - " + api.Title);
                    }
                }
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
        }
    }
}
