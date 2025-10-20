public record ManutencaoDto
{
    public string Id { get; init; } = string.Empty;
    public string MotoId { get; init; } = string.Empty;
    public string MotoristaId { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public DateTime DataManutencao { get; init; }
    public decimal Valor { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime DataCriacao { get; init; }
}

public record CriarManutencaoDto
{
    public string MotoId { get; init; } = string.Empty;
    public string MotoristaId { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public DateTime DataManutencao { get; init; }
    public decimal Valor { get; init; }
}

public record AtualizarManutencaoDto
{
    public string Descricao { get; init; } = string.Empty;
    public DateTime DataManutencao { get; init; }
    public decimal Valor { get; init; }
}