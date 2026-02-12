using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Cupons.Enums;
using ColinhoDaCa.Domain.Cupons.Repositories;

namespace ColinhoDaCa.Application.UseCases.Cupons.v1.AlterarCupom;

public class AlterarCupomService : IAlterarCupomService
{
    private readonly ICupomRepository _cupomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AlterarCupomService(ICupomRepository cupomRepository, IUnitOfWork unitOfWork)
    {
        _cupomRepository = cupomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(long id, AlterarCupomCommand command)
    {
        var cupom = await _cupomRepository.GetAsync(c => c.Id == id);
        if (cupom == null)
            throw new ValidationException("Cupom não encontrado");

        cupom.Alterar(
            command.Codigo,
            command.Descricao,
            (TipoCupom)command.Tipo,
            command.Percentual,
            command.ValorFixo,
            command.MinimoValorTotal,
            command.MinimoPets,
            command.MinimoDiarias,
            command.DataInicio,
            command.DataFim
        );

        _cupomRepository.Update(cupom);
        await _unitOfWork.CommitAsync();
    }
}