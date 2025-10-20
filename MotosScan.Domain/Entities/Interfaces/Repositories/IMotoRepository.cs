using MotosScan.Domain.Entities;

namespace MotosScan.Domain.Interfaces.Repositories;

public interface IMotoRepository
{
    Task<IEnumerable> ObterTodasAsync();
    Task ObterPorIdAsync(string id);
    Task ObterPorPlacaAsync(string placa);
    Task CriarAsync(Moto moto);
    Task AtualizarAsync(Moto moto);
    Task DeletarAsync(string id);
    Task ExistePorPlacaAsync(string placa);
}