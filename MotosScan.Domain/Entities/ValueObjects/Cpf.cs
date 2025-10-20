namespace MotosScan.Domain.ValueObjects;

public class Cpf
{
    public string Valor { get; private set; }

    private Cpf(string valor)
    {
        Valor = valor;
    }

    public static Cpf Criar(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF n�o pode ser vazio");

        var cpfLimpo = cpf.Replace(".", "").Replace("-", "");

        if (!ValidarCpf(cpfLimpo))
            throw new ArgumentException("CPF inv�lido");

        return new Cpf(FormatarCpf(cpfLimpo));
    }

    private static bool ValidarCpf(string cpf)
    {
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        // Valida��o dos d�gitos verificadores
        var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        var resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        var digito = resto.ToString();
        tempCpf += digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }

    private static string FormatarCpf(string cpf)
    {
        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }

    public override string ToString() => Valor;
}