## 📝 **IMPORTANTE - Manutenção da Documentação**

**SEMPRE que houver qualquer mudança no projeto:**
1. ✅ **Atualizar CONTEXT.md** - Documentar novas funcionalidades, endpoints, tabelas
2. ✅ **Atualizar Postman Collection** - Adicionar/modificar requests, incluir scripts automáticos
3. ✅ **Manter sincronizado** - Documentação deve refletir exatamente o estado atual da API

---

# Contexto do Projeto - Colinho da Cá API

## 📋 Visão Geral
Sistema completo de gerenciamento para pet shop com funcionalidades de cadastro de clientes, pets, raças, reservas, autenticação JWT, fluxo de status de reservas e envio de emails.

## 🏗️ Arquitetura
**Clean Architecture** com separação em camadas:
- **Domain**: Entidades, enums e interfaces de repositórios
- **Application**: Use Cases, DTOs, Services e validações
- **Infra.Data**: Implementação de repositórios, contexto EF Core e configurações
- **API**: Controllers, middlewares e configurações
- **IoC**: Injeção de dependências

## 🛠️ Tecnologias
- .NET 8
- Entity Framework Core
- PostgreSQL (Render)
- JWT Authentication (Bearer Token)
- SHA256 para hash de senhas
- Testcontainers (testes integrados)
- K6 (testes de carga)
- xUnit + FluentAssertions + Bogus

## 📊 Estrutura de Dados

### Clientes
- Id, Nome, Email, Celular, Cpf, Observacoes
- DataInclusao, DataAlteracao
- Validações: Email único, CPF único e válido

### Pets
- Id, Nome, **RacaId** (FK para Racas), Idade, Peso (double), Porte (P/M/G), Observacoes
- ClienteId (FK para Clientes)
- DataInclusao, DataAlteracao

### Racas
- Id, Nome, Porte (P/M/G/null)
- 36 raças pré-cadastradas (12 pequenas, 10 médias, 12 grandes, 2 SRD)

### Reservas
- Id, ClienteId, DataInicial, DataFinal, QuantidadeDiarias, QuantidadePets, ValorTotal, **ValorDesconto**, **ValorFinal**, Observacoes
- **CupomId** (FK nullable para Cupons)
- **Status** (enum): ReservaCriada, ReservaConfirmada, PagamentoPendente, PagamentoAprovado, ReservaFinalizada, ReservaCancelada
- ComprovantePagamento, DataPagamento, ObservacoesPagamento
- DataInclusao, DataAlteracao
- Relacionamento N:N com Pets através de ReservaPets

### Cupons
- Id, Codigo (único), Descricao, Tipo (enum), Percentual, ValorFixo, MinimoValorTotal, MinimoPets, MinimoDiarias
- DataInicio, DataFim (validação de período)
- Ativo (bool)
- DataInclusao, DataAlteracao
- **4 Tipos**: PercentualSobreTotal, PercentualPorPetComMinimo, PercentualPorPetComDiarias, ValorFixoComMinimo

### ReservaStatusHistorico
- Id, ReservaId, Status, UsuarioId, DataAlteracao
- Registra histórico completo de mudanças de status

### Usuarios
- Id, SenhaHash, ClienteId (FK único), Ativo (bool)
- DataInclusao, DataAlteracao
- Relacionamento N:N com Perfis através de UsuarioPerfis

### LoginHistorico
- Id, UsuarioId, Email, UserAgent, Platform, Language, ScreenResolution, Timezone, ClientIP, DataLogin
- Registra todas as informações de login com dados do dispositivo

### RefreshTokens
- Id, UsuarioId, Token, ExpiresAt, IsRevoked, CreatedAt, RevokedAt
- Controla tokens de refresh para OAuth2 (expiração 7 dias)

## 🔐 Autenticação OAuth2

