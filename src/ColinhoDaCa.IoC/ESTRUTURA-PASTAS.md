# Estrutura de Pastas - ColinhoDaCa.IoC

```
ColinhoDaCa.IoC/
├── Extensions/
│   ├── PersistenceExtensions.cs
│   └── UseCaseExtensions.cs
├── ColinhoDaCa.IoC.csproj
├── CONTEXT.md
└── ServiceRegistrationExtensions.cs
```

## 📁 Descrição dos Arquivos

### Extensions/
Pasta com extensões para registro de dependências por categoria.

**PersistenceExtensions.cs**
- Configuração do DbContext (PostgreSQL)
- Registro do Unit of Work
- Registro de todos os repositórios

**UseCaseExtensions.cs**
- Registro de todos os Use Cases
- Registro de Services (Email, Auth, Validation)

### ServiceRegistrationExtensions.cs
Ponto de entrada principal que orquestra todas as extensões.

### ColinhoDaCa.IoC.csproj
Arquivo de projeto com referências às camadas Domain, Application e Infra.Data.

### CONTEXT.md
Documentação completa da camada de IoC.
