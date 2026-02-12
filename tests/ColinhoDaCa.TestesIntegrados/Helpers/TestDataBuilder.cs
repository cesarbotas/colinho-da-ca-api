using Bogus;
using Bogus.Extensions.Brazil;

namespace ColinhoDaCa.TestesIntegrados.Helpers;

public static class TestDataBuilder
{
    public static Faker<RegistrarCommand> RegistrarCommandFaker => new Faker<RegistrarCommand>("pt_BR")
        .RuleFor(r => r.Nome, f => f.Name.FullName())
        .RuleFor(r => r.Email, f => f.Internet.Email())
        .RuleFor(r => r.Celular, f => f.Phone.PhoneNumber("11#########"))
        .RuleFor(r => r.Cpf, f => f.Person.Cpf(false))
        .RuleFor(r => r.Senha, f => "Senha@123");

    public static Faker<CadastrarClienteCommand> ClienteCommandFaker => new Faker<CadastrarClienteCommand>("pt_BR")
        .RuleFor(c => c.Nome, f => f.Name.FullName())
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.Celular, f => f.Phone.PhoneNumber("11#########"))
        .RuleFor(c => c.Cpf, f => f.Person.Cpf(false))
        .RuleFor(c => c.Observacoes, f => f.Lorem.Sentence());

    public static Faker<CadastrarPetCommand> PetCommandFaker => new Faker<CadastrarPetCommand>("pt_BR")
        .RuleFor(p => p.Nome, f => f.Name.FirstName())
        .RuleFor(p => p.RacaId, f => f.Random.Long(1, 36))
        .RuleFor(p => p.Idade, f => f.Random.Int(1, 15))
        .RuleFor(p => p.Peso, f => f.Random.Double(1, 50))
        .RuleFor(p => p.Porte, f => f.PickRandom("P", "M", "G"))
        .RuleFor(p => p.Observacoes, f => f.Lorem.Sentence())
        .RuleFor(p => p.ClienteId, 1);
}

public class RegistrarCommand
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public string Cpf { get; set; }
    public string Senha { get; set; }
}

public class CadastrarClienteCommand
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public string Cpf { get; set; }
    public string Observacoes { get; set; }
}

public class CadastrarPetCommand
{
    public string Nome { get; set; }
    public long? RacaId { get; set; }
    public int Idade { get; set; }
    public double Peso { get; set; }
    public string Porte { get; set; }
    public string Observacoes { get; set; }
    public long ClienteId { get; set; }
}
