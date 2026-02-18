# Prompt: API Clean Architecture Template

Crie uma API completa seguindo os padrões do projeto base, adaptando para o domínio especificado.

## 🏗️ Arquitetura Obrigatória
**Clean Architecture** com 5 projetos:
- **Domain**: Entidades, enums, interfaces de repositórios
- **Application**: Use Cases, DTOs, Services, validações
- **Infra.Data**: Repositórios, EF Core, configurações
- **API**: Controllers, middlewares
- **IoC**: Injeção de dependências

## 🛠️ Stack Tecnológica
- .NET 8
- Entity Framework Core
- PostgreSQL
- JWT Authentication (Bearer Token)
- SHA256 para senhas
- Middleware global de exceções

## 🔐 Sistema de Autenticação
### Estrutura Base:
- **Usuarios**: Id, SenhaHash, ClienteId (FK único), Ativo
- **Perfis**: Id, Nome, Descricao (Administrador=1, Cliente=2)
- **UsuarioPerfis**: Relacionamento N:N
- **LoginHistorico**: Rastreamento completo de logins

### JWT Token:
- Expiração: 24 horas
- Claims: NameIdentifier, Email, Name, clienteId, perfis (JSON)
- Todos os endpoints protegidos exceto `/auth/registrar` e `/auth/login`

### Endpoints Auth:
```
POST /api/v1/auth/registrar - Cria Cliente e Usuario
POST /api/v1/auth/login - Retorna JWT + grava histórico
```

## 📊 Padrões de Entidades
### Campos Obrigatórios:
```csharp
public long Id { get; set; }
public DateTime DataInclusao { get; set; }
public DateTime DataAlteracao { get; set; }
```

### Validações Padrão:
- Email único
- CPF único e válido (usar CpfValidationService)
- Campos obrigatórios

## 🌐 Padrões de API
### Controllers:
```csharp
[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
```

### Endpoints CRUD Padrão:
```
GET /api/v1/{entidade} - Lista com paginação
POST /api/v1/{entidade} - Cadastra
PUT /api/v1/{entidade}/{id} - Atualiza
DELETE /api/v1/{entidade}/{id} - Remove
```

### Paginação:
```csharp
public class PaginacaoDto
{
    public int NumeroPagina { get; set; } = 1;
    public int QuantidadeRegistros { get; set; } = 10;
}

public class ResultadoPaginadoDto<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public List<T> Data { get; set; }
}
```

## 🛡️ Tratamento de Exceções
### Middleware Global:
```csharp
public class ExceptionHandlingMiddleware
{
    // Captura todas as exceções
    // Retorna JSON: { "message": "..." }
    // Status HTTP apropriado
}
```

### Exceções Customizadas:
```csharp
public class ValidationException : BaseException // HTTP 400
public class EntityNotFoundException : BaseException // HTTP 404
```

## 📧 Serviços Reutilizáveis
### Implementar sempre:
```csharp
public interface IEmailService
{
    Task EnviarEmailAsync(string destinatario, string assunto, string corpo);
}

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public interface IJwtService
{
    string GenerateToken(UsuarioResponse usuario);
}

public interface ICpfValidationService
{
    bool IsValid(string cpf);
}
```

## 🗄️ Configurações EF Core
### Context Base:
```csharp
public class {Projeto}Context : DbContext
{
    // DbSets das entidades
    // Configurações via IEntityTypeConfiguration
    // Schema "public"
}
```

### Repository Pattern:
```csharp
public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetAllAsync();
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
```

## ⚙️ Configurações (appsettings.json)
```json
{
  "ConnectionStrings": {
    "{Projeto}": "sua-connection-string"
  },
  "Jwt": {
    "Secret": "sua-chave-secreta-32-caracteres",
    "ExpirationHours": 24
  },
  "Email": {
    "SmtpHost": "smtp-relay.brevo.com",
    "SmtpPort": 587,
    "SmtpUser": "seu-usuario",
    "SmtpPassword": "sua-senha",
    "RemetenteEmail": "seu-email",
    "RemetenteNome": "Seu Nome"
  }
}
```

