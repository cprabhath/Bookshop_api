using FluentValidation;

namespace Bookshop_api.Validations
{
    public class PasswordValidations
    {
        public string Password { get; set; }

        public PasswordValidations(string Password)
        {
            this.Password = Password;
        }
    }

    public class PasswordValidationValidator : AbstractValidator<PasswordValidations>
    {
        public PasswordValidationValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one number")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character");
        }
    }
}
