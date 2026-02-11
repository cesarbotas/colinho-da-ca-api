namespace ColinhoDaCa.Application.UseCases.Racas.v1.Listar;

public interface IListarRacasService
{
    Task<List<ListarRacasResponse>> Handle(long? racaId);
}
