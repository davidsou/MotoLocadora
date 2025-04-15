using FluentValidation;

namespace MotoLocadora.Application.Features.Motorcycles.Validators;

public class UpdateMotorcycleValidator : AbstractValidator<UpdateMotorcycle.Command>
{
    public UpdateMotorcycleValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Motorcycle.Ano).NotNull().WithMessage("Ano é obrigatório.")
            .InclusiveBetween(1000, 9999).WithMessage("Ano deve ter 4 dígitos.");
        RuleFor(x => x.Motorcycle.Modelo).NotEmpty();
        RuleFor(x => x.Motorcycle.Placa).NotEmpty().MaximumLength(10);
    }
}
