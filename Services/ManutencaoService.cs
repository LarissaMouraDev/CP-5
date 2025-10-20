using MotosScan.Application.DTOs;
using MotosScan.Domain.Entities;
using MotosScan.Domain.Enums;
using MotosScan.Domain.Interfaces.Repositories;

namespace MotosScan.Application.Services;

public class ManutencaoService
{
    private readonly IManutencaoRepository _repository;
    private readonly IMotoRepository _motoRepository;

    public ManutencaoService(
        IManutencaoRepository repository,
        IMotoRepository motoRepository)
    {
        _repository = repository;
        _motoRepository = motoRepository;
    }

    public async Task<IEnumerable<ManutencaoDto>> ObterTodasAsync()
    {
        var manutencoes = await _repository.ObterTodasAsync();
        return manutencoes.Select(MapearParaDto);
    }

    public async Task<ManutencaoDto?> ObterPorIdAsync(string id)
    {
        var manutencao = await _repository.ObterPorIdAsync(id);
        return manutencao == null ? null : MapearParaDto(manutencao);
    }

    public async Task<IEnumerable<ManutencaoDto>> ObterPorMotoAsync(string motoId)
    {
        var manutencoes = await _repository.ObterPorMotoAsync(motoId);
        return manutencoes.Select(MapearParaDto);
    }

    public async Task<IEnumerable<ManutencaoDto>> ObterPorMotoristaAsync(string motoristaId)
    {
        var manutencoes = await _repository.ObterPorMotoristaAsync(motoristaId);
        return manutencoes.Select(MapearParaDto);
    }

    public async Task<IEnumerable<ManutencaoDto>> ObterPendentesAsync()
    {
        var manutencoes = await _repository.ObterPorStatusAsync(StatusManutencao.Pendente);
        return manutencoes.Select(MapearParaDto);
    }

    public async Task<ManutencaoDto> CriarAsync(CriarManutencaoDto dto)
    {
        var moto = await _motoRepository.ObterPorIdAsync(dto.MotoId);
        if (moto == null)
            throw new ArgumentException("Moto não encontrada");

        var manutencao = new Manutencao(
            dto.MotoId,
            dto.MotoristaId,
            dto.Descricao,
            dto.DataManutencao,
            dto.Valor
        );

        // Marcar moto como em manutenção
        moto.IniciarManutencao();
        await _motoRepository.AtualizarAsync(moto);

        await _repository.CriarAsync(manutencao);
        return MapearParaDto(manutencao);
    }

    public async Task<bool> IniciarAsync(string id)
    {
        var manutencao = await _repository.ObterPorIdAsync(id);

        if (manutencao == null)
            return false;

        manutencao.IniciarManutencao();
        return await _repository.AtualizarAsync(manutencao);
    }

    public async Task<bool> ConcluirAsync(string id)
    {
        var manutencao = await _repository.ObterPorIdAsync(id);

        if (manutencao == null)
            return false;

        manutencao.Concluir();
        await _repository.AtualizarAsync(manutencao);

        // Finalizar manutenção na moto
        var moto = await _motoRepository.ObterPorIdAsync(manutencao.MotoId);
        if (moto != null)
        {
            moto.FinalizarManutencao();
            await _motoRepository.AtualizarAsync(moto);
        }

        return true;
    }

    public async Task<bool> CancelarAsync(string id)
    {
        var manutencao = await _repository.ObterPorIdAsync(id);

        if (manutencao == null)
            return false;

        manutencao.Cancelar();
        await _repository.AtualizarAsync(manutencao);

        // Finalizar manutenção na moto
        var moto = await _motoRepository.ObterPorIdAsync(manutencao.MotoId);
        if (moto != null)
        {
            moto.FinalizarManutencao();
            await _motoRepository.AtualizarAsync(moto);
        }

        return true;
    }

    public async Task<bool> DeletarAsync(string id)
    {
        return await _repository.DeletarAsync(id);
    }

    private static ManutencaoDto MapearParaDto(Manutencao manutencao)
    {
        return new ManutencaoDto
        {
            Id = manutencao.Id,
            MotoId = manutencao.MotoId,
            MotoristaId = manutencao.MotoristaId,
            Descricao = manutencao.Descricao,
            DataManutencao = manutencao.DataManutencao,
            Valor = manutencao.Valor,
            Status = manutencao.Status.ToString(),
            DataCriacao = manutencao.DataCriacao
        };
    }
}