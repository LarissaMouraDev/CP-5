using MongoDB.Driver;
using MotosScan.Domain.Entities;
using MotosScan.Domain.Enums;
using MotosScan.Domain.Interfaces.Repositories;
using MotosScan.Infrastructure.Data;

namespace MotosScan.Infrastructure.Repositories;

public class ManutencaoRepository : IManutencaoRepository
{
    private readonly IMongoCollection _collection;

    public ManutencaoRepository(MongoDbContext context)
    {
        _collection = context.Manutencoes;
    }

    public async Task<IEnumerable> ObterTodasAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task ObterPorIdAsync(string id)
    {
        return await _collection.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable> ObterPorMotoAsync(string motoId)
    {
        return await _collection.Find(m => m.MotoId == motoId).ToListAsync();
    }

    public async Task<IEnumerable> ObterPorMotoristaAsync(string motoristaId)
    {
        return await _collection.Find(m => m.MotoristaId == motoristaId).ToListAsync();
    }

    public async Task<IEnumerable> ObterPorStatusAsync(StatusManutencao status)
    {
        return await _collection.Find(m => m.Status == status).ToListAsync();
    }

    public async Task CriarAsync(Manutencao manutencao)
    {
        await _collection.InsertOneAsync(manutencao);
        return manutencao;
    }

    public async Task AtualizarAsync(Manutencao manutencao)
    {
        var result = await _collection.ReplaceOneAsync(
            m => m.Id == manutencao.Id,
            manutencao
        );
        return result.ModifiedCount > 0;
    }

    public async Task DeletarAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }
}