using FluentValidation;
using MotoLocadora.BuildingBlocks.Extensions;

namespace MotoLocadora.Application.Features.Rents.Validators;

public class SimulateRentValidator : AbstractValidator<SimulateRent.Query>
{

    public SimulateRentValidator()
    {
        RuleFor(x => x.Start)
            .NotEmpty().WithMessage("Data de início é obrigatória.")
            .Must(x => x.IsValidDateFormat(out _)).WithMessage("Formato de data de início inválido. Use yyyy-MM-dd.");

        RuleFor(x => x.EstimateEnd)
            .NotEmpty().WithMessage("Data de término estimado é obrigatória.")
            .Must(x => x.IsValidDateFormat(out _)).WithMessage("Formato de data de término estimado inválido. Use yyyy-MM-dd.");

        RuleFor(x => x.MotorcycleId)
            .GreaterThan(0).WithMessage("O ID da motocicleta deve ser maior que zero.");
    }
}

