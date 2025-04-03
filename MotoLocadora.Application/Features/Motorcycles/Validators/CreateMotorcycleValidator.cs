using FluentValidation;
using MotoLocadora.Application.Features.Motorcycle;
using MotoLocadora.Application.Features.Motorcycles;

namespace MotoLocadora.Application.Features.Motorcycle.Validators;

public class CreateMotorcycleValidator : AbstractValidator<CreateMotorcycle.Command>
{
    public CreateMotorcycleValidator()
    {
        RuleFor(x => x.Motorcycle.Ano)
            .NotEmpty().WithMessage("Ano é obrigatório.")
            .Length(4).WithMessage("Ano deve ter 4 dígitos.");

        RuleFor(x => x.Motorcycle.Modelo)
            .NotEmpty().WithMessage("Modelo é obrigatório.");

        RuleFor(x => x.Motorcycle.Placa)
            .NotEmpty().WithMessage("Placa é obrigatória.")
            .MaximumLength(10);
    }
}
