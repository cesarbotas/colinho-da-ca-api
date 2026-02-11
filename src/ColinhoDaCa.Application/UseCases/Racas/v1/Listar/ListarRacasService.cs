using ColinhoDaCa.Domain.Racas.Repositories;

namespace ColinhoDaCa.Application.UseCases.Racas.v1.Listar;

public class ListarRacasService : IListarRacasService
{
    private readonly IRacaRepository _racaRepository;

    public ListarRacasService(IRacaRepository racaRepository)
    {
        _racaRepository = racaRepository;
    }

    public async Task<List<ListarRacasResponse>> Handle(long? racaId)
    {
        if (racaId.HasValue)
        {
            var raca = await _racaRepository.GetByIdAsync(racaId.Value);
            
            if (raca == null)
                return new List<ListarRacasResponse>();

            return new List<ListarRacasResponse>
            {
                new ListarRacasResponse
                {
                    Id = raca.Id,
                    Nome = raca.Nome,
                    Porte = raca.Porte
                }
            };
        }

        var racas = await _racaRepository.GetAllAsync();

        return racas.Select(r => new ListarRacasResponse
        {
            Id = r.Id,
            Nome = r.Nome,
            Porte = r.Porte
        }).ToList();
    }
}
