# Prompt: Estrutura Completa de Testes

Crie uma estrutura completa de testes para a API seguindo os padrões do projeto base.

## 📁 Estrutura de Projetos de Testes

```
tests/
├── {Projeto}.TestesUnitarios/           # Testes unitários
│   ├── Application/
│   │   ├── UseCases/
│   │   └── Services/
│   ├── Domain/
│   ├── Infra.Data/
│   ├── Api/
│   ├── Repositories/
│   ├── coverage/
│   └── {Projeto}.TestesUnitarios.csproj
├── {Projeto}.TestesIntegrados/          # Testes integrados
│   ├── Fixtures/
│   │   ├── IntegrationTestFactory.cs
│   │   └── PostgreSqlContainerFixture.cs
│   ├── Helpers/
│   │   ├── TestDataBuilder.cs
│   │   └── HttpClientExtensions.cs
│   ├── Models/
│   │   └── AuthModels.cs
│   ├── Tests/
│   │   ├── AuthTests.cs
│   │   ├── {Entidade}IntegrationTests.cs
│   │   └── {Entidade}FluxoCompletoTests.cs
│   ├── Scripts/
│   │   └── DadosTeste.sql
│   ├── docker-compose.yml
│   ├── docker-compose.debug.yml
│   ├── appsettings.Testing.json
│   ├── appsettings.Debug.json
│   ├── EXECUTAR.md
│   └── {Projeto}.TestesIntegrados.csproj
├── {Projeto}.TestesCarga.K6/            # Testes de carga
│   ├── scripts/
│   │   ├── auth-load-test.js
│   │   ├── {entidade}-flow-test.js
│   │   └── stress-test.js
│   ├── INFO.txt
│   ├── README.md
│   └── run-all-tests.bat
└── README.md
```

## 🧪 1. Testes Unitários

### Pacotes NuGet
```xml
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="FluentAssertions" Version="6.12.1" />
<PackageReference Include="coverlet.collector" Version="6.0.2" />
<PackageReference Include="coverlet.msbuild" Version="6.0.2" />
```

### Estrutura de Teste
```csharp
public class {Acao}{Entidade}ServiceTests
{
    private readonly Mock<I{Entidade}Repository> _repositoryMock;
    private readonly {Acao}{Entidade}Service _service;

    public {Acao}{Entidade}ServiceTests()
    {
        _repositoryMock = new Mock<I{Entidade}Repository>();
        _service = new {Acao}{Entidade}Service(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateEntity()
    {
        // Arrange
        var command = new {Acao}{Entidade}Command { /* ... */ };
        
        // Act
        await _service.Handle(command);
        
        // Assert
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<{Entidade}>()), Times.Once);
    }
}
```

### Cobertura de Código
- Meta mínima: 60%
- Comando: `dotnet test /p:CollectCoverage=true`
- Relatório HTML: `./run-unit-tests-coverage.sh`

## 🔗 2. Testes Integrados

### Pacotes NuGet
```xml
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.11" />
<PackageReference Include="Testcontainers.PostgreSql" Version="4.1.0" />
<PackageReference Include="FluentAssertions" Version="6.12.1" />
<PackageReference Include="Bogus" Version="35.6.1" />
<PackageReference Include="xunit" Version="2.9.2" />
```

### IntegrationTestFactory.cs
```csharp
public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    
    public IntegrationTestFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("test_db")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Substituir DbContext
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<{Projeto}Context>));
            if (descriptor != null) services.Remove(descriptor);
            
            services.AddDbContext<{Projeto}Context>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
            
            // Mock EmailService
            services.AddScoped<IEmailService, MockEmailService>();
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<{Projeto}Context>();
        await context.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
```

### TestDataBuilder.cs
```csharp
public static class TestDataBuilder
{
    public static Faker<{Entidade}Dto> {Entidade}Faker => new Faker<{Entidade}Dto>()
        .RuleFor(x => x.Nome, f => f.Name.FullName())
        .RuleFor(x => x.Email, f => f.Internet.Email())
        .RuleFor(x => x.Cpf, f => GerarCpfValido());
}
```

