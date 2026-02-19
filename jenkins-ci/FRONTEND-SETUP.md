# Configuração Pipeline Front-end

## 🎯 O que foi adicionado

1. **Node.js 20 LTS** no Docker Agent
2. **npm e Yarn** para gerenciamento de pacotes
3. **Jenkinsfile.frontend** - Pipeline pronto para front-end
4. **Dockerfile.frontend** - Build otimizado com Nginx
5. **nginx.conf** - Configuração para SPA com proxy para API

## 🚀 Como usar

### 1. Rebuild do Agent
```bash
cd jenkins-ci
docker compose down
docker compose up -d --build
```

### 2. Criar Pipeline no Jenkins

1. Acesse Jenkins: http://localhost:8090
2. New Item → Pipeline
3. Nome: `colinho-frontend`
4. Pipeline → Definition: Pipeline script from SCM
5. SCM: Git
6. Repository URL: [URL do repositório front-end]
7. Script Path: `Jenkinsfile` (ou copie o Jenkinsfile.frontend para raiz)

### 3. Estrutura do Projeto Front-end

```
frontend/
├── src/
├── public/
├── package.json
├── Dockerfile          # Copiar de jenkins-ci/Dockerfile.frontend
├── nginx.conf          # Copiar de jenkins-ci/nginx.conf
└── Jenkinsfile         # Copiar de jenkins-ci/Jenkinsfile.frontend
```

### 4. Ajustar Dockerfile

Edite o Dockerfile conforme seu framework:

**React/Vite:**
```dockerfile
RUN npm run build
# dist/ já está correto
```

**Angular:**
```dockerfile
RUN npm run build
COPY --from=builder /app/dist/[nome-projeto] /usr/share/nginx/html
```

**Vue:**
```dockerfile
RUN npm run build
# dist/ já está correto
```

### 5. Ajustar nginx.conf

Altere o proxy_pass para apontar para sua API:
```nginx
location /api {
    proxy_pass http://colinho-api:8080;  # Ajustar conforme necessário
}
```

## 🔧 Variáveis de Ambiente

No Jenkinsfile, ajuste:
```groovy
environment {
    DOCKER_IMAGE = 'seu-registry/colinho-frontend'
    DOCKER_TAG = "${env.BUILD_NUMBER}"
}
```

## 📦 Docker Registry

Para push automático, configure credenciais no Jenkins:

1. Manage Jenkins → Credentials
2. Add Credentials → Username with password
3. ID: `docker-registry`
4. Adicione no Jenkinsfile:
```groovy
stage('Push Image') {
    steps {
        script {
            docker.withRegistry('https://registry.hub.docker.com', 'docker-registry') {
                sh "docker push ${DOCKER_IMAGE}:${DOCKER_TAG}"
            }
        }
    }
}
```

## ✅ Verificar Instalação

```bash
# Entrar no agent
docker exec -it jenkins-agent bash

# Verificar versões
node --version    # v20.x.x
npm --version     # 10.x.x
yarn --version    # 1.x.x
dotnet --version  # 8.0.x
```

## 🎨 Frameworks Suportados

- ✅ React
- ✅ Vue.js
- ✅ Angular
- ✅ Next.js (com ajustes)
- ✅ Svelte
- ✅ Vite
