using FluentValidation;

namespace Bookshop_api.Validations
{
    public class CustomerValidations
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }

        public CustomerValidations(string Email, string Password, string Name, string MobileNumber, string Address)
        {
            this.Email = Email;
            this.Password = Password;
            this.Name = Name;
            this.MobileNumber = MobileNumber;
            this.Address = Address;
        }
    }

    public class CustomerValidationValidator : AbstractValidator<CustomerValidations>
    {
        public CustomerValidationValidator()
        {
            // Email validations
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            // Password validations
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one number")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character");

            // Name validations
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must be at most 50 characters");

            // Mobile number validations
            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile Number is required")
                .Matches(@"^\+?[1-10]\d{1,14}$").WithMessage("Invalid mobile number format");

            // Address validations
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(100).WithMessage("Address must be at most 100 characters");
        }
    }
}
