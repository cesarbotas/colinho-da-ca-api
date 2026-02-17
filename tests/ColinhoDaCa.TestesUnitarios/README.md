# Testes Unitários - Colinho da Cá API

## 📊 Cobertura de Código

Este projeto utiliza o **Coverlet** para análise de cobertura de código nos testes unitários.

### Executar Testes com Cobertura

```bash
# Executar testes com cobertura
./run-unit-tests-coverage.sh

# Ou manualmente:
cd tests/ColinhoDaCa.TestesUnitarios
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
```

### Gerar Relatório HTML

```bash
# Gerar relatório HTML de cobertura
./generate-coverage-report.sh

# Ou manualmente:
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"tests/ColinhoDaCa.TestesUnitarios/coverage/**/coverage.cobertura.xml" -targetdir:"tests/ColinhoDaCa.TestesUnitarios/coverage/html" -reporttypes:Html
```

### Visualizar Relatório

Após gerar o relatório HTML, abra o arquivo:
```
tests/ColinhoDaCa.TestesUnitarios/coverage/html/index.html
```

## 🔧 Configuração

### Coverlet MSBuild

O projeto está configurado com:
- **coverlet.msbuild**: Para integração com MSBuild
- **coverlet.collector**: Para coleta de dados de cobertura

### Configurações de Cobertura

```xml
<PropertyGroup>
  <CollectCoverage>true</CollectCoverage>
  <CoverletOutputFormat>opencover,lcov,json</CoverletOutputFormat>
  <CoverletOutput>./coverage/</CoverletOutput>
  <Exclude>[*]*.Program,[*]*.Startup,[*]*Migrations*,[*]*Tests*</Exclude>
  <ExcludeByFile>**/Migrations/**/*</ExcludeByFile>
</PropertyGroup>
```

### Exclusões

- Classes `Program` e `Startup`
- Arquivos de migração
- Classes de teste
- Diretório `Migrations`

## 📈 Métricas Atuais

- **Total**: 14.86% de cobertura de linha
- **ColinhoDaCa.Application**: 11.65% de cobertura
- **ColinhoDaCa.Domain**: 27.31% de cobertura

## 🎯 Objetivos

- Aumentar cobertura para pelo menos 80%
- Focar em cenários críticos de negócio
- Manter testes rápidos e confiáveis