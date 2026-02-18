# Postman - Colinho da Cá API

## 📋 Coleção Atualizada

A coleção do Postman foi atualizada com todos os endpoints da API e inclui:

### 🔐 Autenticação
- **Registrar**: Cadastro de novos usuários
- **Login OAuth2**: Autenticação com JWT + Refresh Token
- **Refresh Token**: Renovação automática de tokens

### 👥 Clientes
- **Listar**: Paginação configurável
- **Cadastrar**: Criação de novos clientes
- **Alterar**: Atualização de dados
- **Excluir**: Remoção de clientes

### 🐕 Pets
- **Listar**: Por cliente com paginação
- **Cadastrar**: Novo pet vinculado ao cliente
- **Alterar**: Atualização de dados do pet
- **Excluir**: Remoção de pets

### 🏨 Reservas
- **Listar**: Com paginação e filtros
- **Cadastrar**: Nova reserva com múltiplos pets
- **Alterar**: Modificação de reservas
- **Confirmar**: Mudança de status
- **Enviar Comprovante**: Upload de comprovante
- **Aprovar Pagamento**: Aprovação administrativa
- **Visualizar Comprovante**: Download do arquivo
- **Conceder Desconto**: Desconto manual
- **Cancelar**: Cancelamento de reserva
- **Aplicar Cupom**: Validação em tempo real

### 🎫 Cupons
- **Listar**: Cupons disponíveis
- **Cadastrar**: Tipos 1 (Percentual) e 4 (Valor Fixo)
- **Alterar**: Modificação de cupons
- **Inativar**: Desativação de cupons

### 🐾 Raças
- **Listar Todas**: Catálogo completo
- **Buscar por ID**: Consulta específica

### 📧 Sobre
- **Enviar Email**: Formulário de contato

## 🌐 Environments

### Produção
- **BaseURL**: `https://colinho-da-ca-api.onrender.com`

### Local
- **LocalURL**: `http://localhost:5000`

### Variáveis Automáticas
- **BearerToken**: Preenchido automaticamente no login
- **RefreshToken**: Gerenciado automaticamente
- **ClienteId**, **PetId**, **ReservaId**: Para facilitar testes

## 🚀 Como Usar

1. **Importe a coleção**: `Colinho-da-Ca-API.postman_collection.json`
2. **Importe o environment**: `Colinho-da-Ca-API.postman_environment.json`
3. **Execute o fluxo**:
   - Registrar usuário
   - Fazer login (token salvo automaticamente)
   - Testar endpoints autenticados

## 🔧 Scripts Automáticos

### Login
```javascript
if (pm.response.code === 200) {
    var jsonData = pm.response.json();
    pm.environment.set('BearerToken', jsonData.accessToken);
    pm.environment.set('RefreshToken', jsonData.refreshToken);
}
```

### Refresh Token
```javascript
if (pm.response.code === 200) {
    var jsonData = pm.response.json();
    pm.environment.set('BearerToken', jsonData.accessToken);
    pm.environment.set('RefreshToken', jsonData.refreshToken);
}
```

## ✅ Validações

- ✅ Todos os endpoints mapeados
- ✅ Autenticação Bearer Token
- ✅ Payloads de exemplo atualizados
- ✅ Variáveis de ambiente configuradas
- ✅ Scripts de automação implementados
- ✅ Documentação completa

## 🔄 Atualizações Recentes

- Removido campo `timestamp` desnecessário do login
- Adicionadas variáveis de ambiente para IDs
- Corrigida estrutura JSON da coleção
- Adicionado LocalURL para desenvolvimento
- Melhorada documentação dos endpoints