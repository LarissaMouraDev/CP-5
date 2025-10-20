using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MotosScan.Domain.Entities;

namespace MotosScan.Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    public MongoDbContext(IOptions settings)
    {
        _settings = settings.Value;
        var client = new MongoClient(_settings.ConnectionString);
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection Motos =>
        _database.GetCollection(_settings.Collections.Motos);

    public IMongoCollection Motoristas =>
        _database.GetCollection(_settings.Collections.Motoristas);

    public IMongoCollection Manutencoes =>
        _database.GetCollection(_settings.Collections.Manutencoes);

    public IMongoDatabase Database => _database;
}