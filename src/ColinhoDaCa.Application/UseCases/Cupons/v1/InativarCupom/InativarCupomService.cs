using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Cupons.Repositories;

namespace ColinhoDaCa.Application.UseCases.Cupons.v1.InativarCupom;

public class InativarCupomService : IInativarCupomService
{
    private readonly ICupomRepository _cupomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InativarCupomService(ICupomRepository cupomRepository, IUnitOfWork unitOfWork)
    {
        _cupomRepository = cupomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(long id)
    {
        var cupom = await _cupomRepository.GetAsync(c => c.Id == id);
        if (cupom == null)
            throw new ValidationException("Cupom não encontrado");

        cupom.Inativar();

        _cupomRepository.Update(cupom);
        await _unitOfWork.CommitAsync();
    }
}