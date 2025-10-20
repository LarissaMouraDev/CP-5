using MotosScan.Application.DTOs;
using MotosScan.Domain.Entities;
using MotosScan.Domain.Interfaces.Repositories;

namespace MotosScan.Application.Services;

public class MotoristaService
{
    private readonly IMotoristaRepository _repository;

    public MotoristaService(IMotoristaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MotoristaDto>> ObterTodosAsync()
    {
        var motoristas = await _repository.ObterTodosAsync();
        return motoristas.Select(MapearParaDto);
    }

    public async Task<MotoristaDto?> ObterPorIdAsync(string id)
    {
        var motorista = await _repository.ObterPorIdAsync(id);
        return motorista == null ? null : MapearParaDto(motorista);
    }

    public async Task<MotoristaDto?> ObterPorCpfAsync(string cpf)
    {
        var motorista = await _repository.ObterPorCpfAsync(cpf);
        return motorista == null ? null : MapearParaDto(motorista);
    }

    public async Task<MotoristaDto?> ObterPorCnhAsync(string cnh)
    {
        var motorista = await _repository.ObterPorCnhAsync(cnh);
        return motorista == null ? null : MapearParaDto(motorista);
    }

    public async Task<MotoristaDto> CriarAsync(CriarMotoristaDto dto)
    {
        if (await _repository.ExistePorCpfAsync(dto.Cpf))
            throw new ArgumentException("Já existe um motorista com este CPF");

        if (await _repository.ExistePorCnhAsync(dto.Cnh))
            throw new ArgumentException("Já existe um motorista com esta CNH");

        var motorista = new Motorista(
            dto.Nome,
            dto.Cpf,
            dto.Cnh,
            dto.DataNascimento,
            dto.Telefone,
            dto.Email
        );

        await _repository.CriarAsync(motorista);
        return MapearParaDto(motorista);
    }

    public async Task<bool> AtualizarAsync(string id, AtualizarMotoristaDto dto)
    {
        var motorista = await _repository.ObterPorIdAsync(id);

        if (motorista == null)
            return false;

        // Como as entidades são imutáveis em relação a alguns campos,
        // aqui você precisaria criar métodos de atualização na entidade
        // ou reconstruir a entidade com os novos valores

        return await _repository.AtualizarAsync(motorista);
    }

    public async Task<bool> DeletarAsync(string id)
    {
        return await _repository.DeletarAsync(id);
    }

    public async Task<bool> AdicionarFotoCnhAsync(string id, string fotoCnhUrl)
    {
        var motorista = await _repository.ObterPorIdAsync(id);

        if (motorista == null)
            return false;

        motorista.AdicionarFotoCnh(fotoCnhUrl);
        return await _repository.AtualizarAsync(motorista);
    }

    public async Task<bool> RemoverFotoCnhAsync(string id)
    {
        var motorista = await _repository.ObterPorIdAsync(id);

        if (motorista == null)
            return false;

        motorista.RemoverFotoCnh();
        return await _repository.AtualizarAsync(motorista);
    }

    private static MotoristaDto MapearParaDto(Motorista motorista)
    {
        return new MotoristaDto
        {
            Id = motorista.Id,
            Nome = motorista.Nome,
            Cpf = motorista.Cpf,
            Cnh = motorista.Cnh,
            DataNascimento = motorista.DataNascimento,
            Telefone = motorista.Telefone,
            Email = motorista.Email,
            FotoCnhUrl = motorista.FotoCnhUrl,
            DataCadastro = motorista.DataCadastro
        };
    }
}
