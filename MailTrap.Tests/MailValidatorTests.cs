using Mailtrap.Entities;
using Mailtrap.Validators;

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
                Text = "Congratulations on your order no. 1234",
                Html = "<p>Congratulations on your order no. <strong>1234</strong>.</p>",
                Category = "API test",
                Attachments = new List<Attachment> { attachment }
            };

            var isValid = MailtrapValidator.TryValidateMail(mail, out var errors);

            Assert.That(isValid, Is.False);
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(5));
            Assert.That(errors.ElementAt(0), Is.EqualTo("The property To is required."));
            Assert.That(errors.ElementAt(1), Is.EqualTo("The property From is required."));
            Assert.That(errors.ElementAt(2), Is.EqualTo("The property Text should not be set because the property Html is set."));
            Assert.That(errors.ElementAt(3), Is.EqualTo("The property Content is required."));
            Assert.That(errors.ElementAt(4), Is.EqualTo("The property Filename is required."));
        }
    }
}