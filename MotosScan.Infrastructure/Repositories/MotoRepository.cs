using MongoDB.Driver;
using MotosScan.Domain.Entities;
using MotosScan.Domain.Interfaces.Repositories;
using MotosScan.Infrastructure.Data;

namespace MotosScan.Infrastructure.Repositories;

public class MotoRepository : IMotoRepository
{
    private readonly IMongoCollection _collection;

    public MotoRepository(MongoDbContext context)
    {
        _collection = context.Motos;
    }

    public async Task<IEnumerable> ObterTodasAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task ObterPorIdAsync(string id)
    {
        return await _collection.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task ObterPorPlacaAsync(string placa)
    {
        return await _collection.Find(m => m.Placa == placa).FirstOrDefaultAsync();
    }

    public async Task CriarAsync(Moto moto)
    {
        await _collection.InsertOneAsync(moto);
        return moto;
    }

    public async Task AtualizarAsync(Moto moto)
    {
        var result = await _collection.ReplaceOneAsync(
            m => m.Id == moto.Id,
            moto
        );
        return result.ModifiedCount > 0;
    }

    public async Task DeletarAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task ExistePorPlacaAsync(string placa)
    {
        return await _collection.Find(m => m.Placa == placa).AnyAsync();
    }
}