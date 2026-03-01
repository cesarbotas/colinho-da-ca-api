# Padrão de Estrutura de Pastas - Clean Architecture

## 📁 Estrutura Completa do Projeto

```
{projeto}-api/
├── src/
│   ├── {Projeto}.Domain/
│   │   ├── _Shared/
│   │   │   ├── Entities/
│   │   │   │   ├── IRepository.cs
│   │   │   │   └── IUnitOfWork.cs
│   │   │   └── Exceptions/
│   │   │       ├── BaseException.cs
│   │   │       ├── ValidationException.cs
│   │   │       └── EntityNotFoundException.cs
│   │   ├── {Entidade1}/
│   │   │   ├── Entities/
│   │   │   │   └── {Entidade1}.cs
│   │   │   ├── Enums/
│   │   │   │   └── {Entidade1}Status.cs
│   │   │   └── Repositories/
│   │   │       └── I{Entidade1}Repository.cs
│   │   ├── {Entidade2}/
│   │   │   ├── Entities/
│   │   │   ├── Enums/
│   │   │   └── Repositories/
│   │   └── {Projeto}.Domain.csproj
│   │
│   ├── {Projeto}.Application/
│   │   ├── _Shared/
│   │   │   └── DTOs/
│   │   │       └── Paginacao/
│   │   │           ├── PaginacaoDto.cs
│   │   │           └── ResultadoPaginadoDto.cs
│   │   ├── DTOs/
│   │   │   ├── {Entidade1}/
│   │   │   │   └── {Entidade1}Dto.cs
│   │   │   └── {Entidade2}/
│   │   │       └── {Entidade2}Dto.cs
│   │   ├── Services/
│   │   │   ├── Auth/
│   │   │   │   ├── IJwtService.cs
│   │   │   │   ├── JwtService.cs
│   │   │   │   ├── IPasswordService.cs
│   │   │   │   └── PasswordService.cs
│   │   │   ├── Email/
│   │   │   │   ├── IEmailService.cs
│   │   │   │   └── EmailService.cs
│   │   │   ├── EmailTemplates/
│   │   │   │   └── EmailTemplateService.cs
│   │   │   └── Validation/
│   │   │       ├── ICpfValidationService.cs
│   │   │       └── CpfValidationService.cs
│   │   ├── Repositories/
│   │   │   ├── {Entidade1}/
│   │   │   │   └── I{Entidade1}ReadRepository.cs
│   │   │   └── {Entidade2}/
│   │   │       └── I{Entidade2}ReadRepository.cs
│   │   ├── UseCases/
│   │   │   ├── Auth/
│   │   │   │   └── v1/
│   │   │   │       ├── Login/
│   │   │   │       │   ├── ILoginService.cs
│   │   │   │       │   ├── LoginService.cs
│   │   │   │       │   ├── LoginCommand.cs
│   │   │   │       │   └── LoginResponse.cs
│   │   │   │       ├── Registrar/
│   │   │   │       │   ├── IRegistrarService.cs
│   │   │   │       │   ├── RegistrarService.cs
│   │   │   │       │   └── RegistrarCommand.cs
│   │   │   │       └── RefreshTokens/
│   │   │   │           ├── IRefreshTokenService.cs
│   │   │   │           ├── RefreshTokenService.cs
│   │   │   │           └── RefreshTokenCommand.cs
│   │   │   ├── {Entidade1}/
│   │   │   │   └── v1/
│   │   │   │       ├── Cadastrar{Entidade1}/
│   │   │   │       │   ├── ICadastrar{Entidade1}Service.cs
│   │   │   │       │   ├── Cadastrar{Entidade1}Service.cs
│   │   │   │       │   └── Cadastrar{Entidade1}Command.cs
│   │   │   │       ├── Alterar{Entidade1}/
│   │   │   │       │   ├── IAlterar{Entidade1}Service.cs
│   │   │   │       │   ├── Alterar{Entidade1}Service.cs
│   │   │   │       │   └── Alterar{Entidade1}Command.cs
│   │   │   │       ├── Excluir{Entidade1}/
│   │   │   │       │   ├── IExcluir{Entidade1}Service.cs
│   │   │   │       │   └── Excluir{Entidade1}Service.cs
│   │   │   │       └── Listar{Entidade1}/
│   │   │   │           ├── IListar{Entidade1}Service.cs
│   │   │   │           ├── Listar{Entidade1}Service.cs
│   │   │   │           └── Listar{Entidade1}Query.cs
│   │   │   └── {Entidade2}/
│   │   │       └── v1/
│   │   │           ├── Cadastrar{Entidade2}/
│   │   │           ├── Alterar{Entidade2}/
│   │   │           ├── Excluir{Entidade2}/
│   │   │           └── Listar{Entidade2}/
│   │   └── {Projeto}.Application.csproj
│   │
│   ├── {Projeto}.Infra.Data/
│   │   ├── _Shared/
│   │   │   └── Postgres/
│   │   │       ├── Repositories/
│   │   │       │   └── Repository.cs
│   │   │       └── UoW/
│   │   │           └── UnitOfWork.cs
│   │   ├── Context/
│   │   │   ├── Configuration/
│   │   │   │   ├── {Entidade1}Configuration.cs
│   │   │   │   ├── {Entidade2}Configuration.cs
│   │   │   │   ├── UsuarioConfiguration.cs
│   │   │   │   ├── PerfilConfiguration.cs
│   │   │   │   └── UsuarioPerfilConfiguration.cs
│   │   │   ├── Repositories/
│   │   │   │   ├── {Entidade1}/
│   │   │   │   │   └── {Entidade1}Repository.cs
│   │   │   │   ├── {Entidade2}/
│   │   │   │   │   └── {Entidade2}Repository.cs
│   │   │   │   └── Usuarios/
│   │   │   │       └── UsuarioRepository.cs
│   │   │   └── {Projeto}Context.cs
│   │   └── {Projeto}.Infra.Data.csproj
│   │
│   ├── {Projeto}.IoC/
│   │   ├── Extensions/
│   │   │   ├── AuthenticationExtensions.cs
│   │   │   ├── DatabaseExtensions.cs
│   │   │   └── SwaggerExtensions.cs
│   │   ├── ServiceRegistrationExtensions.cs
│   │   └── {Projeto}.IoC.csproj
│   │
│   ├── {Projeto}Api/
│   │   ├── Controllers/
│   │   │   └── v1/
│   │   │       ├── AuthController.cs
│   │   │       ├── {Entidade1}Controller.cs
│   │   │       └── {Entidade2}Controller.cs
│   │   ├── Middlewares/
│   │   │   └── ExceptionHandlingMiddleware.cs
│   │   ├── Properties/
│   │   │   └── launchSettings.json
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.Example.json
│   │   ├── Program.cs
│   │   └── {Projeto}Api.csproj
│   │
│   └── {Projeto}.sln
│
├── tests/
│   ├── {Projeto}.TestesUnitarios/
│   │   ├── Application/
│   │   │   ├── Services/
│   │   │   │   ├── Auth/
│   │   │   │   │   ├── JwtServiceTests.cs
│   │   │   │   │   └── PasswordServiceTests.cs
│   │   │   │   ├── Email/
│   │   │   │   │   └── EmailServiceTests.cs
│   │   │   │   └── Validation/
│   │   │   │       └── CpfValidationServiceTests.cs
│   │   │   └── UseCases/
│   │   │       ├── Auth/
│   │   │       │   ├── LoginServiceTests.cs
│   │   │       │   └── RegistrarServiceTests.cs
│   │   │       ├── {Entidade1}/
│   │   │       │   ├── Cadastrar{Entidade1}ServiceTests.cs
│   │   │       │   ├── Alterar{Entidade1}ServiceTests.cs
│   │   │       │   └── Listar{Entidade1}ServiceTests.cs
│   │   │       └── {Entidade2}/
│   │   ├── Domain/
│   │   │   ├── {Entidade1}Tests.cs
│   │   │   ├── {Entidade2}Tests.cs
│   │   │   └── UsuarioTests.cs
│   │   ├── coverage/
│   │   └── {Projeto}.TestesUnitarios.csproj
│   │
│   ├── {Projeto}.TestesIntegrados/
│   │   ├── Fixtures/
│   │   │   └── {Projeto}ApiFactory.cs
│   │   ├── Tests/
│   │   │   ├── Auth/
│   │   │   │   ├── LoginTests.cs
│   │   │   │   └── RegistrarTests.cs
│   │   │   ├── {Entidade1}/
│   │   │   │   └── {Entidade1}IntegrationTests.cs
│   │   │   └── {Entidade2}/
│   │   ├── docker-compose.yml
│   │   └── {Projeto}.TestesIntegrados.csproj
│   │
│   └── {Projeto}.TestesCarga.K6/
│       ├── scripts/
│       │   ├── auth-test.js
│       │   ├── {entidade1}-test.js
│       │   └── {entidade2}-test.js
│       └── README.md
│
├── database/
│   └── scripts/
│       ├── 01_TabelaClientes.sql
│       ├── 02_Tabela{Entidade1}.sql
│       ├── 03_Tabela{Entidade2}.sql
│       ├── 04_TabelaUsuarios.sql
│       ├── 05_TabelaPerfis.sql
│       ├── 06_TabelaUsuarioPerfis.sql
│       ├── 07_TabelaLoginHistorico.sql
│       ├── 08_TabelaRefreshTokens.sql
│       └── 99_AdicionarGrants.sql
│
├── deploy/
│   ├── Dockerfile
│   └── DEPLOY_CONFIG.md
│
├── .github/
│   └── workflows/
│       └── deploy.yml
│
├── .gitignore
├── README.md
├── CONTEXT.md
├── SECRETS-GUIDE.md
└── USER-SECRETS-LOCAL.md
```

