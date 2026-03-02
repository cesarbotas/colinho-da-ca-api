# ColinhoDaCa.IoC - Injeção de Dependências

## 📋 Responsabilidade
Centraliza toda a configuração de injeção de dependências do projeto, registrando serviços, repositórios, contextos e use cases.

## 🏗️ Estrutura

```
ColinhoDaCa.IoC/
├── Extensions/
│   ├── PersistenceExtensions.cs      # Configuração de persistência (DbContext, UoW, Repositórios)
│   └── UseCaseExtensions.cs          # Registro de Use Cases e Services
└── ServiceRegistrationExtensions.cs  # Ponto de entrada principal
```

## 🔧 ServiceRegistrationExtensions.cs

**Método Principal**: `RegistraDependencias()`

Orquestra o registro de todas as dependências do projeto:
- Unit of Work
- DbContext (PostgreSQL)
- Repositórios
- Use Cases
- Services (Email, Auth, Validation)

**Uso**:
```csharp
// Program.cs
builder.Services.RegistraDependencias(builder.Configuration);
```

## 📦 PersistenceExtensions.cs

### AdicionarContextoDb()
- Configura `ColinhoDaCaContext` com PostgreSQL
- Connection String: `ColinhoDaCaRender`
- Retry automático: 5 tentativas, 10s delay
- Logs habilitados (desenvolvimento)

### AdicionarUnitOfWork()
- Registra `IUnitOfWork` → `UnitOfWork`
- Scoped lifetime

### AdicionarRepositorios()
Registra todos os repositórios:

**Clientes**:
- `IClienteRepository` → `ClienteRepository`
- `IClienteReadRepository` → `ClienteRepository`

**Pets**:
- `IPetRepository` → `PetRepository`
- `IPetReadRepository` → `PetRepository`

**Raças**:
- `IRacaRepository` → `RacaRepository`

**Reservas**:
- `IReservaRepository` → `ReservaRepository`
- `IReservaReadRepository` → `ReservaRepository`

**Cupons**:
- `ICupomRepository` → `CupomRepository`

**Usuários**:
- `IUsuarioRepository` → `UsuarioRepository`

**Login Histórico**:
- `ILoginHistoricoRepository` → `LoginHistoricoRepository`

**Refresh Tokens**:
- `IRefreshTokenRepository` → `RefreshTokenRepository`

## 🎯 UseCaseExtensions.cs

### AdicionarUseCases()
Registra todos os use cases e services (Scoped):

**Auth**:
- `ILoginService` → `LoginService`
- `IRefreshTokenService` → `RefreshTokenService`
- `IRegistrarService` → `RegistrarService`

**Clientes**:
- `ICadastrarClienteService` → `CadastrarClienteService`
- `IListarClienteService` → `ListarClienteService`
- `IAlterarClienteService` → `AlterarClienteService`
- `IExcluirClienteService` → `ExcluirClienteService`

**Pets**:
- `ICadastrarPetService` → `CadastrarPetService`
- `IListarPetService` → `ListarPetService`
- `IAlterarPetService` → `AlterarPetService`
- `IExcluirPetService` → `ExcluirPetService`

**Raças**:
- `IListarRacasService` → `ListarRacasService`

**Reservas**:
- `ICadastrarReservaService` → `CadastrarReservaService`
- `IListarReservaService` → `ListarReservaService`
- `IAlterarReservaService` → `AlterarReservaService`
- `IExcluirReservaService` → `ExcluirReservaService`
- `IConfirmarReservaService` → `ConfirmarReservaService`
- `IEnviarComprovanteService` → `EnviarComprovanteService`
- `IAprovarPagamentoService` → `AprovarPagamentoService`
- `IAplicarCupomService` → `AplicarCupomService`
- `IConcederDescontoService` → `ConcederDescontoService`
- `ICancelarReservaService` → `CancelarReservaService`

**Cupons**:
- `ICadastrarCupomService` → `CadastrarCupomService`
- `IListarCupomService` → `ListarCupomService`
- `IAlterarCupomService` → `AlterarCupomService`
- `IInativarCupomService` → `InativarCupomService`

**Sobre**:
- `IEnviarEmailService` → `EnviarEmailService`

**Services Compartilhados**:
- `IEmailService` → `EmailService`
- `IPasswordService` → `PasswordService`
- `IJwtService` → `JwtService`
- `ICpfValidationService` → `CpfValidationService`

## 🔄 Lifetime dos Serviços

Todos os serviços são registrados como **Scoped**:
- Nova instância por requisição HTTP
- Compartilhada dentro da mesma requisição
- Descartada ao final da requisição

## 📝 Padrão de Nomenclatura

- **Interface**: `I{Acao}{Entidade}Service`
- **Implementação**: `{Acao}{Entidade}Service`
- **Exemplo**: `ICadastrarClienteService` → `CadastrarClienteService`

## 🎯 Boas Práticas

1. **Separação de Responsabilidades**: Extensions separadas por domínio
2. **Configuração Centralizada**: Único ponto de entrada
3. **Injeção por Interface**: Facilita testes e manutenção
4. **Scoped Lifetime**: Adequado para aplicações web
5. **Retry Policy**: Resiliência em conexões com banco
