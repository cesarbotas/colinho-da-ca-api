using ColinhoDaCa.Domain.LoginHistorico.Entities;
using ColinhoDaCa.Domain.LoginHistorico.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.LoginHistorico;

public class LoginHistoricoRepository : Repository<LoginHistoricoDb>, ILoginHistoricoRepository
{
    public LoginHistoricoRepository(ColinhoDaCaContext context) : base(context)
    {
    }
}