using FluentValidation;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.BuildingBlocks.Extensions;

namespace MotoLocadora.Application.Features.Ryders.Validators;

public class RiderDtoValidator : AbstractValidator<RiderDto>
{
    public RiderDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(x => x.CommpanyId).NotEmpty().WithMessage("CNPJ é obrigatório.").RuleForCnpj();
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Data de nascimento é obrigatória.");
        RuleFor(x => x.LicenseDrive).NotEmpty().WithMessage("CNH é obrigatória.").RuleForCnh();
        RuleFor(x => x.LicenseDriveType).RuleForEnum().WithMessage("Tipo de CNH deve ser A, B ou AB.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail é obrigatório.").RuleForEmail();
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefone é obrigatório.");
    }
}