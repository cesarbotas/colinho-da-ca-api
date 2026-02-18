# Jenkins - Configuração e Solução de Problemas

## 🚨 Problema Identificado

O erro `dotnet: not found` indica que o Jenkins está executando no agente principal sem .NET SDK instalado.

## ✅ Soluções Implementadas

### 1. Jenkinsfile com Docker Agent
- **Arquivo**: `Jenkinsfile` (atualizado)
- **Agente**: Container Docker com .NET SDK 8.0
- **Vantagem**: Ambiente isolado e consistente

### 2. Jenkinsfile Simplificado
- **Arquivo**: `Jenkinsfile.simple`
- **Foco**: Apenas build e testes básicos
- **Uso**: Para ambientes com limitações

## 🔧 Configuração no Jenkins

### Opção 1: Usar Docker Agent (Recomendado)
1. Substitua o conteúdo do Jenkinsfile pelo atualizado
2. Certifique-se que Jenkins tem acesso ao Docker
3. Execute o pipeline

### Opção 2: Instalar .NET no Jenkins
```bash
# No servidor Jenkins
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version latest --install-dir /usr/share/dotnet
ln -s /usr/share/dotnet/dotnet /usr/local/bin
```

### Opção 3: Usar Jenkinsfile Simplificado
1. Renomeie `Jenkinsfile.simple` para `Jenkinsfile`
2. Configure o pipeline para usar este arquivo

## 🐳 Verificar Docker no Jenkins

```bash
# Verificar se Docker está disponível
docker --version

# Verificar se Jenkins pode usar Docker
docker run hello-world
```

## 📋 Checklist de Configuração

- [ ] Jenkins tem acesso ao Docker
- [ ] Plugin Docker Pipeline instalado
- [ ] Jenkinsfile atualizado no repositório
- [ ] Pipeline configurado para branch correta

## 🚀 Próximos Passos

1. **Teste o Jenkinsfile atualizado**
2. **Se falhar**: Use `Jenkinsfile.simple`
3. **Se ainda falhar**: Instale .NET no servidor Jenkins
4. **Para produção**: Configure agente dedicado com .NET

## 📊 Stages do Pipeline

### Básico (Jenkinsfile.simple)
1. ✅ Checkout
2. ✅ Restore
3. ✅ Build
4. ✅ Unit Tests
5. ✅ Publish

### Completo (Jenkinsfile)
1. ✅ Checkout
2. ✅ Restore
3. ✅ Build
4. ✅ Unit Tests
5. ✅ Integration Tests
6. ✅ Publish
7. ✅ Docker Build
8. ✅ Docker Push

## 🔍 Debug

Para verificar o ambiente do agente:
```groovy
stage('Debug') {
    steps {
        sh 'whoami'
        sh 'pwd'
        sh 'ls -la'
        sh 'dotnet --version || echo "dotnet not found"'
        sh 'docker --version || echo "docker not found"'
    }
}
```