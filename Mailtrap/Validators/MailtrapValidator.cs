using System.Collections.Generic;
using System.Linq;

namespace Mailtrap.Validators
{
    public static class MailtrapValidator
    {
        private static MailValidator _context = new MailValidator();

        public static bool TryValidateMail(Mail mail, out ICollection<string> validationResults)
        {
            var results = _context.Validate(mail);

            validationResults = !results.IsValid ? results.Errors.Select(e => e.ErrorMessage).ToList() : null;

            return results.IsValid;
        }
    }
}
