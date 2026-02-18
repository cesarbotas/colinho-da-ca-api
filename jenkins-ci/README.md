# Jenkins CI/CD - Colinho da Cá API

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

## 🔧 Configuração Manual

1. **Acesse Jenkins**: http://localhost:8080
2. **Senha inicial**: Será exibida no terminal
3. **Plugins necessários**:
   - Docker Pipeline
   - Git
   - Pipeline
   - Blue Ocean (opcional)

## 📋 Pipeline Stages

1. **Checkout** - Baixa código fonte
2. **Restore** - Restaura dependências .NET
3. **Build** - Compila aplicação
4. **Unit Tests** - Executa testes unitários com cobertura
5. **Integration Tests** - Executa testes integrados
6. **Publish** - Publica aplicação
7. **Docker Build** - Cria imagem Docker
8. **Docker Push** - Envia para registry (apenas branch main)

## 🐳 Serviços

- **Jenkins**: http://localhost:8080
- **PostgreSQL**: localhost:5432
  - User: admin
  - Password: admin
  - Database: colinhodaca

## 🛠️ Comandos Úteis

```bash
# Parar ambiente
cd jenkins-ci && docker compose down

# Ver logs
docker logs jenkins
docker logs jenkins-agent

# Limpar volumes
docker compose down -v
```

## 📊 Métricas

- ✅ Build automatizado
- ✅ Testes unitários (47 testes)
- ✅ Testes integrados (26 testes)
- ✅ Cobertura de código (71.78% Domain)
- ✅ Imagem Docker otimizada