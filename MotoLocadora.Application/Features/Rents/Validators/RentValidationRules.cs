using FluentValidation;
using MotoLocadora.Application.Features.Rents.Dtos;

namespace MotoLocadora.Application.Features.Rents.Validators;

public static class RentValidationRules
{
    public static void ApplyRentRules<T>(this AbstractValidator<T> validator, Func<T, RentDto> selector)
    {
        validator.RuleFor(x => selector(x).TariffId).GreaterThan(0);
        validator.RuleFor(x => selector(x).RiderId).GreaterThan(0);
        validator.RuleFor(x => selector(x).MotorcycleId).GreaterThan(0);
        validator.RuleFor(x => selector(x).Start).NotEmpty();
        validator.RuleFor(x => selector(x).EstimateEnd).NotEmpty();
        validator.RuleFor(x => selector(x).End).NotEmpty();
        validator.RuleFor(x => selector(x).FinalPrice).GreaterThanOrEqualTo(0);
        validator.RuleFor(x => selector(x).AppliedFine).GreaterThanOrEqualTo(0);
        validator.RuleFor(x => selector(x).FineReason).MaximumLength(200);
    }
}
