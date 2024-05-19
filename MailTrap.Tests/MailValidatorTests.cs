using Mailtrap;
using System.Net;

namespace MailTrap.Tests
{
    [TestFixture]
    public class MailValidatorTests
    {
        [Test]
        public void TryValidateMail_WithMissingRequiredFields_ReturnsFalseAndErrorMessages()
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

            var isValid = MailtrapValidator.TryValidateMail1(mail, out var errors);

            Assert.That(isValid, Is.False);
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(5));
            Assert.That(errors.ElementAt(0), Is.EqualTo("The property To is required"));
            Assert.That(errors.ElementAt(1), Is.EqualTo("The property From is required"));
            Assert.That(errors.ElementAt(2), Is.EqualTo("The property Text should not be set because the property Html is set"));
            Assert.That(errors.ElementAt(3), Is.EqualTo("The property Content is required"));
            Assert.That(errors.ElementAt(4), Is.EqualTo("The property Filename is required"));
        }
    }
}