# Contexto do Projeto - Colinho da Cá API

## Visão Geral
Sistema de gerenciamento para pet shop com funcionalidades de cadastro de clientes, pets, reservas e autenticação.

## Arquitetura
- **Clean Architecture** com separação em camadas:
  - Domain: Entidades e interfaces de repositórios
  - Application: Use Cases, DTOs e Services
  - Infra.Data: Implementação de repositórios e contexto EF Core
  - API: Controllers e configurações
  - IoC: Injeção de dependências

## Tecnologias
- .NET 8
- Entity Framework Core
- PostgreSQL (Render)
- JWT Authentication
- SHA256 para hash de senhas

## Estrutura de Dados

### Clientes
- Id, Nome, Email, Celular, Cpf, Observacoes
- DataInclusao, DataAlteracao

### Pets
- Id, Nome, Raca, Idade, Peso (double), Porte (P/M/G), Observacoes
- ClienteId (FK para Clientes)
- DataInclusao, DataAlteracao

### Reservas
- Id, ClienteId, DataInicial, DataFinal, Observacoes
- DataInclusao, DataAlteracao
- Relacionamento N-N com Pets através de ReservaPets

### Usuarios
- Id, SenhaHash, ClienteId (FK único), Ativo (bool)
- DataInclusao, DataAlteracao
- Relacionamento N-N com Perfis através de UsuarioPerfis

### Perfis
- Id, Nome, Descricao
- Perfis padrão: Administrador (Id=1), Cliente (Id=2)

## Endpoints Principais

### Auth
- POST /api/v1/auth/registrar - Cria Cliente e Usuario com perfil Cliente
- POST /api/v1/auth/login - Retorna JWT e dados do usuário com perfis

### Clientes
- GET /api/v1/clientes - Lista com paginação e filtro por Id
- POST /api/v1/clientes - Cadastra cliente
- PUT /api/v1/clientes/{id} - Atualiza cliente
- DELETE /api/v1/clientes/{id} - Remove cliente

### Pets
- GET /api/v1/pets - Lista com paginação e filtro por ClienteId
- POST /api/v1/pets - Cadastra pet
- PUT /api/v1/pets/{id} - Atualiza pet
- DELETE /api/v1/pets/{id} - Remove pet

### Reservas
- GET /api/v1/reservas - Lista com paginação
- POST /api/v1/reservas - Cadastra reserva com lista de pets
- PUT /api/v1/reservas/{id} - Atualiza reserva
- DELETE /api/v1/reservas/{id} - Remove reserva

### Sobre
- POST /api/v1/sobre/enviar-email - Envia email de contato

## Serviços Reutilizáveis

### EmailService
- EnviarEmailAsync(assunto, corpo)
- Configurado via appsettings (SMTP Brevo)

### PasswordService
- HashPassword(password) - SHA256
- VerifyPassword(password, hash)

### JwtService
- GenerateToken(email, userId)
- Expiração: 24 horas

### CpfValidationService
- IsValid(cpf) - Valida formato e dígitos verificadores

## Regras de Negócio

### Registro
1. Valida CPF
2. Verifica se CPF já existe
3. Cria Cliente
4. Cria Usuario vinculado ao Cliente
5. Adiciona perfil "Cliente" (Id=2)
6. Usuario criado com Ativo=true

### Login
1. Busca Cliente por email
2. Busca Usuario por ClienteId
3. Valida se Usuario está ativo
4. Valida senha
5. Retorna JWT com dados do Cliente e lista de Perfis

### Relacionamentos
- Usuario 1:1 Cliente (ClienteId único)
- Cliente 1:N Pets
- Reserva N:N Pets (ReservaPets)
- Usuario N:N Perfis (UsuarioPerfis)

## Configurações (appsettings.json)

### ConnectionStrings
- ColinhoDaCaRender: PostgreSQL no Render

### JWT
- Secret: chave de 32+ caracteres
- ExpirationHours: 24

### Email
- SmtpHost: smtp-relay.brevo.com
- SmtpPort: 587
- SmtpUser, SmtpPassword
- EmailDestino, RemetenteEmail, RemetenteNome

## CORS
- Permitido: localhost:8080 e colinho-da-ca-site.vercel.app

## Scripts SQL (database/scripts/)
1. 01_TabelaClientes.sql
2. 02_TabelaPets.sql
3. 03_TabelaReservas.sql
4. 05_TabelaUsuarios.sql
5. 06_RemoverColunaEndereco.sql
6. 07_AdicionarClienteIdUsuarios.sql
7. 08_RemoverNomeEmailUsuarios.sql
8. 09_AdicionarAtivoEPerfis.sql

## Padrões de Código
- Commands para entrada de dados
- DTOs para saída
- Services com interface
- Repository pattern
- Unit of Work
- Validações antes de persistir
- Logs de erro
- Exceptions com mensagens claras
