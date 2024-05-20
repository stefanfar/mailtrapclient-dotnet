using FluentValidation;

namespace Mailtrap.Validators
{
    internal class MailValidator : AbstractValidator<Mail>
    {
        public MailValidator()
        {
            RuleFor(x => x.To).NotNull().WithMessage(Constants.ToIsRequired);
            RuleFor(x => x.From).NotNull().WithMessage(Constants.FromIsRequired);
            RuleFor(x => x.Text).NotNull().When(x => x.Html == null).WithMessage(Constants.TextIsRequiredWhenHtmlIsMissing);
            RuleFor(x => x.Text).Null().When(x => x.Html != null).WithMessage(Constants.TextShouldNotBeSetWhenHtmlIsSet);
            RuleFor(x => x.Html).NotNull().When(x => x.Text == null).WithMessage(Constants.HtmlIsRequiredWhenTextIsMissing);
            RuleForEach(x => x.Attachments).SetValidator(new AttachmentValidator());
        }
    }
}
