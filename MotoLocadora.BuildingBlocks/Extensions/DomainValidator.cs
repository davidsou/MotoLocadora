using Microsoft.AspNetCore.Http;

namespace MotoLocadora.BuildingBlocks.Extensions;

public static class DomainValidator
{
    public static bool IsValidCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpj.Length != 14 || new string(cnpj[0], cnpj.Length) == cnpj)
            return false;

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj[..12];
        int soma = tempCnpj.Select((t, i) => int.Parse(t.ToString()) * multiplicador1[i]).Sum();

        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        tempCnpj += resto;

        soma = tempCnpj.Select((t, i) => int.Parse(t.ToString()) * multiplicador2[i]).Sum();
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        string digito = tempCnpj[12] + resto.ToString();

        return cnpj.EndsWith(digito);
    }

    public static bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11 || new string(cpf[0], cpf.Length) == cpf)
            return false;

        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf[..9];
        int soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicador1[i]).Sum();

        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        tempCpf += resto;

        soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicador2[i]).Sum();
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        string digito = tempCpf[9] + resto.ToString();

        return cpf.EndsWith(digito);
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidCnh(string cnh)
    {
        if (string.IsNullOrWhiteSpace(cnh))
            return false;

        cnh = new string(cnh.Where(char.IsDigit).ToArray());

        if (cnh.Length != 11 || new string(cnh[0], cnh.Length) == cnh)
            return false;

        int sum1 = 0;
        for (int i = 0, j = 9; i < 9; i++, j--)
            sum1 += (cnh[i] - '0') * j;

        int firstDigit = sum1 % 11;
        firstDigit = firstDigit >= 10 ? 0 : firstDigit;

        int sum2 = 0;
        for (int i = 0, j = 1; i < 9; i++, j++)
            sum2 += (cnh[i] - '0') * j;

        int secondDigit = sum2 % 11;
        secondDigit = secondDigit >= 10 ? 0 : secondDigit;

        return cnh[9] - '0' == firstDigit && cnh[10] - '0' == secondDigit;
    }

    public static bool IsStrongPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return false;

        bool hasLetter = password.Any(char.IsLetter);
        bool hasDigit = password.Any(char.IsDigit);

        return hasLetter && hasDigit;
    }
    public static bool HasValidFileSignature(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var signatures = new Dictionary<string, List<byte[]>>
    {
        { ".png",  [new byte[] { 0x89, 0x50, 0x4E, 0x47 }] },            // PNG
        { ".bmp",  [new byte[] { 0x42, 0x4D }] },                        // BMP
        { ".jpg",  [new byte[] { 0xFF, 0xD8, 0xFF }] },                  // JPG
        { ".jpeg", [new byte[] { 0xFF, 0xD8, 0xFF }] },                  // JPEG
        { ".pdf",  [new byte[] { 0x25, 0x50, 0x44, 0x46 }] },            // %PDF
        { ".zip",  [new byte[] { 0x50, 0x4B, 0x03, 0x04 }] }             // PK..
    };

        if (!signatures.TryGetValue(extension, out var expectedSignatures))
            return false;

        var maxLength = expectedSignatures.Max(s => s.Length);
        var buffer = new byte[maxLength];

        try
        {
            using var stream = file.OpenReadStream();
            if (stream.Read(buffer, 0, maxLength) < maxLength)
                return false;

            return expectedSignatures.Any(signature =>
                buffer.Take(signature.Length).SequenceEqual(signature));
        }
        catch
        {
            return false;
        }
    }
    public static bool HasValidFileSignature(IFormFile file, params string[] allowedExtensions)
    {
        if (file == null || file.Length == 0)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        // Se a extensão não estiver permitida, já retorna false
        if (allowedExtensions?.Length > 0 && !allowedExtensions.Contains(extension))
            return false;

        var signatures = new Dictionary<string, List<byte[]>>
    {
        { ".png",  [new byte[] { 0x89, 0x50, 0x4E, 0x47 }] },
        { ".bmp",  [new byte[] { 0x42, 0x4D }] },
        { ".jpg",  [new byte[] { 0xFF, 0xD8, 0xFF }] },
        { ".jpeg", [new byte[] { 0xFF, 0xD8, 0xFF }] },
        { ".pdf",  [new byte[] { 0x25, 0x50, 0x44, 0x46 }] },
        { ".zip",  [new byte[] { 0x50, 0x4B, 0x03, 0x04 }] }
    };

        if (!signatures.TryGetValue(extension, out var expectedSignatures))
            return false;

        var maxLength = expectedSignatures.Max(s => s.Length);
        var buffer = new byte[maxLength];

        try
        {
            using var stream = file.OpenReadStream();
            if (stream.Read(buffer, 0, maxLength) < maxLength)
                return false;

            return expectedSignatures.Any(signature =>
                buffer.Take(signature.Length).SequenceEqual(signature));
        }
        catch
        {
            return false;
        }
    }
}

