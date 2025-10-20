using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MotosScan.Domain.ValueObjects;
using MotosScan.Domain.Enums;

namespace MotosScan.Domain.Entities;

public class Moto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }

    [BsonElement("modelo")]
    public string Modelo { get; private set; }

    [BsonElement("placa")]
    public string Placa { get; private set; }

    [BsonElement("ano")]
    public int Ano { get; private set; }

    [BsonElement("quilometragem")]
    public int Quilometragem { get; private set; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public StatusMoto Status { get; private set; }

    [BsonElement("dataCadastro")]
    public DateTime DataCadastro { get; private set; }

    [BsonElement("dataAtualizacao")]
    public DateTime DataAtualizacao { get; private set; }

    public Moto(string modelo, string placa, int ano)
    {
        Id = ObjectId.GenerateNewId().ToString();
        ValidarEAtribuir(modelo, placa, ano);
        Quilometragem = 0;
        Status = StatusMoto.Disponivel;
        DataCadastro = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
    }

    private void ValidarEAtribuir(string modelo, string placa, int ano)
    {
        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("Modelo é obrigatório");

        var placaVO = Placa.Criar(placa);
        Placa = placaVO.Valor;

        if (ano < 1900 || ano > DateTime.Now.Year + 1)
            throw new ArgumentException("Ano inválido");

        Modelo = modelo;
        Ano = ano;
    }

    public void AtualizarQuilometragem(int quilometragem)
    {
        if (quilometragem < Quilometragem)
            throw new ArgumentException("Quilometragem não pode diminuir");

        Quilometragem = quilometragem;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AlterarStatus(StatusMoto novoStatus)
    {
        Status = novoStatus;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void IniciarManutencao()
    {
        if (Status == StatusMoto.EmManutencao)
            throw new InvalidOperationException("Moto já está em manutenção");

        Status = StatusMoto.EmManutencao;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void FinalizarManutencao()
    {
        if (Status != StatusMoto.EmManutencao)
            throw new InvalidOperationException("Moto não está em manutenção");

        Status = StatusMoto.Disponivel;
        DataAtualizacao = DateTime.UtcNow;
    }
}