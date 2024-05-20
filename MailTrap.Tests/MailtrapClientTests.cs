using Mailtrap;
using Mailtrap.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MailTrap.Tests
{
    [TestFixture]
    public class MailtrapClientTests
    {
        private string _token = "1ca2a4a7-fc0a-441f-b9f9-6633f2a246ef";
        private string _sendingEnpoint = "https://stoplight.io/mocks/railsware/mailtrap-api-docs/93404133/api/send";
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        [Test]
        public void SendAsync_WithNullArgument_ReturnsArgumentNullException()
        {
            var mailtrapClient = new MailtrapClient(_token);
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => mailtrapClient.SendAsync(null));

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'mail')"));
        }

        [Test]
        public void SendAsync_WithMissingRequiredFields_ReturnsArgumentException()
        {
            var attachment = new Attachment()
            {
                Type = "text/plain",
                Disposition = "attachment"
            };

            var mail = new Mail
            {
                Text = "Congratulations on your order no. 1234",
                Html = "<p>Congratulations on your order no. <strong>1234</strong>.</p>",
                Category = "API test",
                Attachments = new List<Attachment> { attachment }
            };

            var mailtrapClient = new MailtrapClient(_token);
            var exception = Assert.ThrowsAsync<ArgumentException>(() => mailtrapClient.SendAsync(mail));

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("The property To is required.\r\n" +
                "The property From is required.\r\n" +
                "The property Text should not be set because the property Html is set.\r\n" +
                "The property Content is required.\r\n" +
                "The property Filename is required. (Parameter 'mail')"));
        }

        [Test]
        public async Task SendAsync_WithValidMail_SendsMailSuccessfully()
        {
            var mail = CreateMail();

            var mailtrapOptions = new MailtrapOptions()
            {
                Token = "123456789"
            };

            var _serializationOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var mailResponse = new MailResponse
            {
                Success = true,
                MessageIds = ["0c7fd939-02cf-11ed-88c2-0a58a9feac02"]
            };

            var mailtrapClient = CreateMailtrapClient(mailtrapOptions, mailResponse);

            var actualMailResponse = await mailtrapClient.SendAsync(mail);

            Assert.That(actualMailResponse, Is.Not.Null);
            Assert.That(actualMailResponse.Success, Is.True);
            Assert.That(actualMailResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private MailtrapClient CreateMailtrapClient(MailtrapOptions mailtrapOptions, MailResponse mailResponse)
        {
            var serializedMailResponse = JsonSerializer.Serialize(mailResponse);

            _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                Content = new StringContent(serializedMailResponse)
            });

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var options = Options.Create(mailtrapOptions);
            var logger = new NullLogger<MailtrapClient>();

            return new MailtrapClient(httpClient, options, logger);
        }
        private static Mail CreateMail()
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
                To = new List<Address> { new Address { Email = "john_doe@example.com", Name = "John Doe" } },
                From = new Address { Email = "sales@example.com", Name = "Example Sales Team" },
                Subject = "Your Example Order Confirmation",
                Html = "<p>Congratulations on your order no. <strong>1234</strong>.</p>",
                Category = "API test",
                Attachments = new List<Attachment> { attachment }
            };
            return mail;
        }
    }
}