### Access Token (JWT)
- **Expiração**: 30 minutos (era 24h)
- **Claims**: NameIdentifier, Email, Name, clienteId, celular, cpf, perfis (JSON)
- **Roles**: Incluídas baseadas nos perfis para autorização futura

### Refresh Token
- **Expiração**: 7 dias
- **Segurança**: Tokens únicos por usuário, revogação automática
- **Rotação**: Novo refresh token a cada uso

### Histórico de Login
- Grava todas as informações de dispositivo e IP
- Timestamp UTC de cada login
- Vinculado ao usuário para auditoria

### Endpoints Protegidos
- Todos os endpoints exceto `/auth/registrar`, `/auth/login` e `/racas` requerem `[Authorize]`
- Token extraído via `IHttpContextAccessor` para auditoria
- **Racas**: Endpoint público para facilitar uso em formulários

## 🌐 Endpoints Principais

### Auth (OAuth2)
- POST /api/v1/auth/registrar - Cria Cliente e Usuario com perfil Cliente
- POST /api/v1/auth/login - Retorna access_token + refresh_token + grava histórico
- POST /api/v1/auth/refresh - Renova tokens usando refresh_token

### Clientes
- GET /api/v1/clientes - Lista com paginação e filtro por Id
- POST /api/v1/clientes - Cadastra cliente (valida email/CPF duplicados)
- PUT /api/v1/clientes/{id} - Atualiza cliente (valida email/CPF de outros)
- DELETE /api/v1/clientes/{id} - Remove cliente

### Pets
- GET /api/v1/pets - Lista com paginação, filtro por ClienteId, retorna RacaId e RacaNome
- POST /api/v1/pets - Cadastra pet com RacaId
- PUT /api/v1/pets/{id} - Atualiza pet
- DELETE /api/v1/pets/{id} - Remove pet

### Racas
- GET /api/v1/racas - Lista todas as raças
- GET /api/v1/racas?racaId={id} - Busca raça específica

### Reservas
- GET /api/v1/reservas - Lista com paginação, retorna Status, StatusTimeline, Historico e **Pets com RacaNome**
- POST /api/v1/reservas - Cadastra reserva (Status=ReservaCriada, aceita ValorDesconto, ValorFinal, CupomId)
- PUT /api/v1/reservas/{id} - Atualiza reserva (apenas se Status=ReservaCriada, aceita todos os campos)
- DELETE /api/v1/reservas/{id} - Remove reserva
- **POST /api/v1/reservas/{id}/confirmar** - ADM confirma (1→2→3) + envia email
- **POST /api/v1/reservas/{id}/comprovante** - Cliente envia comprovante
- **POST /api/v1/reservas/{id}/aprovar-pagamento** - ADM aprova (3→4→5) + envia email
- **GET /api/v1/reservas/{id}/comprovante** - Visualiza comprovante
- **POST /api/v1/reservas/{id}/desconto** - Concede desconto manual (apenas Status=ReservaCriada)
- **POST /api/v1/reservas/{id}/cancelar** - Cancela reserva (Status=ReservaCancelada)
- **POST /api/v1/reservas/{id}/aplicar-cupom** - Valida cupom e retorna valores (tempo real, não persiste)

### Cupons
- GET /api/v1/cupons - Lista com paginação
- POST /api/v1/cupons - Cadastra cupom (valida código único)
- PUT /api/v1/cupons/{id} - Atualiza cupom
- POST /api/v1/cupons/{id}/inativar - Inativa cupom

### Sobre
- POST /api/v1/sobre/enviar-email - Envia email de contato

## 🔄 Fluxo de Status de Reservas

```
1. ReservaCriada (Cliente cria)
   ↓ ADM confirma
2. ReservaConfirmada
   ↓ Automático
3. PagamentoPendente (Email enviado ao cliente)
   ↓ Cliente envia comprovante
   ↓ ADM aprova
4. PagamentoAprovado
   ↓ Automático
5. ReservaFinalizada (Email de confirmação enviado)

6. ReservaCancelada (Cancelamento manual)
```

