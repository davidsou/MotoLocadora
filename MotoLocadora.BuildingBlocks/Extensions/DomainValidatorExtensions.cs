using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MotoLocadora.BuildingBlocks.Extensions;
public static class DomainValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> RuleForCnpj<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(DomainValidator.IsValidCnpj).WithMessage("CNPJ inválido.");
    }

    public static IRuleBuilderOptions<T, string> RuleForCpf<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(DomainValidator.IsValidCpf).WithMessage("CPF inválido.");
    }

    public static IRuleBuilderOptions<T, string> RuleForEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(DomainValidator.IsValidEmail).WithMessage("E-mail inválido.");
    }

    public static IRuleBuilderOptions<T, string> RuleForCnh<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(DomainValidator.IsValidCnh).WithMessage("CNH inválida.");
    }

    public static IRuleBuilderOptions<T, string> RuleForStrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(DomainValidator.IsStrongPassword).WithMessage("A senha deve ter no mínimo 6 caracteres, incluindo letras e números.");
    }
    public static IRuleBuilderOptions<T, TEnum> RuleForEnum<T, TEnum>(this IRuleBuilder<T, TEnum> ruleBuilder) where TEnum : struct, Enum
    {
        return ruleBuilder
            .Must(e => Enum.IsDefined(typeof(TEnum), e))
            .WithMessage($"O valor informado para {typeof(TEnum).Name} não é válido.");
    }

    public static IRuleBuilderOptions<T,IFormFile> RuleForValidFileSignature<T>(this IRuleBuilder<T,IFormFile> ruleBuilder)
    {
        return ruleBuilder
                .Must(DomainValidator.HasValidFileSignature)
                .WithMessage("O conteúdo do arquivo não corresponde à assinatura esperada.");
    }
    public static IRuleBuilderOptions<T, IFormFile> RuleForValidFileSignature<T>(
        this IRuleBuilder<T, IFormFile> ruleBuilder,
        params string[] allowedExtensions)
    {
        return ruleBuilder
            .Must(file => DomainValidator.HasValidFileSignature(file, allowedExtensions))
            .WithMessage($"O conteúdo do arquivo não corresponde à assinatura esperada para os tipos permitidos: {string.Join(", ", allowedExtensions)}.");
    }
}