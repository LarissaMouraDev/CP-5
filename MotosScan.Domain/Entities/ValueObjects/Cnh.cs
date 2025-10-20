namespace MotosScan.Domain.ValueObjects;

public class Cnh
{
    public string Numero { get; private set; }

    private Cnh(string numero)
    {
        Numero = numero;
    }

    public static Cnh Criar(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("CNH n�o pode ser vazia");

        var cnhLimpa = numero.Replace(" ", "");

        if (cnhLimpa.Length != 11 || !cnhLimpa.All(char.IsDigit))
            throw new ArgumentException("CNH deve conter 11 d�gitos");

        return new Cnh(cnhLimpa);
    }

    public override string ToString() => Numero;
}