using System.Net;
using System.Net.Mail;
using ImapX;
using NUnit.Framework;

namespace GmailClient
{
    [TestFixture]
    [Category("Imap")]
    public class ImapXTest
    {
        private string host = "imap.gmail.com";
        private string username = "email";
        private string password = "password";
        private ImapClient imapClient;

        [OneTimeSetUp]
        public void Setup()
        {
            // populate our inbox with a few emails for testing
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.Send(username, "email1@test.com", "Lorem ipsum", "Lorem ipsum dolor sit amet, consectetuer adipiscin");
                smtpClient.Send(username, "email2@test.com", "Kafka", "One morning, when Gregor Samsa woke from troubled");
                smtpClient.Send(username, "email3@test.com", "Pangram", "The quick, brown fox jumps over a lazy dog");
            }

            // setup the imap client for accessing our inbox
            imapClient = new ImapClient(host, 993, true);
            imapClient.Behavior.AutoPopulateFolderMessages = true;
            imapClient.Connect();
            imapClient.Login(username, password);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            // empty the inbox
            imapClient.Folders.Inbox.EmptyFolder();

            // close down our client
            imapClient.Disconnect();
        }

        [Test]
        public void ImapX_GetMessages()
        {
            var inbox = imapClient.Folders.Inbox;
            Assert.That(inbox.Exists == 3, "Should be a total of 3 emails as per test setup");
        }

        [Test]
        public void ImapX_SearchMessages()
        {
            var inbox = imapClient.Folders.Inbox;

            // see https://tools.ietf.org/html/rfc3501#section-6.4.4 for list of search criteria
            // e.g. inbox.Search("<criterion> <query>")
            var search = inbox.Search("SUBJECT Kafka");

            Assert.That(search.Length == 1, "Expected a single matching email");
            Assert.That(search[0].To[0].Address, Is.EqualTo("email2@test.com"));
        }
    }
}
