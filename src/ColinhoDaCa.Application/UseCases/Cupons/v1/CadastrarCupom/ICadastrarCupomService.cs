namespace ColinhoDaCa.Application.UseCases.Cupons.v1.CadastrarCupom;

public interface ICadastrarCupomService
{
    Task Handle(CadastrarCupomCommand command);
}
