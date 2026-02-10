# Configurar Secrets no Render

## Passo a Passo

### 1. Acessar o Dashboard do Render
1. Acesse https://dashboard.render.com
2. Faça login na sua conta
3. Selecione seu serviço (Web Service da API)

### 2. Acessar Environment Variables
1. No menu lateral do seu serviço, clique em **"Environment"**
2. Ou acesse diretamente: `https://dashboard.render.com/web/[seu-service-id]/env`

### 3. Adicionar Variáveis de Ambiente

Clique em **"Add Environment Variable"** e adicione cada uma:

#### Formato no Render:
O Render usa `__` (dois underscores) para representar `:` (dois pontos) na hierarquia JSON.

#### Variáveis Necessárias:

```
Key: JWT__SECRET
Value: teste-com-uma-palavra-chave-secreta
```

```
Key: JWT__EXPIRATIONHOURS
Value: 24
```

```
Key: CONNECTIONSTRINGS__COLINHODACARENDER
Value: Host=dpg-d63533npm1nc73ddu7ug-a.ohio-postgres.render.com;Port=5432;Database=colinho_da_ca_db;Username=colinho_da_ca_db_user;Password=DeSlPQfE5lUlW7IKvpycL2Eu8jaBgRvl
```

```
Key: EMAIL__SMTPHOST
Value: smtp-relay.brevo.com
```

```
Key: EMAIL__SMTPPORT
Value: 587
```

```
Key: EMAIL__SMTPUSER
Value: a2065a001@smtp-brevo.com
```

```
Key: EMAIL__SMTPPASSWORD
Value: xsmtpsib-1db1585a5cbc22c3bf3df0cfd2f0ddb1f17642f3d0e6fe51244c2b51ccf1fda2-eK6UVCWIK0se78mK
```

```
Key: EMAIL__EMAILDESTINO
Value: colinhodaca@gmail.com
```

```
Key: EMAIL__REMETENTENOME
Value: Colinho da Ca - Site
```

```
Key: EMAIL__REMETENTEEMAIL
Value: contato@colinhodaca.com.br
```

### 4. Salvar e Aplicar

1. Após adicionar todas as variáveis, clique em **"Save Changes"**
2. O Render irá automaticamente fazer o **redeploy** da aplicação
3. Aguarde o deploy finalizar (pode levar alguns minutos)

### 5. Verificar se está funcionando

Após o deploy:
1. Acesse os logs do serviço
2. Verifique se não há erros relacionados a configurações
3. Teste os endpoints da API

## Alternativa: Usar Secret Files (Mais Seguro)

Para dados muito sensíveis, o Render também suporta **Secret Files**:

### 1. Criar Secret File
1. No menu Environment, clique em **"Secret Files"**
2. Clique em **"Add Secret File"**

### 2. Configurar
```
Filename: appsettings.Production.json
Contents:
{
  "Jwt": {
    "Secret": "teste-com-uma-palavra-chave-secreta"
  },
  "Email": {
    "SmtpPassword": "xsmtpsib-1db1585a5cbc22c3bf3df0cfd2f0ddb1f17642f3d0e6fe51244c2b51ccf1fda2-eK6UVCWIK0se78mK"
  }
}
```

### 3. Configurar Program.cs para ler o arquivo
```csharp
builder.Configuration.AddJsonFile(
    "/etc/secrets/appsettings.Production.json", 
    optional: true, 
    reloadOnChange: true);
```

## Dicas Importantes

### ✅ Boas Práticas:
1. **Nunca** compartilhe as variáveis de ambiente publicamente
2. Use valores diferentes para desenvolvimento e produção
3. Rotacione secrets periodicamente (especialmente JWT Secret)
4. Documente quais variáveis são necessárias

### 🔄 Atualizar uma Variável:
1. Vá em Environment
2. Clique no ícone de editar (lápis) ao lado da variável
3. Altere o valor
4. Salve (irá fazer redeploy automático)

### 🗑️ Remover uma Variável:
1. Vá em Environment
2. Clique no ícone de lixeira ao lado da variável
3. Confirme a remoção

### 📋 Copiar de outro serviço:
Se você tem múltiplos serviços no Render:
1. Pode copiar as variáveis de um serviço para outro
2. Use a opção "Copy from another service"

## Verificação Rápida

Após configurar, teste com curl:

```bash
# Testar login
curl -X POST https://seu-app.onrender.com/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "teste@teste.com",
    "senha": "senha123"
  }'
```

Se retornar um token JWT, está funcionando! ✅

## Troubleshooting

### Erro: "Configuration value not found"
- Verifique se o nome da variável está correto (case-sensitive)
- Confirme que usou `__` (dois underscores) no lugar de `:`

### Erro: "Connection string invalid"
- Verifique se a connection string está completa
- Confirme que o banco de dados está acessível

### Aplicação não reiniciou
- Force um redeploy manual: Settings > Manual Deploy > Deploy Latest Commit

## Exemplo de Configuração Completa no Render

```
Environment Variables (10):
├── JWT__SECRET = ****************************
├── JWT__EXPIRATIONHOURS = 24
├── CONNECTIONSTRINGS__COLINHODACARENDER = Host=dpg-***
├── EMAIL__SMTPHOST = smtp-relay.brevo.com
├── EMAIL__SMTPPORT = 587
├── EMAIL__SMTPUSER = a2065a001@smtp-brevo.com
├── EMAIL__SMTPPASSWORD = ****************************
├── EMAIL__EMAILDESTINO = colinhodaca@gmail.com
├── EMAIL__REMETENTENOME = Colinho da Ca - Site
└── EMAIL__REMETENTEEMAIL = contato@colinhodaca.com.br
```

## Automação (Opcional)

Para automatizar via Render API:

```bash
# Instalar Render CLI
npm install -g @render-com/cli

# Configurar variável
render env set JWT__SECRET "sua-chave-secreta" --service-id srv-xxxxx
```

## Backup das Configurações

**IMPORTANTE**: Mantenha um backup seguro das suas variáveis de ambiente em um gerenciador de senhas (1Password, LastPass, Bitwarden, etc.)

Exemplo de estrutura para backup:
```
Serviço: Colinho da Ca API - Render
URL: https://colinho-da-ca-api.onrender.com

Variáveis:
- JWT__SECRET: [valor]
- EMAIL__SMTPPASSWORD: [valor]
- CONNECTIONSTRINGS__COLINHODACARENDER: [valor]
...
```
