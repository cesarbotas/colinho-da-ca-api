# Configurar User Secrets Local - Passo a Passo

## O que são User Secrets?
User Secrets é uma forma segura de armazenar dados sensíveis durante o desenvolvimento local, sem expor no Git.

## Passo a Passo

### 1. Abrir Terminal no Diretório do Projeto

**Windows (CMD ou PowerShell):**
```cmd
cd c:\Git\Botas\colinho-da-ca-api\src\ColinhoDaCaApi
```

**macOS/Linux:**
```bash
cd /caminho/para/colinho-da-ca-api/src/ColinhoDaCaApi
```

### 2. Inicializar User Secrets

```bash
dotnet user-secrets init
```

**Resultado esperado:**
```
Set UserSecretsId to 'aspnet-ColinhoDaCaApi-xxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' for MSBuild project 'c:\Git\Botas\colinho-da-ca-api\src\ColinhoDaCaApi\ColinhoDaCaApi.csproj'.
```

✅ Isso cria um ID único no arquivo `.csproj` e uma pasta para armazenar seus secrets.

### 3. Adicionar os Secrets (Um por Vez)

Copie e cole cada comando abaixo no terminal:

#### JWT Secret
```bash
dotnet user-secrets set "Jwt:Secret" "teste-com-uma-palavra-chave-secreta"
```

#### Connection String
```bash
dotnet user-secrets set "ConnectionStrings:ColinhoDaCaRender" "Host=dpg-d63533npm1nc73ddu7ug-a.ohio-postgres.render.com;Port=5432;Database=colinho_da_ca_db;Username=colinho_da_ca_db_user;Password=DeSlPQfE5lUlW7IKvpycL2Eu8jaBgRvl"
```

#### Email - SMTP User
```bash
dotnet user-secrets set "Email:SmtpUser" "a2065a001@smtp-brevo.com"
```

#### Email - SMTP Password
```bash
dotnet user-secrets set "Email:SmtpPassword" "xsmtpsib-1db1585a5cbc22c3bf3df0cfd2f0ddb1f17642f3d0e6fe51244c2b51ccf1fda2-eK6UVCWIK0se78mK"
```

#### Email - Destino
```bash
dotnet user-secrets set "Email:EmailDestino" "colinhodaca@gmail.com"
```

#### Email - Remetente
```bash
dotnet user-secrets set "Email:RemetenteEmail" "contato@colinhodaca.com.br"
```

**Resultado esperado para cada comando:**
```
Successfully saved Jwt:Secret = teste-com-uma-palavra-chave-secreta to the secret store.
```

### 4. Verificar os Secrets Configurados

```bash
dotnet user-secrets list
```

**Resultado esperado:**
```
ConnectionStrings:ColinhoDaCaRender = Host=dpg-d63533npm1nc73ddu7ug-a...
Email:EmailDestino = colinhodaca@gmail.com
Email:RemetenteEmail = contato@colinhodaca.com.br
Email:SmtpPassword = xsmtpsib-1db1585a5cbc22c3bf3df0cfd2f0ddb1f17642f3d0e6fe51244c2b51ccf1fda2-eK6UVCWIK0se78mK
Email:SmtpUser = a2065a001@smtp-brevo.com
Jwt:Secret = teste-com-uma-palavra-chave-secreta
```

### 5. Testar a Aplicação

```bash
dotnet run
```

ou no Visual Studio: pressione **F5**

**Verificar se está funcionando:**
- A aplicação deve iniciar sem erros
- Acesse: http://localhost:5163/swagger
- Teste o endpoint de login

### 6. Onde os Secrets Ficam Armazenados?

**Windows:**
```
%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
```

**macOS/Linux:**
```
~/.microsoft/usersecrets/<user_secrets_id>/secrets.json
```

**Exemplo do caminho completo (Windows):**
```
C:\Users\SeuUsuario\AppData\Roaming\Microsoft\UserSecrets\aspnet-ColinhoDaCaApi-xxxxx\secrets.json
```

### 7. Ver o Conteúdo do Arquivo secrets.json

**Windows:**
```cmd
type %APPDATA%\Microsoft\UserSecrets\aspnet-ColinhoDaCaApi-*\secrets.json
```

**PowerShell:**
```powershell
Get-Content $env:APPDATA\Microsoft\UserSecrets\aspnet-ColinhoDaCaApi-*\secrets.json
```

