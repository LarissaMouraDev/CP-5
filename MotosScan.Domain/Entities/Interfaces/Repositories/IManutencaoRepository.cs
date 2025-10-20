using MotosScan.Domain.Entities;
using MotosScan.Domain.Enums;

namespace MotosScan.Domain.Interfaces.Repositories;

public interface IManutencaoRepository
{
    Task<IEnumerable> ObterTodasAsync();
    Task ObterPorIdAsync(string id);
    Task<IEnumerable> ObterPorMotoAsync(string motoId);
    Task<IEnumerable> ObterPorMotoristaAsync(string motoristaId);
    Task<IEnumerable> ObterPorStatusAsync(StatusManutencao status);
    Task CriarAsync(Manutencao manutencao);
    Task AtualizarAsync(Manutencao manutencao);
    Task DeletarAsync(string id);
}