## 📋 Convenções de Nomenclatura

### Projetos
- **Domain**: `{Projeto}.Domain`
- **Application**: `{Projeto}.Application`
- **Infra.Data**: `{Projeto}.Infra.Data`
- **IoC**: `{Projeto}.IoC`
- **API**: `{Projeto}Api`

### Pastas por Domínio
Cada entidade de domínio tem sua própria pasta com subpastas:
```
{Entidade}/
├── Entities/      # Classes de entidade
├── Enums/         # Enumerações relacionadas
└── Repositories/  # Interfaces de repositório
```

### Use Cases
Estrutura por versão e ação:
```
UseCases/
└── {Entidade}/
    └── v1/
        ├── Cadastrar{Entidade}/
        ├── Alterar{Entidade}/
        ├── Excluir{Entidade}/
        └── Listar{Entidade}/
```

### Arquivos de Use Case
Cada pasta de use case contém:
- `I{Acao}{Entidade}Service.cs` - Interface
- `{Acao}{Entidade}Service.cs` - Implementação
- `{Acao}{Entidade}Command.cs` - Entrada (POST/PUT)
- `{Acao}{Entidade}Query.cs` - Entrada (GET)
- `{Acao}{Entidade}Response.cs` - Saída (opcional)

## 🎯 Separação por Domínio

