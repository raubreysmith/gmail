using System;
using NUnit.Framework;
using S22.Imap;

namespace GmailClient
{
    [TestFixture]
    [Category("Imap")]
    public class S22ImapTest
    {
        private string host = "imap.gmail.com";
        private string username = "email";
        private string password = "password";

        [Test]   
        public void S22_GetMessages()
        {
            using (var client = new ImapClient(host, 993, username, password, AuthMethod.Login, true))
            {
                var search = client.Search(SearchCondition.All());
                var messages = client.GetMessages(search);

                foreach (var message in messages)
                {
                    Console.WriteLine($"{message.Date()} {message.Subject}");
                }
            }
        }
    }
}