# ColinhoDaCa.Infra.Data - Camada de Infraestrutura

## 📋 Responsabilidade
Implementa a persistência de dados usando Entity Framework Core com PostgreSQL, incluindo contexto, configurações de entidades e repositórios.

## 🏗️ Estrutura

```
ColinhoDaCa.Infra.Data/
├── _Shared/
│   └── Postgres/
│       ├── Repositories/
│       │   └── Repository.cs              # Repositório base genérico
│       └── UoW/
│           └── UnitOfWork.cs              # Unit of Work pattern
├── Context/
│   ├── Configuration/                     # Configurações EF Core
│   │   ├── ClienteConfiguration.cs
│   │   ├── PetConfiguration.cs
│   │   ├── RacaConfiguration.cs
│   │   ├── ReservaConfiguration.cs
│   │   ├── ReservaPetConfiguration.cs
│   │   ├── ReservaStatusHistoricoConfiguration.cs
│   │   ├── CupomConfiguration.cs
│   │   ├── UsuarioConfiguration.cs
│   │   ├── PerfilConfiguration.cs
│   │   ├── UsuarioPerfilConfiguration.cs
│   │   ├── LoginHistoricoConfiguration.cs
│   │   └── RefreshTokenConfiguration.cs
│   ├── Repositories/                      # Implementações de repositórios
│   │   ├── Clientes/
│   │   │   └── ClienteRepository.cs
│   │   ├── Pets/
│   │   │   └── PetRepository.cs
│   │   ├── Reservas/
│   │   │   └── ReservaRepository.cs
│   │   ├── Cupons/
│   │   │   └── CupomRepository.cs
│   │   ├── Usuarios/
│   │   │   └── UsuarioRepository.cs
│   │   ├── LoginHistoricos/
│   │   │   └── LoginHistoricoRepository.cs
│   │   └── RefreshTokens/
│   │       └── RefreshTokenRepository.cs
│   └── ColinhoDaCaContext.cs              # DbContext principal
└── Racas/
    └── RacaRepository.cs                  # Repositório de raças
```

## 🗄️ ColinhoDaCaContext.cs

**DbContext Principal** do projeto com todas as entidades:

### DbSets
- `Clientes` - Cadastro de clientes
- `Pets` - Pets dos clientes
- `Racas` - Raças de pets (36 pré-cadastradas)
- `Reservas` - Reservas de hospedagem
- `ReservaPets` - Relacionamento N:N (Reserva ↔ Pet)
- `ReservaStatusHistorico` - Histórico de mudanças de status
- `Cupons` - Cupons de desconto
- `Usuarios` - Usuários do sistema
- `Perfis` - Perfis de acesso (Administrador, Cliente)
- `UsuarioPerfis` - Relacionamento N:N (Usuario ↔ Perfil)
- `LoginHistorico` - Histórico de logins
- `RefreshTokens` - Tokens de refresh OAuth2

### Configurações
- **Schema**: `public`
- **Configurações**: Aplicadas via `IEntityTypeConfiguration`
- **Assembly**: `ClienteConfiguration.Assembly`

## 🔧 Configuration/ (IEntityTypeConfiguration)

Cada entidade possui sua configuração isolada:

