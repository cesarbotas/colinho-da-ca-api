# Executar Testes Integrados

## Pré-requisitos
- Docker Desktop rodando
- .NET 8 SDK

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
```

## Acessar PgAdmin

http://localhost:5051
- Email: admin@test.com
- Senha: admin123

## Estrutura Criada

✅ Fixtures/IntegrationTestFactory.cs - WebApplicationFactory com Testcontainers
✅ Helpers/TestDataBuilder.cs - Gerador de dados com Bogus
✅ Helpers/HttpClientExtensions.cs - Extensões para HttpClient
✅ Tests/AuthTests.cs - Testes de autenticação
✅ Tests/RacasTests.cs - Testes de raças
✅ docker-compose.yml - PostgreSQL + PgAdmin
✅ appsettings.Testing.json - Configurações de teste

## Próximos Passos

Criar os testes restantes seguindo o padrão:
- Tests/ClientesTests.cs
- Tests/PetsTests.cs
- Tests/ReservasTests.cs
- Tests/ReservasFluxoCompletoTests.cs
