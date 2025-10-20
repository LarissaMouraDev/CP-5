use MotosScanDB
db.createCollection("Motos")
db.createCollection("Motoristas")
db.createCollection("Manutencoes")

// Criar índices
db.Motos.createIndex({ "placa": 1 }, { unique: true })
db.Motoristas.createIndex({ "cpf": 1 }, { unique: true })
db.Motoristas.createIndex({ "cnh": 1 }, { unique: true })
db.Manutencoes.createIndex({ "motoId": 1 })
db.Manutencoes.createIndex({ "motoristaId": 1 })
db.Manutencoes.createIndex({ "status": 1 })

// Inserir dados de exemplo
db.Motos.insertMany([
  {
    modelo: "Honda CG 160 Titan",
    placa: "ABC-1234",
    ano: 2024,
    quilometragem: 5000,
    status: "Disponivel",
    dataCadastro: new Date(),
    dataAtualizacao: new Date()
  },
  {
    modelo: "Yamaha Factor 150",
    placa: "XYZ-5678",
    ano: 2023,
    quilometragem: 15000,
    status: "EmUso",
    dataCadastro: new Date(),
    dataAtualizacao: new Date()
  }
])

db.Motoristas.insertMany([
  {
    nome: "João Silva",
    cpf: "123.456.789-00",
    cnh: "12345678900",
    dataNascimento: new Date("1990-05-15"),
    telefone: "(11) 98765-4321",
    email: "joao.silva@email.com",
    fotoCnhUrl: null,
    dataCadastro: new Date(),
    dataAtualizacao: new Date()
  }
])

print("Banco de dados configurado com sucesso!")
EOF