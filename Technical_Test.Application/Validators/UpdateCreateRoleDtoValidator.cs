using FluentValidation;

using Technical_Test.Application.DTOs.RolesDTOs;

namespace Technical_Test.Application.Validators;

public class UpdateCreateRoleDtoValidator : AbstractValidator<UpdateCreateRoleDto>
{
    public UpdateCreateRoleDtoValidator()
    {
        RuleFor(x => x.NewName)
             .NotEmpty().WithMessage("This field is required.")
             .NotNull().WithMessage("This field cannot be null.")
             .MaximumLength(50).WithMessage("The name must have a maximum of 50 characters ");
    }
}
