using FluentValidation;
using MotoLocadora.Application.Features.Ryders.Dtos;

namespace MotoLocadora.Application.Features.Ryders.Validators;

public class CreateRiderCommandValidator : AbstractValidator<CreateRider.Command>
{
    public CreateRiderCommandValidator(IValidator<RiderDto> riderDtoValidator)
    {
        RuleFor(x => x.Rider)
            .SetValidator(riderDtoValidator);
    }
}