## 📁 Estrutura de Pastas
```
{projeto}-api/
├── src/
│   ├── {Projeto}Api/
│   ├── {Projeto}.Application/
│   │   ├── DTOs/
│   │   ├── UseCases/
│   │   ├── Services/
│   │   └── Repositories/
│   ├── {Projeto}.Domain/
│   │   ├── {Entidade}/
│   │   │   ├── Entities/
│   │   │   ├── Enums/
│   │   │   └── Repositories/
│   ├── {Projeto}.Infra.Data/
│   │   └── Context/
│   │       ├── Configuration/
│   │       └── Repositories/
│   └── {Projeto}.IoC/
├── database/
│   └── scripts/
└── README.md
```

## 🎯 Use Cases Padrão
### Estrutura:
```csharp
// Command/Query
public class {Acao}{Entidade}Command
{
    // Propriedades de entrada
}

// Service Interface
public interface I{Acao}{Entidade}Service
{
    Task<TResponse> Handle({Acao}{Entidade}Command command);
}

// Service Implementation
public class {Acao}{Entidade}Service : I{Acao}{Entidade}Service
{
    // Injeção de dependências
    // Validações
    // Lógica de negócio
    // Persistência
}
```

## 🔄 Fluxo de Desenvolvimento
1. **Definir Entidades** (Domain)
2. **Criar Migrations** (Scripts SQL)
3. **Implementar Repositórios** (Infra.Data)
4. **Criar DTOs** (Application)
5. **Implementar Use Cases** (Application)
6. **Criar Controllers** (API)
7. **Configurar IoC** (IoC)
8. **Testar Endpoints**

## 📝 Exemplo de Adaptação
Para uma **API de Roupas de Noivos e Noivas**:

### Entidades Principais:
- **Clientes** (base do template)
- **Categorias** (Vestidos, Ternos, Acessórios)
- **Produtos** (Nome, CategoriaId, Tamanho, Cor, Preco)
- **Reservas** (ClienteId, DataEvento, DataRetirada, DataDevolucao)
- **ReservaProdutos** (N:N)

### Endpoints Específicos:
```
GET /api/v1/produtos?categoriaId=1&tamanho=M
POST /api/v1/reservas/{id}/confirmar
POST /api/v1/reservas/{id}/retirar
POST /api/v1/reservas/{id}/devolver
```

### Regras de Negócio:
- Validar disponibilidade por data
- Calcular multas por atraso
- Controlar status: Reservada → Retirada → Devolvida

## ✅ Checklist de Implementação
- [x] Estrutura de projetos Clean Architecture
- [x] Sistema de autenticação JWT completo
- [x] Middleware de exceções global
- [x] Paginação em listagens
- [x] Validações de negócio
- [x] Auditoria (DataInclusao/DataAlteracao)
- [x] Repositórios com Unit of Work
- [x] Configurações via appsettings
- [x] Documentação README.md
- [x] Scripts SQL organizados
- [x] **Testes unitários (48% cobertura)**
- [x] **Testes integrados (Testcontainers)**
- [x] **Testes de carga (K6)**
- [x] **CI/CD com cobertura mínima**

## 🚀 Comandos Iniciais
```bash
# Criar solution
dotnet new sln -n {Projeto}

# Criar projetos
dotnet new webapi -n {Projeto}Api
dotnet new classlib -n {Projeto}.Application
dotnet new classlib -n {Projeto}.Domain
dotnet new classlib -n {Projeto}.Infra.Data
dotnet new classlib -n {Projeto}.IoC

# Adicionar à solution
dotnet sln add src/{Projeto}Api
dotnet sln add src/{Projeto}.Application
dotnet sln add src/{Projeto}.Domain
dotnet sln add src/{Projeto}.Infra.Data
dotnet sln add src/{Projeto}.IoC

# Instalar pacotes principais
dotnet add src/{Projeto}Api package Microsoft.EntityFrameworkCore
dotnet add src/{Projeto}Api package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/{Projeto}Api package Microsoft.AspNetCore.Authentication.JwtBearer
```

Adapte este template para qualquer domínio mantendo a arquitetura, padrões e qualidade do projeto base.