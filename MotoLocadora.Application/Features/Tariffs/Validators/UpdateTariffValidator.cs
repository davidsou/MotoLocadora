using FluentValidation;

namespace MotoLocadora.Application.Features.Tariffs.Validators;

public class UpdateTariffValidator : AbstractValidator<UpdateTariff.Command>
{
    public UpdateTariffValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        this.ApplyTariffRules(x => x.Tariff);
    }
}