### Domain Layer (Entidades de Negócio)
```
Domain/
├── _Shared/              # Compartilhado entre domínios
│   ├── Entities/         # Interfaces base
│   └── Exceptions/       # Exceções customizadas
├── Clientes/             # Domínio: Cliente
│   ├── Entities/
│   │   └── Cliente.cs
│   └── Repositories/
│       └── IClienteRepository.cs
├── Produtos/             # Domínio: Produto
│   ├── Entities/
│   │   └── Produto.cs
│   ├── Enums/
│   │   └── ProdutoStatus.cs
│   └── Repositories/
│       └── IProdutoRepository.cs
└── Reservas/             # Domínio: Reserva
    ├── Entities/
    │   ├── Reserva.cs
    │   └── ReservaProduto.cs
    ├── Enums/
    │   └── ReservaStatus.cs
    └── Repositories/
        └── IReservaRepository.cs
```

### Application Layer (Casos de Uso)
```
Application/
├── DTOs/                 # Data Transfer Objects
│   ├── Clientes/
│   │   └── ClienteDto.cs
│   ├── Produtos/
│   │   └── ProdutoDto.cs
│   └── Reservas/
│       └── ReservaDto.cs
├── Services/             # Serviços compartilhados
│   ├── Auth/
│   ├── Email/
│   └── Validation/
└── UseCases/             # Casos de uso por domínio
    ├── Auth/
    │   └── v1/
    │       ├── Login/
    │       └── Registrar/
    ├── Clientes/
    │   └── v1/
    │       ├── CadastrarCliente/
    │       ├── AlterarCliente/
    │       ├── ExcluirCliente/
    │       └── ListarCliente/
    ├── Produtos/
    │   └── v1/
    │       ├── CadastrarProduto/
    │       ├── AlterarProduto/
    │       ├── ExcluirProduto/
    │       └── ListarProduto/
    └── Reservas/
        └── v1/
            ├── CadastrarReserva/
            ├── AlterarReserva/
            ├── CancelarReserva/
            ├── ConfirmarReserva/
            └── ListarReserva/
```

### Infra.Data Layer (Persistência)
```
Infra.Data/
├── _Shared/
│   └── Postgres/
│       ├── Repositories/
│       │   └── Repository.cs      # Repositório genérico
│       └── UoW/
│           └── UnitOfWork.cs
└── Context/
    ├── Configuration/              # EF Core Configurations
    │   ├── ClienteConfiguration.cs
    │   ├── ProdutoConfiguration.cs
    │   └── ReservaConfiguration.cs
    ├── Repositories/               # Implementações por domínio
    │   ├── Clientes/
    │   │   └── ClienteRepository.cs
    │   ├── Produtos/
    │   │   └── ProdutoRepository.cs
    │   └── Reservas/
    │       └── ReservaRepository.cs
    └── {Projeto}Context.cs
```

### API Layer (Controllers)
```
{Projeto}Api/
├── Controllers/
│   └── v1/
│       ├── AuthController.cs
│       ├── ClientesController.cs
│       ├── ProdutosController.cs
│       └── ReservasController.cs
└── Middlewares/
    └── ExceptionHandlingMiddleware.cs
```

## 🔧 Exemplo Prático: Adicionar Nova Entidade

