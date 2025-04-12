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
}

