#!/bin/bash

echo "🚀 Iniciando ambiente Jenkins CI/CD..."

# Verificar se Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Iniciando Docker..."
    open -a Docker
    echo "⏳ Aguardando Docker inicializar..."
    while ! docker info > /dev/null 2>&1; do
        sleep 2
    done
    echo "✅ Docker iniciado com sucesso"
fi

cd jenkins-ci

# Subir containers
docker compose up -d --build

echo "⏳ Aguardando Jenkins inicializar..."
sleep 30

# Mostrar senha inicial
echo "🔑 Senha inicial do Jenkins:"
docker exec jenkins cat /var/jenkins_home/secrets/initialAdminPassword

echo ""
echo "✅ Ambiente Jenkins disponível em:"
echo "🌐 Jenkins: http://localhost:8090"
echo "🐘 PostgreSQL: localhost:5432"
echo ""
echo "📋 Próximos passos:"
echo "1. Acesse http://localhost:8090"
echo "2. Use a senha acima para configurar"
echo "3. Instale plugins sugeridos"
echo "4. Crie um novo Pipeline job"
echo "5. Configure o repositório Git"