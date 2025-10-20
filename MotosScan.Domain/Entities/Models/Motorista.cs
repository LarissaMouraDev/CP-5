using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MotosScan.Domain.ValueObjects;

namespace MotosScan.Domain.Entities;

public class Motorista
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }

    [BsonElement("nome")]
    public string Nome { get; private set; }

    [BsonElement("cpf")]
    public string Cpf { get; private set; }

    [BsonElement("cnh")]
    public string Cnh { get; private set; }

    [BsonElement("dataNascimento")]
    public DateTime DataNascimento { get; private set; }

    [BsonElement("telefone")]
    public string Telefone { get; private set; }

    [BsonElement("email")]
    public string Email { get; private set; }

    [BsonElement("fotoCnhUrl")]
    public string? FotoCnhUrl { get; private set; }

    [BsonElement("dataCadastro")]
    public DateTime DataCadastro { get; private set; }

    [BsonElement("dataAtualizacao")]
    public DateTime DataAtualizacao { get; private set; }

    public Motorista(string nome, string cpf, string cnh, DateTime dataNascimento,
                     string telefone, string email)
    {
        Id = ObjectId.GenerateNewId().ToString();
        ValidarEAtribuir(nome, cpf, cnh, dataNascimento, telefone, email);
        DataCadastro = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
    }

    private void ValidarEAtribuir(string nome, string cpf, string cnh,
                                   DateTime dataNascimento, string telefone, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório");

        var cpfVO = Cpf.Criar(cpf);
        var cnhVO = Cnh.Criar(cnh);

        if (CalcularIdade(dataNascimento) < 18)
            throw new ArgumentException("Motorista deve ter pelo menos 18 anos");

        Nome = nome;
        Cpf = cpfVO.Valor;
        Cnh = cnhVO.Numero;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Email = email;
    }

    private int CalcularIdade(DateTime dataNascimento)
    {
        var hoje = DateTime.Today;
        var idade = hoje.Year - dataNascimento.Year;
        if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }

    public void AdicionarFotoCnh(string fotoCnhUrl)
    {
        if (string.IsNullOrWhiteSpace(fotoCnhUrl))
            throw new ArgumentException("URL da foto é obrigatória");

        FotoCnhUrl = fotoCnhUrl;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void RemoverFotoCnh()
    {
        FotoCnhUrl = null;
        DataAtualizacao = DateTime.UtcNow;
    }
}