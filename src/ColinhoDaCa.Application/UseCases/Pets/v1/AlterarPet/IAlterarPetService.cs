namespace ColinhoDaCa.Application.UseCases.Pets.v1.AlterarPet;

public interface IAlterarPetService
{
    Task Handle(long id, AlterarPetCommand command);
}
