#!/bin/bash

echo "🚀 Iniciando ambiente Jenkins CI/CD..."

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
echo "🌐 Jenkins: http://localhost:8080"
echo "🐘 PostgreSQL: localhost:5432"
echo ""
echo "📋 Próximos passos:"
echo "1. Acesse http://localhost:8080"
echo "2. Use a senha acima para configurar"
echo "3. Instale plugins sugeridos"
echo "4. Crie um novo Pipeline job"
echo "5. Configure o repositório Git"