namespace MotosScan.Infrastructure.Data;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public CollectionSettings Collections { get; set; } = new();
}

public class CollectionSettings
{
    public string Motos { get; set; } = "Motos";
    public string Motoristas { get; set; } = "Motoristas";
    public string Manutencoes { get; set; } = "Manutencoes";
}