using ColinhoDaCa.Domain.LoginHistoricos.Entities;
using ColinhoDaCa.Domain.LoginHistoricos.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.LoginHistoricos;

public class LoginHistoricoRepository : Repository<LoginHistorico>, ILoginHistoricoRepository
{
    public LoginHistoricoRepository(ColinhoDaCaContext context) : base(context)
    {
    }
}