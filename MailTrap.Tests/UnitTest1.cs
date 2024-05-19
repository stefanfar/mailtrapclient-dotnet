using Mailtrap;
using System.Net;

namespace MailTrap.Tests
{
    [TestFixture]
    public class MailtrapClientTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SendAsync_WithValidMail_SendsMailSuccessfully()
        {
            var attachment = new Attachment()
            {
                Content = "PCFET0NUWVBFIGh0bWw+CjxodG1sIGxhbmc9ImVuIj4KCi" +
                "AgICA8aGVhZD4KICAgICAgICA8bWV0YSBjaGFyc2V0PSJVVEYtOCI+Ci" +
                "AgICAgICAgPG1ldGEgaHR0cC1lcXVpdj0iWC1VQS1Db21wYXRpYmxlIi" +
                "Bjb250ZW50PSJJRT1lZGdlIj4KICAgICAgICA8bWV0YSBuYW1lPSJ2aW" +
                "V3cG9ydCIgY29udGVudD0id2lkdGg9ZGV2aWNlLXdpZHRoLCBpbml0aW" +
                "FsLXNjYWxlPTEuMCI+CiAgICAgICAgPHRpdGxlPkRvY3VtZW50PC90aX" +
                "RsZT4KICAgIDwvaGVhZD4KCiAgICA8Ym9keT4KCiAgICA8L2JvZHk+Cg" +
                "o8L2h0bWw+Cg==",
                Filename = "index.html",
                Type = "text/plain",
                Disposition = "attachment"
            };

            var mail = new Mail
            {
                To = new List<Address> { new Address { Email = "4plx4.test@inbox.testmail.app", Name = "abc" } },
                From = new Address { Email = "stefan.farcas9@gmail.com", Name = "abc" },
                Subject = "subiect",
                Html = "<p>Congratulations on your order no. <strong>1234</strong>.</p>",
                Category = "API test",
                Attachments = new List<Attachment> { attachment }
            };

            var mailtrapClient = new MailtrapClient("28d19bd81273e770522fee5b9fbeb2ed");

            var mailResponse = await mailtrapClient.SendAsync(mail);

            Assert.That(mailResponse, Is.Not.Null);
            Assert.That(mailResponse.Success, Is.True);
            Assert.That(mailResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task SendAsync_WithMissingRequiredFields_ReturnsErrorMessageFromMailtrapServer()
        {
            var attachment = new Attachment()
            {
                Type = "text/plain",
                Disposition = "attachment"
            };

            var mail = new Mail
            {
                Text = "Mail text",
                Html = "<p>Congratulations on your order no. <strong>1234</strong>.</p>",
                Category = "API test",
                Attachments = new List<Attachment> { attachment }
            };

            var mailtrapClient = new MailtrapClient("28d19bd81273e770522fee5b9fbeb2ed");

            var mailResponse = await mailtrapClient.SendAsync(mail);

            Assert.That(mailResponse, Is.Not.Null);
            Assert.That(mailResponse.Success, Is.True);
            Assert.That(mailResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}