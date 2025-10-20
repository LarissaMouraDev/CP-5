namespace MotosScan.Application.DTOs;

public record MotoristaDto
{
    public string Id { get; init; } = string.Empty;
    public string Nome { get; init; } = string.Empty;
    public string Cpf { get; init; } = string.Empty;
    public string Cnh { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
    public string Telefone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? FotoCnhUrl { get; init; }
    public DateTime DataCadastro { get; init; }
}

public record CriarMotoristaDto
{
    public string Nome { get; init; } = string.Empty;
    public string Cpf { get; init; } = string.Empty;
    public string Cnh { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
    public string Telefone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

public record AtualizarMotoristaDto
{
    public string Nome { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}