### Cenários de Teste
```csharp
public class {Entidade}IntegrationTests : IClassFixture<IntegrationTestFactory>
{
    [Fact]
    public async Task Post_ValidEntity_ShouldReturn201()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = await GetAuthToken(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var dto = TestDataBuilder.{Entidade}Faker.Generate();
        
        // Act
        var response = await client.PostAsJsonAsync("/api/v1/{entidade}", dto);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
```

## ⚡ 3. Testes de Carga (K6)

### auth-load-test.js
```javascript
import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '30s', target: 10 },
    { duration: '1m', target: 10 },
    { duration: '30s', target: 50 },
    { duration: '1m', target: 50 },
    { duration: '30s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<500'],
    http_req_failed: ['rate<0.05'],
  },
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5163';

export default function () {
  const registrarPayload = JSON.stringify({
    nome: `Usuario ${Date.now()}`,
    email: `user${Date.now()}@test.com`,
    cpf: gerarCpf(),
    senha: 'Senha@123',
  });

  const registrarRes = http.post(`${BASE_URL}/api/v1/auth/registrar`, registrarPayload, {
    headers: { 'Content-Type': 'application/json' },
  });

  check(registrarRes, {
    'registrar status 201': (r) => r.status === 201,
  });

  sleep(1);
}

function gerarCpf() {
  const n = () => Math.floor(Math.random() * 9);
  return `${n()}${n()}${n()}.${n()}${n()}${n()}.${n()}${n()}${n()}-${n()}${n()}`;
}
```

### Métricas K6
- p(95) < 500ms
- Taxa de erro < 5%
- 100+ usuários simultâneos

## 📊 4. Scripts de Banco de Dados

### Estrutura database/scripts/
```
database/
└── scripts/
    ├── 01_Tabela{Entidade1}.sql
    ├── 02_Tabela{Entidade2}.sql
    ├── 03_CamposDataInclusaoAlteracao.sql
    ├── 04_Tabela{Relacionamento}.sql
    └── XX_AdicionarGrants.sql
```

### Template de Script
```sql
-- XX_Tabela{Entidade}.sql
CREATE TABLE IF NOT EXISTS public.{entidade} (
    id BIGSERIAL PRIMARY KEY,
    nome VARCHAR(200) NOT NULL,
    data_inclusao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_alteracao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Índices
CREATE INDEX IF NOT EXISTS idx_{entidade}_nome ON public.{entidade}(nome);

-- Grants
GRANT SELECT, INSERT, UPDATE, DELETE ON public.{entidade} TO app_user;
GRANT USAGE, SELECT ON SEQUENCE public.{entidade}_id_seq TO app_user;
```

## 🚀 5. Deploy e CI/CD

