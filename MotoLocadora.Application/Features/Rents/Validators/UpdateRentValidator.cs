using FluentValidation;

namespace MotoLocadora.Application.Features.Rents.Validators;
public class UpdateRentValidator : AbstractValidator<UpdateRent.Command>
{
    public UpdateRentValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        this.ApplyRentRules(x => x.Rent);
    }
}
