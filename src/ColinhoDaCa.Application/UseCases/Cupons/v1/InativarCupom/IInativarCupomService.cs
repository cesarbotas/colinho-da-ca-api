namespace ColinhoDaCa.Application.UseCases.Cupons.v1.InativarCupom;

public interface IInativarCupomService
{
    Task Handle(long id);
}
