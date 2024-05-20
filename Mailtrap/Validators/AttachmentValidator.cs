using FluentValidation;
using Mailtrap.Entities;

namespace Mailtrap.Validators
{
    internal class AttachmentValidator : AbstractValidator<Attachment>
    {
        public AttachmentValidator()
        {
            RuleFor(x => x.Content).NotNull().WithMessage(Constants.AttachmentContentIsRequired);
            RuleFor(x => x.Filename).NotNull().WithMessage(Constants.AttachmentFilenameIsRequired);
        }
    }
}
