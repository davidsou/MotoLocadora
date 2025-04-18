using FluentValidation;
using Microsoft.AspNetCore.Http;
using MotoLocadora.Application.Features.Images.Dto;
using MotoLocadora.BuildingBlocks.Extensions;
using System.Linq;

namespace MotoLocadora.Application.Features.Images.Validators;

public class UploadImageDtoValidator : AbstractValidator<UploadImageDto>
{
    private static readonly string[] AllowedExtensions = [".png", ".bmp"];
    private const long MaxFileSizeBytes = 2 * 1024 * 1024; // 2MB

    public UploadImageDtoValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("O arquivo é obrigatório.")
            .Must(f => f.Length <= MaxFileSizeBytes)
                .WithMessage($"O tamanho máximo permitido é de {MaxFileSizeBytes / (1024 * 1024)}MB.");

        RuleFor(x => x.File)
            .RuleForValidFileSignature(AllowedExtensions);
    }
}