### Jenkinsfile (Estrutura Base)
```groovy
pipeline {
    agent any
    
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        IMAGE_NAME = '{usuario}/{projeto}-api'
        GIT_COMMIT_SHORT = sh(script: "git rev-parse --short HEAD", returnStdout: true).trim()
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Restore') {
            steps {
                sh 'dotnet restore src/{Projeto}.sln'
            }
        }
        
        stage('Build') {
            steps {
                sh 'dotnet build src/{Projeto}.sln --no-restore --configuration Release'
            }
        }
        
        stage('Test Coverage') {
            steps {
                sh '''
                dotnet test tests/{Projeto}.TestesUnitarios/{Projeto}.TestesUnitarios.csproj \
                    --collect:"XPlat Code Coverage" \
                    --results-directory ./coverage
                '''
                
                script {
                    def coverageFile = sh(
                        script: 'find ./coverage -name "*.cobertura.xml" | head -1',
                        returnStdout: true
                    ).trim()
                    
                    def coverage = sh(
                        script: "grep -o 'line-rate=\"[0-9.]*\"' ${coverageFile} | grep -o '[0-9.]*'",
                        returnStdout: true
                    ).trim()
                    
                    def coveragePercent = (coverage as Double) * 100
                    echo "Cobertura: ${coveragePercent}%"
                    
                    if (coveragePercent < 20) {
                        error("Cobertura abaixo do mínimo (20%)")
                    }
                }
            }
        }
        
        stage('Docker Build') {
            steps {
                sh """
                docker build -t ${IMAGE_NAME}:${GIT_COMMIT_SHORT} -f deploy/Dockerfile .
                docker tag ${IMAGE_NAME}:${GIT_COMMIT_SHORT} ${IMAGE_NAME}:latest
                """
            }
        }
        
        stage('Docker Push') {
            when {
                branch 'main'
            }
            steps {
                withCredentials([usernamePassword(
                    credentialsId: 'dockerhub',
                    usernameVariable: 'DOCKER_USER',
                    passwordVariable: 'DOCKER_PASS'
                )]) {
                    sh """
                    echo \$DOCKER_PASS | docker login -u \$DOCKER_USER --password-stdin
                    docker push ${IMAGE_NAME}:${GIT_COMMIT_SHORT}
                    docker push ${IMAGE_NAME}:latest
                    """
                }
            }
        }
    }
}
```

### deploy/DEPLOY_CONFIG.md
```markdown
# Configuração de Deploy

## Requisitos de Qualidade

### Cobertura de Testes
- **Mínimo exigido**: 20%
- **Validação**: Executada automaticamente no pipeline
- **Falha**: Deploy bloqueado se cobertura < 20%

### Processo de Deploy
1. Testes unitários executados
2. Cobertura de código calculada
3. Validação do percentual mínimo
4. Build da imagem Docker (se aprovado)
5. Push para DockerHub (branch main)

### Configuração
- Ferramenta: XPlat Code Coverage
- Formato: Cobertura XML
- Projeto: {Projeto}.TestesUnitarios
```

### jenkins-ci/README.md
```markdown
# Jenkins CI/CD - {Projeto} API

## 🏗️ Arquitetura

```
┌─────────────────────┐
│      Jenkins        │
│  (Orquestrador CI)  │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  Docker Agent       │
│  (Build .NET 8)     │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  Docker Daemon      │
│  (Build imagens)    │
└──────────┬──────────┘
           ▼
      PostgreSQL
```

## 🚀 Iniciar Ambiente

```bash
./start-jenkins.sh
```

## 📋 Pipeline Stages

1. **Checkout** - Baixa código fonte
2. **Restore** - Restaura dependências .NET
3. **Build** - Compila aplicação
4. **Test Coverage** - Executa testes com cobertura
5. **Docker Build** - Cria imagem Docker
6. **Docker Push** - Envia para registry (branch main)

## 🐳 Serviços

- **Jenkins**: http://localhost:8090
- **PostgreSQL**: localhost:5432

## 📊 Métricas

- ✅ Build automatizado
- ✅ Testes unitários
- ✅ Cobertura de código (mínimo 20%)
- ✅ Imagem Docker otimizada
```

### jenkins-ci/docker-compose.yml
```yaml
version: '3.8'

services:
  jenkins:
    build:
      context: .
      dockerfile: Dockerfile.jenkins
    container_name: jenkins
    ports:
      - "8090:8080"
      - "50000:50000"
    volumes:
      - jenkins_home:/var/jenkins_home
      - /var/run/docker.sock:/var/run/docker.sock
    environment:
      - JAVA_OPTS=-Djenkins.install.runSetupWizard=false

  jenkins-agent:
    build:
      context: .
      dockerfile: Dockerfile.agent
    container_name: jenkins-agent
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock
    depends_on:
      - jenkins

  postgres:
    image: postgres:16-alpine
    container_name: postgres-ci
    environment:
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: admin
      POSTGRES_DB: {projeto}_db
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  jenkins_home:
  postgres_data:
```

