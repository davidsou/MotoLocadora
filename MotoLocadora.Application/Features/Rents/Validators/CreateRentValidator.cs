using FluentValidation;

namespace MotoLocadora.Application.Features.Rents.Validators;

public class CreateRentValidator : AbstractValidator<CreateRent.Command>
{
    public CreateRentValidator()
    {
        this.ApplyRentRules(x => x.Rent);
    }
}