### Regras de Transição
- **Alterar Reserva**: Apenas Status=1 (ReservaCriada)
- **Confirmar**: Status=1 → 2 → 3 (ADM)
- **Enviar Comprovante**: Status=3 (Cliente)
- **Aprovar Pagamento**: Status=3 → 4 → 5 (ADM, requer comprovante)
- **Conceder Desconto**: Apenas Status=1 (ReservaCriada)
- **Cancelar**: Qualquer status → 6 (ReservaCancelada)

### StatusTimeline (Retorno da API)
```json
{
  "status": 3,
  "statusTimeline": {
    "1": true,
    "2": true,
    "3": true,
    "4": false,
    "5": false
  },
  "historico": [
    {
      "status": 1,
      "usuarioId": 5,
      "usuarioNome": "João Silva",
      "dataAlteracao": "2024-01-10T10:00:00"
    }
  ]
}
```

## 🛡️ Tratamento de Exceções

### Middleware Global (ExceptionHandlingMiddleware)
- Captura todas as exceções
- Retorna JSON padronizado: `{ "message": "..." }`
- Status HTTP apropriado

### Exceções Customizadas
- **ValidationException**: HTTP 400 (erros de validação)
- **EntityNotFoundException**: HTTP 404 (recurso não encontrado)
- **Exception**: HTTP 500 (erro interno)

## 📧 Serviços Reutilizáveis

### EmailService
- `EnviarEmailAsync(destinatario, assunto, corpo)`
- SMTP Brevo (smtp-relay.brevo.com:587)
- Usado em: Contato, Confirmação de Reserva, Aprovação de Pagamento

### PasswordService
- `HashPassword(password)` - SHA256
- `VerifyPassword(password, hash)`

### JwtService
- `GenerateToken(UsuarioResponse usuario)`
- Inclui todos os dados do usuário como claims

### CpfValidationService
- `IsValid(cpf)` - Valida formato e dígitos verificadores

## 📝 Regras de Negócio

### Registro
1. Valida CPF (formato e dígitos)
2. Verifica se CPF já existe
3. Cria Cliente
4. Cria Usuario vinculado ao Cliente
5. Adiciona perfil "Cliente" (Id=2)
6. Registra histórico de status inicial

### Login OAuth2
1. Busca Cliente por email
2. Busca Usuario por ClienteId com perfis
3. Valida se Usuario está ativo
4. Valida senha (SHA256)
5. **Revoga refresh tokens anteriores**
6. **Gera access token (30 min) + refresh token (7 dias)**
7. **Grava histórico de login com dados do dispositivo**
8. Retorna tokens no padrão OAuth2

### Refresh Token
1. Valida refresh token (não expirado, não revogado)
2. Busca usuário e valida se ativo
3. **Revoga refresh token atual**
4. **Gera novos access + refresh tokens**
5. Retorna novos tokens

### Cadastro de Cliente
1. Valida email único
2. Valida CPF único e válido
3. Persiste com DataInclusao/DataAlteracao

### Alteração de Cliente
1. Valida se cliente existe (404)
2. Valida email único (exceto próprio)
3. Valida CPF único (exceto próprio)
4. Atualiza DataAlteracao

### Cadastro de Reserva
1. Cria com Status=ReservaCriada
2. Registra histórico inicial (UsuarioId do token)
3. Vincula pets via ReservaPets

### Confirmação de Reserva (ADM)
1. Valida Status=ReservaCriada
2. Altera para ReservaConfirmada
3. Altera para PagamentoPendente
4. Registra 2 históricos (UsuarioId do token)
5. Envia email ao cliente

### Aprovação de Pagamento (ADM)
1. Valida Status=PagamentoPendente
2. Valida comprovante enviado
3. Altera para PagamentoAprovado
4. Altera para ReservaFinalizada
5. Registra 2 históricos (UsuarioId do token)
6. Envia email de confirmação

