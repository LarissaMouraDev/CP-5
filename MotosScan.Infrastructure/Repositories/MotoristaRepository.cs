using MongoDB.Driver;
using MotosScan.Domain.Entities;
using MotosScan.Domain.Interfaces.Repositories;
using MotosScan.Infrastructure.Data;

namespace MotosScan.Infrastructure.Repositories;

public class MotoristaRepository : IMotoristaRepository
{
    private readonly IMongoCollection<Motorista> _collection;

    public MotoristaRepository(MongoDbContext context)
    {
        _collection = context.Motoristas;
    }

    public async Task<IEnumerable<Motorista>> ObterTodosAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<Motorista?> ObterPorIdAsync(string id)
    {
        return await _collection.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Motorista?> ObterPorCpfAsync(string cpf)
    {
        return await _collection.Find(m => m.Cpf == cpf).FirstOrDefaultAsync();
    }

    public async Task<Motorista?> ObterPorCnhAsync(string cnh)
    {
        return await _collection.Find(m => m.Cnh == cnh).FirstOrDefaultAsync();
    }

    public async Task<Motorista> CriarAsync(Motorista motorista)
    {
        await _collection.InsertOneAsync(motorista);
        return motorista;
    }

    public async Task<bool> AtualizarAsync(Motorista motorista)
    {
        var result = await _collection.ReplaceOneAsync(
            m => m.Id == motorista.Id,
            motorista
        );
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeletarAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }