namespace ColinhoDaCa.Application.UseCases.Cupons.v1.AlterarCupom;

public interface IAlterarCupomService
{
    Task Handle(long id, AlterarCupomCommand command);
}
