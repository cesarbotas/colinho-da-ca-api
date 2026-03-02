# Estrutura de Pastas - ColinhoDaCa.Infra.Data

```
ColinhoDaCa.Infra.Data/
├── _Shared/
│   └── Postgres/
│       ├── Repositories/
│       │   └── Repository.cs
│       └── UoW/
│           └── UnitOfWork.cs
├── Context/
│   ├── Configuration/
│   │   ├── ClienteConfiguration.cs
│   │   ├── CupomConfiguration.cs
│   │   ├── LoginHistoricoConfiguration.cs
│   │   ├── PerfilConfiguration.cs
│   │   ├── PetConfiguration.cs
│   │   ├── RacaConfiguration.cs
│   │   ├── RefreshTokenConfiguration.cs
│   │   ├── ReservaConfiguration.cs
│   │   ├── ReservaPetConfiguration.cs
│   │   ├── ReservaStatusHistoricoConfiguration.cs
│   │   ├── UsuarioConfiguration.cs
│   │   └── UsuarioPerfilConfiguration.cs
│   ├── Repositories/
│   │   ├── Clientes/
│   │   │   └── ClienteRepository.cs
│   │   ├── Cupons/
│   │   │   └── CupomRepository.cs
│   │   ├── LoginHistoricos/
│   │   │   └── LoginHistoricoRepository.cs
│   │   ├── Pets/
│   │   │   └── PetRepository.cs
│   │   ├── RefreshTokens/
│   │   │   └── RefreshTokenRepository.cs
│   │   ├── Reservas/
│   │   │   └── ReservaRepository.cs
│   │   └── Usuarios/
│   │       └── UsuarioRepository.cs
│   └── ColinhoDaCaContext.cs
├── Racas/
│   └── RacaRepository.cs
├── ColinhoDaCa.Infra.Data.csproj
└── CONTEXT.md
```

## 📁 Descrição das Pastas e Arquivos

### _Shared/Postgres/
Componentes compartilhados para persistência PostgreSQL.

**Repositories/Repository.cs**
- Repositório genérico base com operações CRUD
- Métodos: AddAsync, UpdateAsync, DeleteAsync, GetAsync, GetAllAsync

**UoW/UnitOfWork.cs**
- Implementação do pattern Unit of Work
- Gerencia transações e SaveChanges

### Context/
Contexto principal do Entity Framework Core.

**ColinhoDaCaContext.cs**
- DbContext com 12 DbSets (Clientes, Pets, Racas, Reservas, etc.)
- Configuração do schema "public"
- Aplicação de configurações via IEntityTypeConfiguration

### Context/Configuration/
Configurações EF Core para cada entidade (IEntityTypeConfiguration).

**12 Arquivos de Configuração**:
- ClienteConfiguration.cs - Tabela Clientes
- PetConfiguration.cs - Tabela Pets
- RacaConfiguration.cs - Tabela Racas (com seed de 36 raças)
- ReservaConfiguration.cs - Tabela Reservas
- ReservaPetConfiguration.cs - Tabela N:N (Reserva ↔ Pet)
- ReservaStatusHistoricoConfiguration.cs - Histórico de status
- CupomConfiguration.cs - Tabela Cupons
- UsuarioConfiguration.cs - Tabela Usuarios
- PerfilConfiguration.cs - Tabela Perfis (com seed)
- UsuarioPerfilConfiguration.cs - Tabela N:N (Usuario ↔ Perfil)
- LoginHistoricoConfiguration.cs - Histórico de logins
- RefreshTokenConfiguration.cs - Tokens de refresh OAuth2

### Context/Repositories/
Implementações específicas de repositórios por domínio.

**7 Pastas de Repositórios**:
- Clientes/ - ClienteRepository (IClienteRepository + IClienteReadRepository)
- Pets/ - PetRepository (IPetRepository + IPetReadRepository)
- Reservas/ - ReservaRepository (IReservaRepository + IReservaReadRepository)
- Cupons/ - CupomRepository (ICupomRepository)
- Usuarios/ - UsuarioRepository (IUsuarioRepository)
- LoginHistoricos/ - LoginHistoricoRepository (ILoginHistoricoRepository)
- RefreshTokens/ - RefreshTokenRepository (IRefreshTokenRepository)

### Racas/
Repositório de raças (fora de Context por ser mais simples).

**RacaRepository.cs**
- IRacaRepository
- Métodos: GetAll, GetById

### Arquivos Raiz

**ColinhoDaCa.Infra.Data.csproj**
- Referências: Domain, EF Core, Npgsql

**CONTEXT.md**
- Documentação completa da camada de infraestrutura
