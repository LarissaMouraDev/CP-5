namespace MotosScan.Application.DTOs;

public record MotoDto
{
    public string Id { get; init; } = string.Empty;
    public string Modelo { get; init; } = string.Empty;
    public string Placa { get; init; } = string.Empty;
    public int Ano { get; init; }
    public int Quilometragem { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime DataCadastro { get; init; }
}

public record CriarMotoDto
{
    public string Modelo { get; init; } = string.Empty;
    public string Placa { get; init; } = string.Empty;
    public int Ano { get; init; }
}

public record AtualizarMotoDto
{
    public string Modelo { get; init; } = string.Empty;
    public int Quilometragem { get; init; }
    public string Status { get; init; } = string.Empty;
}