## 📝 6. Scripts de Execução

### run-unit-tests.sh
```bash
#!/bin/bash
dotnet test tests/{Projeto}.TestesUnitarios/{Projeto}.TestesUnitarios.csproj
```

### run-unit-tests-coverage.sh
```bash
#!/bin/bash
rm -rf ./coverage
dotnet test tests/{Projeto}.TestesUnitarios/{Projeto}.TestesUnitarios.csproj \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=./coverage/

dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:./coverage/coverage.cobertura.xml \
    -targetdir:./coverage/html \
    -reporttypes:Html

echo "Relatório gerado em: ./coverage/html/index.html"
```

### run-tests.sh
```bash
#!/bin/bash
cd tests/{Projeto}.TestesIntegrados
docker-compose up -d
dotnet test
docker-compose down
```

### start-jenkins.sh
```bash
#!/bin/bash
cd jenkins-ci
docker-compose up -d
echo "Jenkins: http://localhost:8090"
```

## ✅ Checklist de Implementação

- [ ] Criar projeto {Projeto}.TestesUnitarios
- [ ] Criar projeto {Projeto}.TestesIntegrados
- [ ] Criar pasta {Projeto}.TestesCarga.K6
- [ ] Implementar IntegrationTestFactory
- [ ] Implementar TestDataBuilder
- [ ] Criar testes para cada entidade
- [ ] Criar testes de fluxo completo
- [ ] Criar scripts K6
- [ ] Organizar scripts SQL em database/scripts/
- [ ] Criar Jenkinsfile
- [ ] Criar docker-compose.yml para Jenkins
- [ ] Criar scripts de execução
- [ ] Documentar em README.md
- [ ] Configurar cobertura mínima (20%)
- [ ] Testar pipeline completo

## 🎯 Metas de Qualidade

### Testes Unitários
- Cobertura ≥ 60%
- Execução < 30 segundos
- Todos os domínios testados

### Testes Integrados
- Todas as rotas testadas
- Status codes validados
- Execução < 2 minutos

### Testes de Carga
- p(95) < 500ms
- Taxa de erro < 1%
- 100+ usuários simultâneos

### CI/CD
- Build automatizado
- Cobertura mínima 20%
- Deploy automático (branch main)


## 🐳 7. Dockerfiles

### deploy/Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /repo

# Copia todo o repositório
COPY . .

# Entra na pasta do projeto da API
WORKDIR /repo/src/{Projeto}Api

# Restaura e publica
RUN dotnet restore {Projeto}Api.csproj
RUN dotnet publish {Projeto}Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "{Projeto}Api.dll"]
```

### jenkins-ci/Dockerfile.jenkins
```dockerfile
FROM jenkins/jenkins:lts

USER root

