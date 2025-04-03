using FluentValidation;

namespace MotoLocadora.Application.Features.Motorcycles.Validators;

public class UpdateMotorcycleValidator : AbstractValidator<UpdateMotorcycle.Command>
{
    public UpdateMotorcycleValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Motorcycle.Ano).NotEmpty().Length(4);
        RuleFor(x => x.Motorcycle.Modelo).NotEmpty();
        RuleFor(x => x.Motorcycle.Placa).NotEmpty().MaximumLength(10);
    }
}
