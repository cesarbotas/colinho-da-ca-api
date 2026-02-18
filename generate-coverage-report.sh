#!/bin/bash

echo "📊 Gerando relatório HTML de cobertura..."

# Verificar se o ReportGenerator está instalado
if ! command -v reportgenerator &> /dev/null; then
    echo "🔧 Instalando ReportGenerator..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
fi

# Gerar relatório HTML
reportgenerator \
    -reports:"tests/ColinhoDaCa.TestesUnitarios/coverage/**/coverage.cobertura.xml" \
    -targetdir:"tests/ColinhoDaCa.TestesUnitarios/coverage/html" \
    -reporttypes:Html

echo "✅ Relatório HTML gerado com sucesso!"
echo "🌐 Abra o arquivo: tests/ColinhoDaCa.TestesUnitarios/coverage/html/index.html"