### Padrões Comuns
- **Primary Key**: `Id` (BIGINT IDENTITY)
- **Campos Auditoria**: `DataInclusao`, `DataAlteracao`
- **Naming**: PascalCase (C#) → PascalCase (PostgreSQL)
- **Schema**: `public`

### Configurações Específicas

**ClienteConfiguration**:
- Email único (índice)
- CPF único (índice)
- Relacionamento 1:N com Pets
- Relacionamento 1:1 com Usuario

**PetConfiguration**:
- FK para Cliente (obrigatória)
- FK para Raca (opcional, SRD)
- Relacionamento N:N com Reservas

**RacaConfiguration**:
- Nome único
- Porte (P/M/G/null para SRD)
- Seed de 36 raças

**ReservaConfiguration**:
- FK para Cliente (obrigatória)
- FK para Cupom (opcional)
- Status (enum)
- Campos de pagamento
- Relacionamento N:N com Pets

**ReservaPetConfiguration**:
- Chave composta (ReservaId, PetId)
- FKs obrigatórias

**ReservaStatusHistoricoConfiguration**:
- FK para Reserva (obrigatória)
- FK para Usuario (obrigatória)
- Índice em ReservaId

**CupomConfiguration**:
- Codigo único (índice)
- Tipo (enum)
- Validações de período

**UsuarioConfiguration**:
- FK para Cliente (única)
- Relacionamento N:N com Perfis
- Campo Ativo

**PerfilConfiguration**:
- Nome único
- Seed: Administrador (1), Cliente (2)

**UsuarioPerfilConfiguration**:
- Chave composta (UsuarioId, PerfilId)

**LoginHistoricoConfiguration**:
- FK para Usuario (obrigatória)
- Índice em UsuarioId
- Campos de dispositivo e IP

**RefreshTokenConfiguration**:
- FK para Usuario (obrigatória)
- Token único (índice)
- Campos de expiração e revogação

## 📦 Repositories/

### Padrão Repository
Cada repositório implementa operações específicas da entidade:

**ClienteRepository**:
- `IClienteRepository` (escrita)
- `IClienteReadRepository` (leitura)
- Métodos: Add, Update, Delete, GetById, GetByEmail, GetByCpf, GetAll (paginado)

**PetRepository**:
- `IPetRepository` (escrita)
- `IPetReadRepository` (leitura)
- Métodos: Add, Update, Delete, GetById, GetByClienteId (paginado)
- Inclui Raca no retorno

**ReservaRepository**:
- `IReservaRepository` (escrita)
- `IReservaReadRepository` (leitura)
- Métodos: Add, Update, Delete, GetById, GetAll (paginado)
- Inclui: Cliente, Pets (com Raca), Cupom, StatusHistorico

**CupomRepository**:
- `ICupomRepository`
- Métodos: Add, Update, GetById, GetByCodigo, GetAll (paginado)

**UsuarioRepository**:
- `IUsuarioRepository`
- Métodos: Add, GetByClienteId, GetById
- Inclui: Cliente, Perfis

**LoginHistoricoRepository**:
- `ILoginHistoricoRepository`
- Métodos: Add

**RefreshTokenRepository**:
- `IRefreshTokenRepository`
- Métodos: Add, GetByToken, RevokeByUsuarioId, Update

**RacaRepository**:
- `IRacaRepository`
- Métodos: GetAll, GetById

## 🔄 UnitOfWork.cs

**Pattern**: Unit of Work

### Responsabilidade
Gerencia transações e persistência de mudanças no banco.

### Métodos
- `CommitAsync()` - Salva todas as mudanças pendentes
- `SaveChangesAsync()` - Alias para CommitAsync

### Uso
```csharp
await _repository.AddAsync(entity);
await _unitOfWork.CommitAsync();
```

## 🗃️ Repository.cs (Base)

**Repositório Genérico** com operações CRUD básicas:

### Métodos
- `AddAsync<T>(T entity)`
- `UpdateAsync<T>(T entity)`
- `DeleteAsync<T>(T entity)`
- `GetAsync<T>(Expression<Func<T, bool>> predicate)`
- `GetAllAsync<T>()`

### Características
- Genérico para qualquer entidade
- Suporte a expressões LINQ
- Async/await
- Tracking automático do EF Core

## 🔗 Relacionamentos

### 1:N (Um para Muitos)
- Cliente → Pets
- Cliente → Reservas
- Raca → Pets
- Reserva → ReservaStatusHistorico
- Usuario → LoginHistorico
- Usuario → RefreshTokens

### N:N (Muitos para Muitos)
- Reserva ↔ Pet (via ReservaPets)
- Usuario ↔ Perfil (via UsuarioPerfis)

### 1:1 (Um para Um)
- Cliente ↔ Usuario

## 📊 Migrations

Migrations são gerenciadas via scripts SQL em `database/scripts/`:
- 01_TabelaClientes.sql
- 02_TabelaPets.sql
- 03_CamposDataInclusaoAlteracao.sql
- 04_TabelaReservas.sql
- 05_TabelaUsuarios.sql
- ... (21 scripts no total)

## 🎯 Boas Práticas

1. **Separação Read/Write**: Repositórios separados para leitura e escrita
2. **Eager Loading**: Includes explícitos para evitar N+1
3. **Paginação**: Sempre usar Skip/Take em listagens
4. **Índices**: Campos únicos e FKs indexados
5. **Configurações Isoladas**: Uma classe por entidade
6. **Unit of Work**: Controle transacional centralizado
7. **Async/Await**: Todas as operações assíncronas
8. **Retry Policy**: Resiliência em falhas de conexão

## 🔍 Queries Otimizadas

### Exemplo: Listar Reservas
```csharp
return await _context.Reservas
    .Include(r => r.Cliente)
    .Include(r => r.ReservaPets)
        .ThenInclude(rp => rp.Pet)
            .ThenInclude(p => p.Raca)
    .Include(r => r.Cupom)
    .Include(r => r.StatusHistorico)
        .ThenInclude(sh => sh.Usuario)
            .ThenInclude(u => u.Cliente)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

## 📝 Connection String

**Configuração**: `appsettings.json`
```json
{
  "ConnectionStrings": {
    "ColinhoDaCaRender": "Host=...;Database=...;Username=...;Password=..."
  }
}
```

**Retry Policy**:
- Max Retries: 5
- Max Delay: 10 segundos
- Automático em falhas transientes