RUN apt-get update && \
    apt-get install -y docker.io && \
    usermod -aG docker jenkins && \
    rm -rf /var/lib/apt/lists/*

USER jenkins
```

### jenkins-ci/Dockerfile.agent
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0

RUN apt-get update && apt-get install -y \
    docker.io \
    git \
    curl \
    wget \
    openjdk-17-jre \
    && rm -rf /var/lib/apt/lists/*

# Instalar Docker Compose
RUN curl -L "https://github.com/docker/compose/releases/download/v2.24.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose \
    && chmod +x /usr/local/bin/docker-compose

# Configurar usuário jenkins
RUN useradd -m -s /bin/bash jenkins
RUN usermod -aG docker jenkins

WORKDIR /app
USER jenkins

CMD ["tail", "-f", "/dev/null"]
```

## 📚 8. Documentação Adicional

### tests/README.md (Principal)
```markdown
# Projetos de Testes - {Projeto} API

## 📁 Estrutura Completa

```
tests/
├── {Projeto}.TestesUnitarios/           # Testes unitários (60%+ cobertura)
├── {Projeto}.TestesIntegrados/          # Testes integrados
└── {Projeto}.TestesCarga.K6/            # Testes de carga
```

## 🧪 {Projeto}.TestesUnitarios

**Cobertura de Testes:**
- ✅ **Domain**: Entidades e regras de negócio
- ✅ **Services**: Serviços de aplicação
- ✅ **UseCases**: Casos de uso
- ✅ **Auth**: Autenticação e autorização
- ✅ **Controllers**: Validação de endpoints

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

## 🔗 {Projeto}.TestesIntegrados

**Cobertura Completa:**
- ✅ Auth (Login, Register, Refresh)
- ✅ CRUD completo de todas entidades
- ✅ Validações de entrada
- ✅ Regras de negócio
- ✅ Fluxos end-to-end

**Executar:**
```bash
./run-tests.sh
```

---

## ⚡ {Projeto}.TestesCarga.K6

**Cenários:**
- Load testing (usuários normais)
- Stress testing (picos de carga)
- Fluxo completo

---

## 🚀 Executar Todos os Testes

### Sequência Completa
```bash
# 1. Testes unitários com cobertura
./run-unit-tests.sh

# 2. Testes integrados + Docker
./run-tests.sh

# 3. Testes de carga (opcional)
cd tests/{Projeto}.TestesCarga.K6
k6 run scripts/auth-load-test.js
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
```

### tests/{Projeto}.TestesIntegrados/EXECUTAR.md
```markdown
# Como Executar os Testes Integrados

## Pré-requisitos
- Docker Desktop instalado e rodando
- .NET 8 SDK

## Opção 1: Script Automatizado (Recomendado)
```bash
./run-tests.sh
```

## Opção 2: Manual
```bash
cd tests/{Projeto}.TestesIntegrados
docker-compose up -d
dotnet test
docker-compose down
```

## Opção 3: Apenas um teste
```bash
dotnet test --filter "FullyQualifiedName~AuthTests"
```

## Troubleshooting

### Docker não inicia
```bash
docker ps
# Reiniciar Docker Desktop
```

### Porta já em uso
Alterar porta no docker-compose.yml:
```yaml
ports:
  - "5433:5432"  # Usar 5433
```
```

## 🎓 Comandos Úteis

### Criar Projetos de Teste
```bash
# Testes Unitários
dotnet new xunit -n {Projeto}.TestesUnitarios -o tests/{Projeto}.TestesUnitarios
cd tests/{Projeto}.TestesUnitarios
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package coverlet.collector
dotnet add package coverlet.msbuild

# Testes Integrados
dotnet new xunit -n {Projeto}.TestesIntegrados -o tests/{Projeto}.TestesIntegrados
cd tests/{Projeto}.TestesIntegrados
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Testcontainers.PostgreSql
dotnet add package FluentAssertions
dotnet add package Bogus

# Adicionar referências
dotnet add reference ../../src/{Projeto}Api/{Projeto}Api.csproj
```

### Executar Testes
```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true

# Filtrar por nome
dotnet test --filter "FullyQualifiedName~{Entidade}Tests"

# Verboso
dotnet test --logger "console;verbosity=detailed"
```

### K6
```bash
# Instalar K6 (macOS)
brew install k6

# Executar teste
k6 run scripts/auth-load-test.js

# Com URL customizada
k6 run -e BASE_URL=https://api.production.com scripts/auth-load-test.js

# Gerar relatório
k6 run --out json=results/result.json scripts/auth-load-test.js
```

## 🔄 Integração com CI/CD

O pipeline Jenkins executa automaticamente:
1. Testes unitários com cobertura
2. Validação de cobertura mínima (20%)
3. Build da imagem Docker
4. Push para DockerHub (branch main)

## 📖 Referências

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions](https://fluentassertions.com/)
- [Testcontainers](https://dotnet.testcontainers.org/)
- [K6 Documentation](https://k6.io/docs/)
- [Bogus](https://github.com/bchavez/Bogus)
