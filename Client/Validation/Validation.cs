using FluentValidation;

namespace Client.Validation
{
    public class VisitorDtoValidator : AbstractValidator<DTO.User.VisitorDto>
    {
        public VisitorDtoValidator()
        {
            _ = this.RuleFor(p => p.Email).NotEmpty().WithMessage(x => "Email Required");

        }
    }


}
