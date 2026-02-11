# ColinhoDaCa.TestesIntegrados

Projeto de testes integrados completo com Docker, PostgreSQL e cenários end-to-end.

## Estrutura

```
tests/ColinhoDaCa.TestesIntegrados/
├── Fixtures/
│   ├── IntegrationTestFactory.cs          # WebApplicationFactory customizada
│   └── PostgreSqlContainerFixture.cs      # Testcontainer PostgreSQL
├── Helpers/
│   ├── TestDataBuilder.cs                 # Bogus data generators
│   └── HttpClientExtensions.cs            # Extensões para HttpClient
├── Tests/
│   ├── AuthTests.cs                       # Testes de autenticação
│   ├── ClientesTests.cs                   # CRUD Clientes
│   ├── PetsTests.cs                       # CRUD Pets
│   ├── RacasTests.cs                      # Listar Raças
│   ├── ReservasTests.cs                   # CRUD Reservas
│   └── ReservasFluxoCompletoTests.cs      # Fluxo completo de reserva
├── docker-compose.yml                     # PostgreSQL + PgAdmin
└── appsettings.Testing.json               # Configurações de teste
```

## Pacotes Instalados

- **Microsoft.AspNetCore.Mvc.Testing**: WebApplicationFactory para testes de integração
- **Testcontainers.PostgreSql**: Container PostgreSQL isolado para cada teste
- **FluentAssertions**: Assertions fluentes e legíveis
- **Bogus**: Geração de dados fake realistas
- **xUnit**: Framework de testes

## Como Executar

### Pré-requisitos
- Docker Desktop instalado e rodando
- .NET 8 SDK

### Executar todos os testes
```bash
cd tests/ColinhoDaCa.TestesIntegrados
dotnet test
```

### Executar teste específico
```bash
dotnet test --filter "FullyQualifiedName~ReservasFluxoCompletoTests"
```

### Com Docker Compose (ambiente persistente)
```bash
docker-compose up -d
dotnet test
docker-compose down
```

## Cenários de Teste

### 1. Autenticação (AuthTests.cs)
- ✅ Registrar novo usuário
- ✅ Login com credenciais válidas
- ✅ Login com credenciais inválidas
- ✅ Token JWT contém claims corretas
- ✅ Endpoint protegido sem token retorna 401
- ✅ Endpoint protegido com token válido retorna 200

### 2. Clientes (ClientesTests.cs)
- ✅ Cadastrar cliente válido
- ✅ Cadastrar cliente com email duplicado retorna 400
- ✅ Cadastrar cliente com CPF duplicado retorna 400
- ✅ Listar clientes com paginação
- ✅ Alterar cliente existente
- ✅ Alterar cliente com email de outro retorna 400
- ✅ Excluir cliente existente
- ✅ Buscar cliente inexistente retorna 404

### 3. Pets (PetsTests.cs)
- ✅ Cadastrar pet com raça válida
- ✅ Cadastrar pet sem raça (SRD)
- ✅ Listar pets por cliente
- ✅ Alterar pet existente
- ✅ Excluir pet existente
- ✅ Validar relacionamento Pet-Cliente

### 4. Raças (RacasTests.cs)
- ✅ Listar todas as raças
- ✅ Buscar raça por ID
- ✅ Validar raças pequenas (P)
- ✅ Validar raças médias (M)
- ✅ Validar raças grandes (G)
- ✅ Validar raças SRD (porte null)

### 5. Reservas (ReservasTests.cs)
- ✅ Cadastrar reserva válida
- ✅ Listar reservas com paginação
- ✅ Alterar reserva em status "Criada"
- ✅ Tentar alterar reserva em outro status retorna 400
- ✅ Excluir reserva
- ✅ Validar cálculo de valor total

### 6. Fluxo Completo de Reserva (ReservasFluxoCompletoTests.cs)
- ✅ **Cenário Completo de Sucesso:**
  1. Registrar usuário cliente
  2. Login e obter token
  3. Cadastrar cliente
  4. Cadastrar 2 pets
  5. Criar reserva (Status: ReservaCriada)
  6. ADM confirma reserva (Status: PagamentoPendente)
  7. Cliente envia comprovante
  8. ADM visualiza comprovante
  9. ADM aprova pagamento (Status: ReservaFinalizada)
  10. Validar histórico de status
  11. Validar timeline de status
  12. Validar emails enviados

- ✅ **Cenário de Rejeição:**
  - Tentar confirmar reserva já confirmada
  - Tentar enviar comprovante sem estar em PagamentoPendente
  - Tentar aprovar sem comprovante

## Fixtures e Helpers

### IntegrationTestFactory
```csharp
// Configura WebApplicationFactory com:
// - PostgreSQL Testcontainer
// - Banco de dados isolado por teste
// - Migrations automáticas
// - Seed de dados iniciais (Perfis, Raças)
// - Mock de EmailService (captura emails)
```

### PostgreSqlContainerFixture
```csharp
// Gerencia ciclo de vida do container PostgreSQL
// - Inicia container antes dos testes
// - Cria banco de dados
// - Executa migrations
// - Limpa dados entre testes
// - Para container após testes
```

### TestDataBuilder
```csharp
// Usa Bogus para gerar dados realistas:
// - Clientes com CPF válido
// - Pets com raças válidas
// - Reservas com datas futuras
// - Usuários com senhas fortes
```

## Variáveis de Ambiente

```bash
# docker-compose.yml
POSTGRES_USER=testuser
POSTGRES_PASSWORD=testpass
POSTGRES_DB=colinho_test
```

## Boas Práticas Implementadas

1. **Isolamento**: Cada teste usa banco de dados limpo
2. **Idempotência**: Testes podem rodar em qualquer ordem
3. **Dados Realistas**: Bogus gera dados válidos
4. **Assertions Claras**: FluentAssertions para legibilidade
5. **Cleanup Automático**: Testcontainers gerencia lifecycle
6. **Mocks Mínimos**: Apenas EmailService mockado
7. **Testes E2E**: Fluxos completos simulando usuário real

## Próximos Passos

1. Adicionar testes de performance (stress test)
2. Implementar testes de concorrência
3. Adicionar cobertura de código
4. Integrar com CI/CD (GitHub Actions)
5. Adicionar testes de segurança (OWASP)
6. Implementar testes de carga (K6/JMeter)

## Troubleshooting

### Docker não inicia
```bash
# Verificar se Docker está rodando
docker ps

# Reiniciar Docker Desktop
```

### Porta já em uso
```bash
# Alterar porta no docker-compose.yml
ports:
  - "5433:5432"  # Usar 5433 ao invés de 5432
```

### Testes lentos
```bash
# Usar banco em memória para testes rápidos
# Testcontainers para testes completos
```

## Métricas

- **Cobertura de Código**: Objetivo 80%+
- **Tempo de Execução**: < 2 minutos para suite completa
- **Testes**: 50+ cenários
- **Endpoints Cobertos**: 100%
