using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using NUnit.Framework;

namespace GmailClient
{
    [TestFixture]
    [Category("API")]
    public class UserAccessCodeFlow
    {
        private string[] Scopes = { GmailService.Scope.MailGoogleCom };
        private string ApplicationName = "Gmail Client";

        [Test]
        public async Task Gmail_GetMessages()
        {
            UserCredential credential;

            // note that this may prompt the user to sign in via OAuth
            using (var stream = new FileStream($"{TestContext.CurrentContext.TestDirectory}/client_secret.json", FileMode.Open, FileAccess.ReadWrite))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-client.json");

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None
                    );
            };

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            var response = await service.Users.Messages.List("me").ExecuteAsync();

            foreach (var message in response.Messages)
            {
                var msg = await service.Users.Messages.Get("me", message.Id).ExecuteAsync();
                Console.WriteLine($"{msg.Id} {msg.Payload.Headers.FirstOrDefault(h => h.Name == "Subject").Value}");
            }
        }
    }
}