### Passo 1: Domain
```csharp
// Domain/Categorias/Entities/Categoria.cs
public class Categoria
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }
}

// Domain/Categorias/Repositories/ICategoriaRepository.cs
public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<Categoria?> GetByNomeAsync(string nome);
}
```

### Passo 2: Application - DTO
```csharp
// Application/DTOs/Categorias/CategoriaDto.cs
public class CategoriaDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
```

### Passo 3: Application - Use Case
```csharp
// Application/UseCases/Categorias/v1/CadastrarCategoria/CadastrarCategoriaCommand.cs
public class CadastrarCategoriaCommand
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
}

// Application/UseCases/Categorias/v1/CadastrarCategoria/ICadastrarCategoriaService.cs
public interface ICadastrarCategoriaService
{
    Task Handle(CadastrarCategoriaCommand command);
}

// Application/UseCases/Categorias/v1/CadastrarCategoria/CadastrarCategoriaService.cs
public class CadastrarCategoriaService : ICadastrarCategoriaService
{
    private readonly ICategoriaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarCategoriaService(
        ICategoriaRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CadastrarCategoriaCommand command)
    {
        var categoria = new Categoria
        {
            Nome = command.Nome,
            Descricao = command.Descricao,
            DataInclusao = DateTime.Now,
            DataAlteracao = DateTime.Now
        };

        await _repository.InsertAsync(categoria);
        await _unitOfWork.CommitAsync();
    }
}
```

### Passo 4: Infra.Data - Configuration
```csharp
// Infra.Data/Context/Configuration/CategoriaConfiguration.cs
public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("categorias", "public");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Descricao).HasMaxLength(500);
    }
}
```

### Passo 5: Infra.Data - Repository
```csharp
// Infra.Data/Context/Repositories/Categorias/CategoriaRepository.cs
public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    private readonly {Projeto}Context _context;

    public CategoriaRepository({Projeto}Context context) : base(context)
    {
        _context = context;
    }

    public async Task<Categoria?> GetByNomeAsync(string nome)
    {
        return await _context.Categorias
            .FirstOrDefaultAsync(c => c.Nome == nome);
    }
}
```

### Passo 6: API - Controller
```csharp
// {Projeto}Api/Controllers/v1/CategoriasController.cs
[ApiController]
[Authorize]
[Route("api/v1/categorias")]
[ApiExplorerSettings(GroupName = "v1")]
public class CategoriasController : Controller
{
    private readonly ICadastrarCategoriaService _cadastrarService;

    public CategoriasController(ICadastrarCategoriaService cadastrarService)
    {
        _cadastrarService = cadastrarService;
    }

    [HttpPost]
    public async Task<ActionResult> Cadastrar([FromBody] CadastrarCategoriaCommand command)
    {
        await _cadastrarService.Handle(command);
        return Ok();
    }
}
```

### Passo 7: IoC - Registro
```csharp
// IoC/ServiceRegistrationExtensions.cs
services.AddScoped<ICategoriaRepository, CategoriaRepository>();
services.AddScoped<ICadastrarCategoriaService, CadastrarCategoriaService>();
```

## ✅ Checklist para Nova Entidade

- [ ] Criar entidade em `Domain/{Entidade}/Entities/`
- [ ] Criar interface de repositório em `Domain/{Entidade}/Repositories/`
- [ ] Criar enums (se necessário) em `Domain/{Entidade}/Enums/`
- [ ] Criar DTO em `Application/DTOs/{Entidade}/`
- [ ] Criar use cases em `Application/UseCases/{Entidade}/v1/`
- [ ] Criar configuration em `Infra.Data/Context/Configuration/`
- [ ] Criar repository em `Infra.Data/Context/Repositories/{Entidade}/`
- [ ] Adicionar DbSet no Context
- [ ] Criar controller em `{Projeto}Api/Controllers/v1/`
- [ ] Registrar dependências no IoC
- [ ] Criar script SQL em `database/scripts/`
- [ ] Criar testes unitários
- [ ] Criar testes integrados
- [ ] Atualizar documentação

## 🎨 Padrões de Código

### Entidades (Domain)
- Usar métodos estáticos `Create()` para criação
- Métodos de negócio na própria entidade
- Propriedades com `protected set`

### Use Cases (Application)
- Um use case = uma responsabilidade
- Validações no service
- Usar Command para entrada, Response para saída

### Repositórios (Infra.Data)
- Herdar de `Repository<T>`
- Implementar apenas métodos específicos
- Queries complexas no repositório

### Controllers (API)
- Apenas orquestração
- Não ter lógica de negócio
- Retornar ActionResult apropriado

Este padrão garante:
✅ Separação clara de responsabilidades
✅ Fácil manutenção e evolução
✅ Testabilidade
✅ Escalabilidade
✅ Organização por domínio
