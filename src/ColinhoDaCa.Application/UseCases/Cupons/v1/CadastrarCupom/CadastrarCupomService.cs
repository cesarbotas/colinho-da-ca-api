using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Cupons.Entities;
using ColinhoDaCa.Domain.Cupons.Enums;
using ColinhoDaCa.Domain.Cupons.Repositories;

namespace ColinhoDaCa.Application.UseCases.Cupons.v1.CadastrarCupom;

public class CadastrarCupomService : ICadastrarCupomService
{
    private readonly ICupomRepository _cupomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarCupomService(ICupomRepository cupomRepository, IUnitOfWork unitOfWork)
    {
        _cupomRepository = cupomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CadastrarCupomCommand command)
    {
        var cupomExistente = await _cupomRepository.GetByCodigoAsync(command.Codigo);
        if (cupomExistente != null)
            throw new ValidationException("Código de cupom já existe");

        var cupom = CupomDb.Create(
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

        await _cupomRepository.InsertAsync(cupom);
        await _unitOfWork.CommitAsync();
    }
}