namespace ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;

public interface ICadastrarPetService
{
    Task Handle(CadastrarPetCommand command);
}
