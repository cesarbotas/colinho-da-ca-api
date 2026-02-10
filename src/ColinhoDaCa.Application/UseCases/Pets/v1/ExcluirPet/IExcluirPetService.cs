namespace ColinhoDaCa.Application.UseCases.Pets.v1.ExcluirPet;

public interface IExcluirPetService
{
    Task Handle(long id);
}