## 🗄️ Relacionamentos

- Usuario **1:1** Cliente (ClienteId único)
- Cliente **1:N** Pets
- Reserva **N:N** Pets (ReservaPets)
- Reserva **1:N** ReservaStatusHistorico
- Reserva **N:1** Cupom (CupomId nullable)
- Usuario **N:N** Perfis (UsuarioPerfis)
- Pet **N:1** Raca
- LoginHistorico **N:1** Usuario
- RefreshToken **N:1** Usuario

## ⚙️ Configurações (appsettings.json)

### ConnectionStrings
- ColinhoDaCaRender: PostgreSQL no Render

### JWT
- Secret: chave de 32+ caracteres
- ~~ExpirationHours: 24~~ (removido, agora fixo 30 min)

### Email
- SmtpHost: smtp-relay.brevo.com
- SmtpPort: 587
- SmtpUser, SmtpPassword
- RemetenteEmail, RemetenteNome

### CORS
- localhost:8080
- colinho-da-ca-site.vercel.app

## 🔒 Segurança

### User Secrets (Desenvolvimento)
```bash
dotnet user-secrets set "Jwt:Secret" "sua-chave-secreta"
dotnet user-secrets set "Email:SmtpUser" "seu-usuario"
```

### Variáveis de Ambiente (Produção - Render)
```
Jwt__Secret
Email__SmtpUser
Email__SmtpPassword
ConnectionStrings__ColinhoDaCaRender
```

## 🗃️ Scripts SQL (database/scripts/)

1. 01_TabelaClientes.sql
2. 02_TabelaPets.sql
3. 03_CamposDataInclusaoAlteracao.sql
4. 04_TabelaReservas.sql
5. 05_TabelaUsuarios.sql
6. 06_RemoverColunaEndereco.sql
7. 07_AdicionarClienteIdUsuarios.sql
8. 08_RemoverNomeEmailUsuarios.sql
9. 09_AdicionarAtivoEPerfis.sql
10. 10_AdicionarCamposReservas.sql
11. **11_TabelaRacas.sql** - Cria tabela e insere 36 raças
12. **12_AlterarPetsRacaId.sql** - Adiciona RacaId, remove Raca string
13. **13_AdicionarStatusReservas.sql** - Adiciona Status e campos de pagamento
14. **14_TabelaReservaStatusHistorico.sql** - Cria tabela de histórico
15. **15_AdicionarCamposDesconto.sql** - Adiciona ValorDesconto e ValorFinal
16. **16_TabelaCupons.sql** - Cria tabela Cupons, adiciona CupomId em Reservas, insere 4 cupons exemplo
17. **17_TabelaLoginHistorico.sql** - Cria tabela de histórico de login
18. **18_TabelaRefreshTokens.sql** - Cria tabela de refresh tokens OAuth2
19. **19_AdicionarGrantsTabelas.sql** - Adiciona GRANT permissions faltantes

## 🧪 Testes

### Testes Integrados (ColinhoDaCa.TestesIntegrados)
- **Tecnologias**: xUnit, Testcontainers, FluentAssertions, Bogus
- **Cobertura**: Auth, Clientes, Pets, Racas, Reservas, Fluxo Completo
- **Execução**: `dotnet test`
- **Banco**: PostgreSQL em container isolado
- **Incluído na Solution**: ✅ Sim

### Testes de Carga (ColinhoDaCa.TestesCarga.K6)
- **Tecnologias**: K6 (JavaScript)
- **Cenários**: Auth (10-50 VUs), Fluxo Completo (20 VUs), Stress (300 VUs)
- **Execução**: `k6 run scripts/auth-load-test.js`
- **Métricas**: p(95), p(99), taxa de erro, throughput
- **Incluído na Solution**: ❌ Não (projeto JavaScript)

