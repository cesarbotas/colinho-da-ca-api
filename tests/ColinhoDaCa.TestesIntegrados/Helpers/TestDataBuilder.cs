using Bogus;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;

namespace ColinhoDaCa.TestesIntegrados.Helpers;

public static class TestDataBuilder
{
    private static readonly Faker _faker = new("pt_BR");

    public static LoginCommand CreateLoginCommand()
    {
        return new LoginCommand
        {
            Email = "admin@test.com",
            Senha = "Admin123!"
        };
    }

    public static CadastrarClienteCommand CreateClienteCommand()
    {
        return new CadastrarClienteCommand
        {
            Nome = _faker.Person.FullName,
            Email = _faker.Internet.Email(),
            Celular = _faker.Phone.PhoneNumber("11#########"),
            Cpf = _faker.Random.Replace("###########"),
            Observacoes = _faker.Lorem.Sentence()
        };
    }

    public static AlterarClienteCommand CreateAlterarClienteCommand()
    {
        return new AlterarClienteCommand
        {
            Nome = _faker.Person.FullName,
            Email = _faker.Internet.Email(),
            Celular = _faker.Phone.PhoneNumber("11#########"),
            Cpf = _faker.Random.Replace("###########"),
            Observacoes = _faker.Lorem.Sentence()
        };
    }

    public static CadastrarPetCommand CreatePetCommand(long clienteId)
    {
        return new CadastrarPetCommand
        {
            Nome = _faker.Name.FirstName(),
            RacaId = 1,
            Idade = _faker.Random.Int(1, 15),
            Peso = _faker.Random.Double(1, 50),
            Porte = _faker.PickRandom("Pequeno", "Médio", "Grande"),
            Observacoes = _faker.Lorem.Sentence(),
            ClienteId = clienteId
        };
    }

    public static CadastrarReservaCommand CreateReservaCommand(long clienteId, long[] petIds)
    {
        var dataInicial = _faker.Date.Future(1);
        return new CadastrarReservaCommand
        {
            ClienteId = clienteId,
            DataInicial = dataInicial,
            DataFinal = dataInicial.AddDays(_faker.Random.Int(1, 7)),
            Observacoes = _faker.Lorem.Sentence(),
            PetIds = petIds.ToList()
        };
    }
}