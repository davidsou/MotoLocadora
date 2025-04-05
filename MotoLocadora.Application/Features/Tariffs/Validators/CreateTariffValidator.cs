using FluentValidation;

namespace MotoLocadora.Application.Features.Tariffs.Validators;

public class CreateTariffValidator : AbstractValidator<CreateTariff.Command>
{
    public CreateTariffValidator()
    {
        this.ApplyTariffRules(x => x.Tariff);
    }
}