### Metas de Performance
- p(95) < 500ms
- p(99) < 1000ms
- Taxa de erro < 1%
- Suportar 100+ usuários simultâneos

## 📁 Estrutura do Projeto

```
colinho-da-ca-api/
├── src/
│   ├── ColinhoDaCaApi/              # API e Controllers
│   ├── ColinhoDaCa.Application/     # Use Cases e Services
│   ├── ColinhoDaCa.Domain/          # Entidades e Interfaces
│   ├── ColinhoDaCa.Infra.Data/      # Repositórios e EF Core
│   └── ColinhoDaCa.IoC/             # Injeção de Dependências
├── tests/
│   ├── ColinhoDaCa.TestesIntegrados/    # xUnit + Testcontainers
│   └── ColinhoDaCa.TestesCarga.K6/      # K6 Load Tests
├── database/
│   └── scripts/                     # Scripts SQL numerados
├── CONTEXT.md                       # Este arquivo
├── SECRETS-GUIDE.md                 # Guia de segurança
├── USER-SECRETS-LOCAL.md            # Setup local
└── RENDER-SECRETS-SETUP.md          # Setup produção
```

## 🎯 Padrões de Código

### Commands e Queries
- Commands para entrada de dados (POST, PUT)
- Queries para filtros (GET)
- DTOs para saída

### Services
- Interface + Implementação
- Validações antes de persistir
- Logs de erro
- Try-catch com rethrow

### Repositories
- Repository pattern
- Unit of Work
- Métodos assíncronos

### Exceções
- Mensagens claras em português
- ValidationException para erros de negócio (400)
- EntityNotFoundException para recursos não encontrados (404)
- Exception genérica para erros internos (500)

### Auditoria
- DataInclusao e DataAlteracao em todas as entidades
- ReservaStatusHistorico registra quem e quando alterou status
- UsuarioId extraído do token JWT via IHttpContextAccessor

## 🚀 Deploy

### Desenvolvimento
```bash
dotnet run --project src/ColinhoDaCaApi
```

### Produção (Render)
- Build Command: `dotnet publish -c Release -o out`
- Start Command: `dotnet out/ColinhoDaCaApi.dll`
- Variáveis de ambiente configuradas no painel

## 📚 Documentação Adicional

- **SECRETS-GUIDE.md**: Proteção de dados sensíveis
- **USER-SECRETS-LOCAL.md**: Configuração local passo a passo
- **RENDER-SECRETS-SETUP.md**: Configuração no Render
- **tests/README.md**: Guia completo de testes
- **tests/ColinhoDaCa.TestesCarga.K6/README.md**: Guia K6 detalhado

## 🔄 Últimas Atualizações

- ✅ OAuth2 implementado com access token (30 min) e refresh token (7 dias)
- ✅ Histórico de login com informações de dispositivo e IP
- ✅ Roles preparadas para autorização baseada em perfis
- ✅ Rotação automática de refresh tokens
- ✅ Sistema de Raças com 36 raças pré-cadastradas
- ✅ Fluxo completo de status de reservas (6 estados incluindo Cancelada)
- ✅ Histórico de status com auditoria (quem e quando)
- ✅ Timeline de status no retorno da API
- ✅ Sistema de Cupons com 4 tipos de desconto
- ✅ Validação de cupons em tempo real (não persiste)
- ✅ Campos ValorDesconto, ValorFinal e CupomId em Reservas
- ✅ CRUD completo de Cupons com endpoint de inativação
- ✅ Validação de período de validade de cupons (DataInicio/DataFim)
- ✅ Pets retornam RacaNome na listagem de reservas
- ✅ Envio de emails em confirmação e aprovação
- ✅ JWT com todos os dados do usuário
- ✅ Validações de email e CPF duplicados
- ✅ Middleware global de exceções
- ✅ Testes integrados com Testcontainers
- ✅ Testes de carga com K6
- ✅ Documentação completa
