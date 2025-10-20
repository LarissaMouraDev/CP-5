using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MotosScan.Domain.Enums;

namespace MotosScan.Domain.Entities;

public class Manutencao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }

    [BsonElement("motoId")]
    public string MotoId { get; private set; }

    [BsonElement("motoristaId")]
    public string MotoristaId { get; private set; }

    [BsonElement("descricao")]
    public string Descricao { get; private set; }

    [BsonElement("dataManutencao")]
    public DateTime DataManutencao { get; private set; }

    [BsonElement("valor")]
    public decimal Valor { get; private set; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public StatusManutencao Status { get; private set; }

    [BsonElement("dataCriacao")]
    public DateTime DataCriacao { get; private set; }

    [BsonElement("dataAtualizacao")]
    public DateTime DataAtualizacao { get; private set; }

    public Manutencao(string motoId, string motoristaId, string descricao,
                      DateTime dataManutencao, decimal valor)
    {
        Id = ObjectId.GenerateNewId().ToString();
        ValidarEAtribuir(motoId, motoristaId, descricao, dataManutencao, valor);
        Status = StatusManutencao.Pendente;
        DataCriacao = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
    }

    private void ValidarEAtribuir(string motoId, string motoristaId, string descricao,
                                   DateTime dataManutencao, decimal valor)
    {
        if (string.IsNullOrWhiteSpace(motoId))
            throw new ArgumentException("ID da moto é obrigatório");

        if (string.IsNullOrWhiteSpace(motoristaId))
            throw new ArgumentException("ID do motorista é obrigatório");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição é obrigatória");

        if (valor < 0)
            throw new ArgumentException("Valor deve ser positivo");

        MotoId = motoId;
        MotoristaId = motoristaId;
        Descricao = descricao;
        DataManutencao = dataManutencao;
        Valor = valor;
    }

    public void IniciarManutencao()
    {
        if (Status != StatusManutencao.Pendente)
            throw new InvalidOperationException("Apenas manutenções pendentes podem ser iniciadas");

        Status = StatusManutencao.EmAndamento;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Concluir()
    {
        if (Status == StatusManutencao.Concluida)
            throw new InvalidOperationException("Manutenção já foi concluída");

        if (Status == StatusManutencao.Cancelada)
            throw new InvalidOperationException("Manutenção cancelada não pode ser concluída");

        Status = StatusManutencao.Concluida;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Cancelar()
    {
        if (Status == StatusManutencao.Concluida)
            throw new InvalidOperationException("Manutenção concluída não pode ser cancelada");

        if (Status == StatusManutencao.Cancelada)
            throw new InvalidOperationException("Manutenção já foi cancelada");

        Status = StatusManutencao.Cancelada;
        DataAtualizacao = DateTime.UtcNow;
    }
}