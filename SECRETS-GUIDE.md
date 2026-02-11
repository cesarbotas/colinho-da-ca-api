# Guia de Proteção de Dados Sensíveis

## 1. User Secrets (Desenvolvimento Local)

### Configurar User Secrets
```bash
# No diretório do projeto ColinhoDaCaApi
dotnet user-secrets init
dotnet user-secrets set "Jwt:Secret" "sua-chave-jwt-super-secreta-aqui"
dotnet user-secrets set "Email:SmtpPassword" "sua-senha-smtp-aqui"
dotnet user-secrets set "ConnectionStrings:ColinhoDaCaRender" "sua-connection-string-aqui"
```

### Listar secrets configurados
```bash
dotnet user-secrets list
```

### Remover um secret
```bash
dotnet user-secrets remove "Jwt:Secret"
```

### Limpar todos os secrets
```bash
dotnet user-secrets clear
```

## 2. Variáveis de Ambiente (Produção)

### No Render, Azure, AWS, etc:
- Configurar variáveis de ambiente no painel de controle
- Exemplo Render:
  - JWT__SECRET=sua-chave-jwt
  - EMAIL__SMTPPASSWORD=sua-senha-smtp
  - CONNECTIONSTRINGS__COLINHODACARENDER=sua-connection-string

### No Docker:
```yaml
environment:
  - JWT__SECRET=${JWT_SECRET}
  - EMAIL__SMTPPASSWORD=${EMAIL_SMTP_PASSWORD}
```

## 3. Azure Key Vault (Produção Empresarial)

### Instalar pacote
```bash
dotnet add package Azure.Extensions.AspNetCore.Configuration.Secrets
dotnet add package Azure.Identity
```

### Configurar no Program.cs
```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());
```

## 4. AWS Secrets Manager

### Instalar pacote
```bash
dotnet add package AWSSDK.SecretsManager
```

## 5. Boas Práticas

### ✅ FAZER:
1. Usar User Secrets em desenvolvimento
2. Usar variáveis de ambiente em produção
3. Adicionar appsettings.json ao .gitignore se contiver secrets
4. Criar appsettings.Example.json com valores de exemplo
5. Documentar quais secrets são necessários
6. Rotacionar secrets periodicamente
7. Usar diferentes secrets para cada ambiente

### ❌ NÃO FAZER:
1. Commitar secrets no Git
2. Compartilhar secrets por email/chat
3. Usar mesmos secrets em dev e prod
4. Deixar secrets em código
5. Usar secrets fracos ou padrão

## 6. Estrutura Recomendada

### appsettings.json (commitado)
```json
{
  "Jwt": {
    "Secret": "",
    "ExpirationHours": "24"
  },
  "Email": {
    "SmtpHost": "smtp-relay.brevo.com",
    "SmtpPort": "587",
    "SmtpUser": "",
    "SmtpPassword": "",
    "EmailDestino": "",
    "RemetenteNome": "Colinho da Ca - Site",
    "RemetenteEmail": ""
  }
}
```

### appsettings.Development.json (no .gitignore)
```json
{
  "ConnectionStrings": {
    "ColinhoDaCaRender": "Host=localhost;Database=colinho_dev"
  }
}
```

### .env (no .gitignore) - Para Docker
```
JWT_SECRET=sua-chave-jwt
EMAIL_SMTP_PASSWORD=sua-senha-smtp
DB_CONNECTION_STRING=sua-connection-string
```

## 7. Verificar se há secrets expostos

### Instalar git-secrets
```bash
# macOS
brew install git-secrets

# Windows
# Baixar de: https://github.com/awslabs/git-secrets
```

### Configurar
```bash
git secrets --install
git secrets --register-aws
git secrets --add 'password.*=.*'
git secrets --add 'secret.*=.*'
```

## 8. Remover secrets do histórico Git (se já commitou)

```bash
# CUIDADO: Reescreve histórico
git filter-branch --force --index-filter \
  "git rm --cached --ignore-unmatch appsettings.json" \
  --prune-empty --tag-name-filter cat -- --all

# Forçar push
git push origin --force --all
```

## 9. Configuração Atual Recomendada

### Passo 1: Limpar appsettings.json
Remover todos os valores sensíveis e deixar vazios

### Passo 2: Configurar User Secrets
```bash
cd src/ColinhoDaCaApi
dotnet user-secrets set "Jwt:Secret" "teste-com-uma-palavra-chave-secreta"
dotnet user-secrets set "Email:SmtpPassword" "xsmtpsib-1db1585a5cbc22c3bf3df0cfd2f0ddb1f17642f3d0e6fe51244c2b51ccf1fda2-eK6UVCWIK0se78mK"
dotnet user-secrets set "ConnectionStrings:ColinhoDaCaRender" "Host=dpg-d63533npm1nc73ddu7ug-a.ohio-postgres.render.com;Port=5432;Database=colinho_da_ca_db;Username=colinho_da_ca_db_user;Password=DeSlPQfE5lUlW7IKvpycL2Eu8jaBgRvl"
dotnet user-secrets set "Email:SmtpUser" "a2065a001@smtp-brevo.com"
dotnet user-secrets set "Email:EmailDestino" "colinhodaca@gmail.com"
dotnet user-secrets set "Email:RemetenteEmail" "contato@colinhodaca.com.br"
```

### Passo 3: Configurar no Render (Produção)
Environment Variables:
- JWT__SECRET
- EMAIL__SMTPPASSWORD
- EMAIL__SMTPUSER
- EMAIL__EMAILDESTINO
- EMAIL__REMETENTEEMAIL
- CONNECTIONSTRINGS__COLINHODACARENDER

### Passo 4: Atualizar .gitignore
```
appsettings.Development.json
appsettings.Production.json
*.user
.env
.env.local
```
