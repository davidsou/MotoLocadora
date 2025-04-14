using FluentValidation;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.BuildingBlocks.Extensions;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Ryders.Validators;

public class RiderDtoValidator : AbstractValidator<RiderDto>
{
    private readonly IRiderRepository _riderRepository;
    public RiderDtoValidator(IRiderRepository riderRepository)
    {
        _riderRepository = riderRepository;

        RuleFor(x => x.Name).NotEmpty().WithMessage("Nome é obrigatório.");
        
        RuleFor(x => x.CommpanyId).NotEmpty().WithMessage("CNPJ é obrigatório.").RuleForCnpj()
            .MustAsync(CompanyIdUnique)
            .WithMessage("Já existe um entregador cadastrado com este CNPJ.");
        
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Data de nascimento é obrigatória.");
        
        RuleFor(x => x.LicenseDrive).NotEmpty().WithMessage("CNH é obrigatória.").RuleForCnh()
            .MustAsync(LicenseDriveUnique)
            .WithMessage("Já existe um entregador cadastrado com esta CNH.");

        RuleFor(x => x.LicenseDriveType).RuleForEnum().WithMessage("Tipo de CNH deve ser A, B ou AB.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail é obrigatório.").RuleForEmail();
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefone é obrigatório.");
    }

    private async Task<bool> CompanyIdUnique(string companyId, CancellationToken cancellationToken)
    {
        return !await _riderRepository.ExistsByCompanyIdAsync(companyId);
    }

    private async Task<bool> LicenseDriveUnique(string licenseDrive, CancellationToken cancellationToken)
    {
        return !await _riderRepository.ExistsByLicenseDriveAsync(licenseDrive);
    }
}