using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Cupons.Repositories;

namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AplicarCupom;

public class AplicarCupomService : IAplicarCupomService
{
    private readonly ICupomRepository _cupomRepository;

    public AplicarCupomService(ICupomRepository cupomRepository)
    {
        _cupomRepository = cupomRepository;
    }

    public async Task<AplicarCupomResponse> Handle(long reservaId, AplicarCupomCommand command)
    {
        var cupom = await _cupomRepository.GetByCodigoAsync(command.CodigoCupom);
        if (cupom == null)
            throw new ValidationException("Cupom não encontrado");

        if (!cupom.Ativo)
            throw new ValidationException("Cupom inativo");

        if (cupom.DataInicio.HasValue && DateTime.Now < cupom.DataInicio.Value)
            throw new ValidationException("Cupom ainda não está válido");

        if (cupom.DataFim.HasValue && DateTime.Now > cupom.DataFim.Value)
            throw new ValidationException("Cupom expirado");

        var valorDesconto = cupom.CalcularDesconto(command.ValorTotal, command.QuantidadePets, command.QuantidadeDiarias);

        if (valorDesconto == 0)
            throw new ValidationException("Cupom não atende aos requisitos mínimos para esta reserva");

        return new AplicarCupomResponse
        {
            ValorTotal = command.ValorTotal,
            ValorDesconto = valorDesconto,
            ValorFinal = command.ValorTotal - valorDesconto,
            CupomAplicado = cupom.Codigo
        };
    }
}