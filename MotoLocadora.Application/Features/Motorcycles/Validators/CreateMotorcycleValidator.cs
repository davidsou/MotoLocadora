using FluentValidation;

namespace MotoLocadora.Application.Features.Motorcycles.Validators;

public class CreateMotorcycleValidator : AbstractValidator<CreateMotorcycle.Command>
{
    public CreateMotorcycleValidator()
    {
        RuleFor(x => x.Motorcycle.Ano)
            .NotNull().WithMessage("Ano é obrigatório.")
            .InclusiveBetween(1000, 9999).WithMessage("Ano deve ter 4 dígitos.");

        RuleFor(x => x.Motorcycle.Modelo)
            .NotEmpty().WithMessage("Modelo é obrigatório.");

        RuleFor(x => x.Motorcycle.Placa)
            .NotEmpty().WithMessage("Placa é obrigatória.")
            .MaximumLength(10);
    }
}
