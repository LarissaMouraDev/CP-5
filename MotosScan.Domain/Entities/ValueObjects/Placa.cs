using System.Text.RegularExpressions;

namespace MotosScan.Domain.ValueObjects;

public class Placa
{
    public string Valor { get; private set; }

    private Placa(string valor)
    {
        Valor = valor;
    }

    public static Placa Criar(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("Placa não pode ser vazia");

        var placaLimpa = placa.Replace("-", "").ToUpper();

        if (!Regex.IsMatch(placaLimpa, @"^[A-Z]{3}\d{4}$") &&
            !Regex.IsMatch(placaLimpa, @"^[A-Z]{3}\d[A-Z]\d{2}$"))
            throw new ArgumentException("Formato de placa inválido");

        return new Placa(FormatarPlaca(placaLimpa));
    }

    private static string FormatarPlaca(string placa)
    {
        return $"{placa.Substring(0, 3)}-{placa.Substring(3)}";
    }

    public override string ToString() => Valor;
}