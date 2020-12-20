using FluentValidation;
using System.Collections.Generic;

namespace Blog.Service.Identity.Application.Features.UserAccount.Commands.Validators
{
    public class CreateUserAccountValidator : AbstractValidator<CreateUserAccountCommand>
    {
        public CreateUserAccountValidator()
        {
            RuleFor(model => model.UserName)
              .Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage("User Name field is missing")
              .Must(IsUserNameUnique).WithMessage("Email Id is already exist");

            RuleFor(model => model.Email)
              .Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage("Email field is missing")
              .EmailAddress().WithMessage("A valid email is required")
              .Must(IsEmailUnique).WithMessage("Email Id is already exist");

            RuleFor(model => model.PhoneNumber)
              .Cascade(CascadeMode.Stop)
              .Matches(@"^(\+[0-9]{9})$").When(s => !string.IsNullOrEmpty(s.PhoneNumber)).WithMessage("Invalid phone number");

            RuleFor(model => model.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(4).WithMessage("Minimum length for password is 4");
        }

        private bool IsEmailUnique(string email)
        {
            List<string> mockEmails = new List<string> {
                "a@gmail.com",
                "a.m.rabiul@outlook.com",
                "test@gmail.com"
                };

            if (mockEmails.Contains(email)) return false;

            return true;

        }

        private bool IsUserNameUnique(string userName)
        {
            List<string> mockUserNames = new List<string> {
                "abc",
                "silentrobi",
                "test"
                };

            if (mockUserNames.Contains(userName)) return false;

            return true;

        }
    }
}