**macOS/Linux:**
```bash
cat ~/.microsoft/usersecrets/aspnet-ColinhoDaCaApi-*/secrets.json
```

**Conteúdo esperado:**
```json
{
  "Jwt:Secret": "teste-com-uma-palavra-chave-secreta",
  "ConnectionStrings:ColinhoDaCaRender": "Host=dpg-...",
  "Email:SmtpUser": "a2065a001@smtp-brevo.com",
  "Email:SmtpPassword": "xsmtpsib-...",
  "Email:EmailDestino": "colinhodaca@gmail.com",
  "Email:RemetenteEmail": "contato@colinhodaca.com.br"
}
```

## Comandos Úteis

### Remover um Secret Específico
```bash
dotnet user-secrets remove "Jwt:Secret"
```

### Limpar Todos os Secrets
```bash
dotnet user-secrets clear
```

### Adicionar Secret com Valor que Contém Espaços
```bash
dotnet user-secrets set "Jwt:Secret" "minha chave com espaços"
```

### Adicionar Secret com Aspas no Valor
```bash
dotnet user-secrets set "Jwt:Secret" "\"valor com aspas\""
```

## Troubleshooting

### Erro: "Could not find the global property 'UserSecretsId'"
**Solução:** Execute `dotnet user-secrets init` primeiro

### Erro: "No executable found matching command 'dotnet-user-secrets'"
**Solução:** Atualize o .NET SDK para versão 2.0 ou superior

### Secrets não estão sendo lidos
**Verificar:**
1. Confirme que está no ambiente Development:
   ```bash
   echo $env:ASPNETCORE_ENVIRONMENT  # PowerShell
   echo %ASPNETCORE_ENVIRONMENT%     # CMD
   ```
   Deve retornar: `Development`

2. Verifique se o UserSecretsId está no .csproj:
   ```bash
   type ColinhoDaCaApi.csproj | findstr UserSecretsId
   ```

### Aplicação não inicia após configurar secrets
**Verificar:**
1. Connection string está correta
2. Banco de dados está acessível
3. Veja os logs de erro no console

## Script Completo (Copiar e Colar Tudo)

```bash
# Navegar para o diretório
cd c:\Git\Botas\colinho-da-ca-api\src\ColinhoDaCaApi

# Inicializar
dotnet user-secrets init

# Adicionar todos os secrets
dotnet user-secrets set "Jwt:Secret" "teste-com-uma-palavra-chave-secreta"
dotnet user-secrets set "ConnectionStrings:ColinhoDaCaRender" "Host=dpg-d63533npm1nc73ddu7ug-a.ohio-postgres.render.com;Port=5432;Database=colinho_da_ca_db;Username=colinho_da_ca_db_user;Password=DeSlPQfE5lUlW7IKvpycL2Eu8jaBgRvl"
dotnet user-secrets set "Email:SmtpUser" "a2065a001@smtp-brevo.com"
dotnet user-secrets set "Email:SmtpPassword" "xsmtpsib-1db1585a5cbc22c3bf3df0cfd2f0ddb1f17642f3d0e6fe51244c2b51ccf1fda2-eK6UVCWIK0se78mK"
dotnet user-secrets set "Email:EmailDestino" "colinhodaca@gmail.com"
dotnet user-secrets set "Email:RemetenteEmail" "contato@colinhodaca.com.br"

# Listar para verificar
dotnet user-secrets list

# Executar aplicação
dotnet run
```

## Vantagens do User Secrets

✅ Não fica no Git  
✅ Específico por desenvolvedor  
✅ Fácil de gerenciar  
✅ Funciona automaticamente em Development  
✅ Não precisa modificar código  

## Próximos Passos

Após configurar os User Secrets localmente:
1. ✅ Desenvolvimento local funcionando
2. 🚀 Configure as variáveis de ambiente no Render (veja RENDER-SECRETS-SETUP.md)
3. 🔒 Nunca commite o arquivo secrets.json no Git
4. 📝 Documente quais secrets são necessários para novos desenvolvedores

## Compartilhar com a Equipe

**NÃO compartilhe:**
- ❌ O arquivo secrets.json
- ❌ Os valores dos secrets por email/chat

**Compartilhe:**
- ✅ Este guia (USER-SECRETS-LOCAL.md)
- ✅ Lista de quais secrets são necessários
- ✅ Onde obter os valores (gerenciador de senhas da equipe)
