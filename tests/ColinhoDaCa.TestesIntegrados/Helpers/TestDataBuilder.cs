using Bogus;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;
using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;

namespace ColinhoDaCa.TestesIntegrados.Helpers;

public static class TestDataBuilder
{
    private static readonly Faker _faker = new("pt_BR");

    public static Faker<RegistrarCommand> RegistrarCommandFaker => new Faker<RegistrarCommand>()
        .RuleFor(x => x.Nome, f => f.Person.FullName)
        .RuleFor(x => x.Email, f => f.Internet.Email())
        .RuleFor(x => x.Celular, f => f.Phone.PhoneNumber("11#########"))
        .RuleFor(x => x.Cpf, f => GenerateValidCpf())
        .RuleFor(x => x.Senha, f => "Test123!");

    private static string GenerateValidCpf()
    {
        var cpf = new int[11];
        var random = new Random();
        
        // Gerar os 9 primeiros dígitos
        for (int i = 0; i < 9; i++)
        {
            cpf[i] = random.Next(0, 10);
        }
        
        // Calcular primeiro dígito verificador
        var soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += cpf[i] * (10 - i);
        }
        var resto = soma % 11;
        cpf[9] = resto < 2 ? 0 : 11 - resto;
        
        // Calcular segundo dígito verificador
        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += cpf[i] * (11 - i);
        }
        resto = soma % 11;
        cpf[10] = resto < 2 ? 0 : 11 - resto;
        
        return string.Join("", cpf);
    }

    public static LoginCommand CreateLoginCommand(string email, string senha)
    {
        return new LoginCommand
        {
            Email = email,
            Senha = senha
        };
    }

    public static CadastrarClienteCommand CreateClienteCommand()
    {
        return new CadastrarClienteCommand
        {
            Nome = _faker.Person.FullName,
            Email = _faker.Internet.Email(),
            Celular = _faker.Phone.PhoneNumber("11#########"),
            Cpf = GenerateValidCpf(),
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
            Cpf = GenerateValidCpf(),
            Observacoes = _faker.Lorem.Sentence()
        };
    }

    public static CadastrarPetCommand CreatePetCommand(long clienteId)
    {
        return new CadastrarPetCommand
        {
            Nome = _faker.Name.FirstName(),
            RacaId = _faker.Random.Int(1, 34), // IDs das raças inseridas no script (1-34)
            Idade = _faker.Random.Int(1, 15),
            Peso = _faker.Random.Double(1, 50),
            Porte = _faker.PickRandom("P", "M", "G"),
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