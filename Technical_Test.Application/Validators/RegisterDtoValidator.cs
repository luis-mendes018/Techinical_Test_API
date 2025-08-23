using FluentValidation;
using Technical_Test.Application.DTOs;

namespace Technical_Test.Application.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .NotNull().WithMessage("Username cannot be null.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .NotNull().WithMessage("Password cannot be null.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");


        RuleFor(x => x.PasswordConfirm)
            .Equal(x => x.Password)
            .WithMessage("The password and confirmation password do not match.");
    }
}
