# Projetos de Testes - Colinho da Cá API

## 📁 Estrutura

```
tests/
├── ColinhoDaCa.TestesIntegrados/          # Testes integrados com xUnit + Testcontainers
└── ColinhoDaCa.TestesCarga.K6/            # Testes de carga com K6
```

## 🧪 ColinhoDaCa.TestesIntegrados

**Tecnologias:**
- xUnit
- Testcontainers (PostgreSQL)
- FluentAssertions
- Bogus
- Microsoft.AspNetCore.Mvc.Testing

**Executar:**
```bash
cd ColinhoDaCa.TestesIntegrados
dotnet test
```

**Incluído na Solution:** ✅ Sim (`ColinhoDaCa.sln`)

**Documentação:** [EXECUTAR.md](ColinhoDaCa.TestesIntegrados/EXECUTAR.md)

---

## ⚡ ColinhoDaCa.TestesCarga.K6

**Tecnologias:**
- K6 (JavaScript)
- Docker (opcional)

**Executar:**
```bash
cd ColinhoDaCa.TestesCarga.K6
k6 run scripts/auth-load-test.js
```

**Incluído na Solution:** ❌ Não (projeto JavaScript, não .NET)

**Documentação:** [README.md](ColinhoDaCa.TestesCarga.K6/README.md)

---

## 🚀 Executar Todos os Testes

### Testes Integrados
```bash
dotnet test tests/ColinhoDaCa.TestesIntegrados/ColinhoDaCa.TestesIntegrados.csproj
```

### Testes de Carga
```bash
cd tests/ColinhoDaCa.TestesCarga.K6
run-all-tests.bat
```

---

## 📊 Comparação

| Aspecto | Testes Integrados | Testes de Carga |
|---------|-------------------|-----------------|
| **Objetivo** | Validar funcionalidade | Validar performance |
| **Tecnologia** | .NET/xUnit | K6/JavaScript |
| **Banco de Dados** | Testcontainers | API real |
| **Duração** | ~2 minutos | ~5-20 minutos |
| **Quando Executar** | A cada commit | Antes de deploy |
| **CI/CD** | Sim | Opcional |

---

## 🎯 Metas de Qualidade

### Testes Integrados
- ✅ Cobertura > 80%
- ✅ Todos os endpoints testados
- ✅ Fluxos completos validados
- ✅ Execução < 2 minutos

### Testes de Carga
- ✅ p(95) < 500ms
- ✅ Taxa de erro < 1%
- ✅ Suportar 100+ usuários simultâneos
- ✅ Throughput > 100 req/s
