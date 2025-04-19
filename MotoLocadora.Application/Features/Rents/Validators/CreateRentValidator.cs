using FluentValidation;
using MotoLocadora.Application.Features.Rents.Dtos;

namespace MotoLocadora.Application.Features.Rents.Validators;

public class CreateRentDtoValidator : AbstractValidator<CreateRentDto>
{
    public CreateRentDtoValidator()
    {
        RuleFor(x => x.TariffId)
            .GreaterThan(0).WithMessage("Tarifa obrigatória.");

        RuleFor(x => x.MotorcycleId)
            .GreaterThan(0).WithMessage("Motocicleta obrigatória.");

        RuleFor(x => x.Start)
            .NotEmpty().WithMessage("Data de início obrigatória.");

        RuleFor(x => x.EstimateEnd)
            .NotEmpty().WithMessage("Data de término estimada obrigatória.")
            .GreaterThan(x => x.Start).WithMessage("A data de término estimada deve ser após a data de início.");

        RuleFor(x => x.End)
            .GreaterThanOrEqualTo(x => x.Start).When(x => x.End > DateTime.MinValue)
            .WithMessage("A data de término deve ser igual ou posterior à data de início.");

    }
}
