using MotosScan.Application.DTOs;
using MotosScan.Domain.Entities;
using MotosScan.Domain.Enums;
using MotosScan.Domain.Interfaces.Repositories;

namespace MotosScan.Application.Services;

public class MotoService
{
    private readonly IMotoRepository _repository;

    public MotoService(IMotoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable> ObterTodasAsync()
    {
        var motos = await _repository.ObterTodasAsync();
        return motos.Select(MapearParaDto);
    }

    public async Task ObterPorIdAsync(string id)
    {
        var moto = await _repository.ObterPorIdAsync(id);
        return moto == null ? null : MapearParaDto(moto);
    }

    public async Task ObterPorPlacaAsync(string placa)
    {
        var moto = await _repository.ObterPorPlacaAsync(placa);
        return moto == null ? null : MapearParaDto(moto);
    }

    public async Task CriarAsync(CriarMotoDto dto)
    {
        if (await _repository.ExistePorPlacaAsync(dto.Placa))
            throw new ArgumentException("Já existe uma moto com esta placa");

        var moto = new Moto(dto.Modelo, dto.Placa, dto.Ano);
        await _repository.CriarAsync(moto);

        return MapearParaDto(moto);
    }

    public async Task AtualizarAsync(string id, AtualizarMotoDto dto)
    {
        var moto = await _repository.ObterPorIdAsync(id);

        if (moto == null)
            return false;

        moto.AtualizarQuilometragem(dto.Quilometragem);

        if (Enum.TryParse(dto.Status, out var status))
            moto.AlterarStatus(status);

        return await _repository.AtualizarAsync(moto);
    }

    public async Task DeletarAsync(string id)
    {
        return await _repository.DeletarAsync(id);
    }

    private static MotoDto MapearParaDto(Moto moto)
    {
        return new MotoDto
        {
            Id = moto.Id,
            Modelo = moto.Modelo,
            Placa = moto.Placa,
            Ano = moto.Ano,
            Quilometragem = moto.Quilometragem,
            Status = moto.Status.ToString(),
            DataCadastro = moto.DataCadastro
        };
    }
}