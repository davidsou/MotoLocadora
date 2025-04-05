using FluentValidation;
using MotoLocadora.Application.Features.Tariffs.Dtos;

namespace MotoLocadora.Application.Features.Tariffs.Validators;

public static class TariffValidationRules
{
    public static void ApplyTariffRules<T>(this AbstractValidator<T> validator, Func<T, TariffDto> selector)
    {
        validator.RuleFor(x => selector(x).Price).GreaterThan(0);
        validator.RuleFor(x => selector(x).Days).GreaterThan(0);
    }
}
