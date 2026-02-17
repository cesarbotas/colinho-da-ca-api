# Projetos de Testes - Colinho da Cá API

## 📁 Estrutura Completa

```
tests/
├── ColinhoDaCa.TestesUnitarios/           # Testes unitários (60%+ cobertura)
│   ├── Domain/                            # Entidades de domínio
│   ├── Services/                          # Serviços de aplicação
│   ├── UseCases/                          # Casos de uso
│   ├── Auth/                              # Autenticação
│   └── Controllers/                       # Controladores
├── ColinhoDaCa.TestesIntegrados/          # Testes integrados
└── ColinhoDaCa.TestesCarga.K6/            # Testes de carga
```

## 🧪 ColinhoDaCa.TestesUnitarios

**Cobertura de Testes:**
- ✅ **Domain**: Cliente, Pet, Usuario, Reserva entities
- ✅ **Services**: PasswordService, JwtService, EmailService
- ✅ **UseCases**: CRUD operations, business logic
- ✅ **Auth**: Login, Registration, Token refresh
- ✅ **Controllers**: API endpoints validation

**Tecnologias:**
- xUnit + FluentAssertions
- Moq (mocking)
- Coverlet (coverage)

**Executar:**
```bash
./run-unit-tests.sh
```

**Meta de Cobertura:** ≥60%

---

## 🔗 ColinhoDaCa.TestesIntegrados

**Cobertura Completa:**
- ✅ Auth (Login, Register, Refresh)
- ✅ Clientes (CRUD + validações)
- ✅ Pets (CRUD + regras de negócio)
- ✅ Reservas (Fluxo completo)
- ✅ Status codes corretos
- ✅ Validações de entrada
- ✅ Regras de negócio

**Executar:**
```bash
./run-tests.sh
```

---

## ⚡ ColinhoDaCa.TestesCarga.K6

**Cenários:**
- Load testing (usuários normais)
- Stress testing (picos de carga)
- Fluxo completo de reservas

---

## 🚀 Executar Todos os Testes

### Sequência Completa
```bash
# 1. Testes unitários com cobertura
./run-unit-tests.sh

# 2. Testes integrados + Docker
./run-tests.sh

# 3. Testes de carga (opcional)
cd tests/ColinhoDaCa.TestesCarga.K6
./run-all-tests.bat
```

---

## 📊 Métricas de Qualidade

### Testes Unitários
- ✅ Cobertura ≥ 60%
- ✅ Todos os domínios testados
- ✅ Mocks para dependências
- ✅ Execução < 30 segundos

### Testes Integrados
- ✅ Todas as rotas testadas
- ✅ Status codes validados
- ✅ Fluxos end-to-end
- ✅ Execução < 2 minutos

### Testes de Carga
- ✅ p(95) < 500ms
- ✅ Taxa de erro < 1%
- ✅ 100+ usuários simultâneos

---

## 🎯 Estrutura de Testes por Camada

### Domain Layer
```
Domain/
├── ClienteTests.cs          # Entidade Cliente
├── PetTests.cs              # Entidade Pet + regras
├── UsuarioTests.cs          # Entidade Usuario
└── ReservaTests.cs          # Entidade Reserva + status
```

### Application Layer
```
Services/
├── PasswordServiceTests.cs  # Hash de senhas
├── JwtServiceTests.cs       # Tokens JWT
└── EmailServiceTests.cs     # Envio de emails

UseCases/
├── CadastrarClienteServiceTests.cs
├── ExcluirPetServiceTests.cs
├── CadastrarReservaServiceTests.cs
└── LoginServiceTests.cs
```

### Infrastructure Layer
```
Repositories/
├── ClienteRepositoryTests.cs
├── PetRepositoryTests.cs
└── ReservaRepositoryTests.cs

Controllers/
├── AuthControllerTests.cs
├── ClientesControllerTests.cs
├── PetsControllerTests.cs
└── ReservasControllerTests.cs
```

---

## 📈 Relatórios

### Coverage Report
- **Local**: `tests/ColinhoDaCa.TestesUnitarios/TestResults/CoverageReport/index.html`
- **Métricas**: Line, Branch, Method coverage
- **Filtros**: Por namespace, classe, método

### Test Results
- **Console**: Resultados em tempo real
- **XML**: Compatível com CI/CD
- **HTML**: Relatório visual detalhado