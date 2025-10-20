using MotosScan.Domain.Entities;

namespace MotosScan.Domain.Interfaces.Repositories;

public interface IMotoristaRepository
{
    Task<IEnumerable> ObterTodosAsync();
    Task ObterPorIdAsync(string id);
    Task ObterPorCpfAsync(string cpf);
    Task ObterPorCnhAsync(string cnh);
    Task CriarAsync(Motorista motorista);
    Task AtualizarAsync(Motorista motorista);
    Task DeletarAsync(string id);
    Task ExistePorCpfAsync(string cpf);
    