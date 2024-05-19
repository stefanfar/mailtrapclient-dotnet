using FluentValidation;

namespace Mailtrap.Validators
{
    public class AttachmentValidator : AbstractValidator<Attachment>
    {
        public AttachmentValidator()
        {
            RuleFor(x => x.Content).NotNull().WithMessage($"The property {nameof(Attachment.Content)} is required");
            RuleFor(x => x.Filename).NotNull().WithMessage($"The property {nameof(Attachment.Filename)} is required");
        }
    }
}
