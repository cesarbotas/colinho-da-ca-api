# Executar Testes Integrados

## Pré-requisitos
- Docker Desktop rodando
- .NET 8 SDK

## Status Atual
✅ **Infraestrutura Configurada**: TestContainers + PostgreSQL funcionando
✅ **Scripts SQL**: Todos os scripts da pasta `database/scripts` são executados automaticamente
✅ **Scripts de Teste**: Scripts de dados de teste em `Scripts/DadosTeste.sql` executados automaticamente
✅ **Compilação**: Todos os erros de compilação foram corrigidos
✅ **Endpoints Básicos**: API responde corretamente (ex: `/api/v1/racas`)
✅ **Autenticação**: Todos os testes de auth funcionando
✅ **Dados de Teste**: Cliente e pet de teste inseridos automaticamente pelos scripts
⚠️ **Alguns Endpoints**: 4 testes ainda falhando (22/26 passando - 85% sucesso)

## Opção 1: Com Testcontainers (Recomendado)

Os testes criam e destroem containers automaticamente:

```bash
cd tests/ColinhoDaCa.TestesIntegrados
dotnet test
```

## Opção 2: Com Docker Compose

Usar banco persistente para debug:

```bash
# Iniciar banco
docker-compose up -d

# Executar testes
dotnet test

# Parar banco
docker-compose down
```

## Executar teste específico

```bash
dotnet test --filter "FullyQualifiedName~AuthTests"
dotnet test --filter "FullyQualifiedName~RacasTests"
dotnet test --filter "FullyQualifiedName~BasicIntegrationTest"
```

## Estrutura Criada

✅ **Fixtures/IntegrationTestFactory.cs** - WebApplicationFactory com Testcontainers
- Executa automaticamente todos os scripts SQL da pasta `database/scripts`
- Cria container PostgreSQL isolado para cada execução de teste
- Configura connection string para o banco de teste

✅ **Helpers/TestDataBuilder.cs** - Gerador de dados com Bogus
- Faker para comandos de registro
- Builders para entidades de teste

✅ **Helpers/HttpClientExtensions.cs** - Extensões para HttpClient
- Métodos de autenticação automática
- Helpers para requisições autenticadas
- Métodos para criação de dados de teste

✅ **Tests/** - Testes de integração
- BasicIntegrationTest.cs - Teste básico de infraestrutura
- AuthTests.cs - Testes de autenticação
- ClientesIntegrationTests.cs - Testes de clientes
- PetsIntegrationTests.cs - Testes de pets
- ReservasIntegrationTests.cs - Testes de reservas
- RacasTests.cs - Testes de raças

✅ **docker-compose.yml** - PostgreSQL + PgAdmin
✅ **appsettings.Testing.json** - Configurações de teste

## Correções Realizadas

### ✅ Problemas Resolvidos:
1. **Target Framework**: Corrigido de .NET 10.0 para .NET 8.0
2. **Stack Overflow**: Corrigido recursão infinita no `HttpClientExtensions.PostAsJsonAsync`
3. **Program Class**: Adicionado `public partial class Program { }` para acesso nos testes
4. **PostgreSQL Builder**: Corrigido construtor obsoleto
5. **Scripts SQL**: Implementada execução automática de todos os scripts da pasta `database/scripts`
6. **Banco de Dados**: Tabelas são criadas corretamente pelos scripts SQL
7. **Endpoints Básicos**: API responde corretamente para endpoints simples

### ⚠️ Problemas Pendentes:
1. **Autenticação**: Alguns endpoints de registro/login retornando erro 400/500
2. **Validação**: Possíveis problemas de validação nos DTOs de entrada
3. **Dados Iniciais**: Podem faltar dados específicos para alguns testes

## Resultados dos Testes
- **Total**: 26 testes
- **Passou**: 22 testes ✅ (85%)
- **Falhou**: 4 testes ❌ (15%)
- **Infraestrutura**: ✅ Funcionando
- **Endpoints Básicos**: ✅ Funcionando
- **Autenticação**: ✅ Funcionando
- **Dados de Teste**: ✅ Scripts SQL inserindo dados automaticamente

### ✅ Testes que Passam (22):
- **Autenticação (6/6)**: Registro, login, credenciais inválidas, CPF inválido/duplicado, refresh token
- **Endpoints Autenticados (4/4)**: Acesso sem/com token, criar cliente, refresh token
- **Raças (3/3)**: Listar todas, raças pequenas, raças SRD
- **Clientes (3/4)**: Cadastrar, listar, alterar
- **Pets (2/3)**: Cadastrar, listar
- **Reservas (2/4)**: Cadastrar, listar
- **Básicos (1/1)**: Endpoint de raças
- **Debug (1/1)**: Teste de debug

### ❌ Testes que Falham (4):
- **Clientes (1)**: Excluir cliente - 500 Internal Server Error
- **Pets (1)**: Excluir pet - 404 Not Found
- **Reservas (2)**: Confirmar/Cancelar reserva - 405 Method Not Allowed
