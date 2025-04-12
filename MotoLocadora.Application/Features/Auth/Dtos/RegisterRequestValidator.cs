using FluentValidation;
using MotoLocadora.Application.Features.Ryders.Validators;
using MotoLocadora.BuildingBlocks.Extensions;

namespace MotoLocadora.Application.Features.Auth.Dtos;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .RuleForEmail();

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .RuleForStrongPassword();

        RuleFor(x => x.Rider)
            .SetValidator(new RiderDtoValidator());
    }
}