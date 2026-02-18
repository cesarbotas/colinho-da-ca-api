#!/bin/bash

echo "🧪 Executando testes unitários com cobertura de código..."

cd tests/ColinhoDaCa.TestesUnitarios

# Limpar resultados anteriores
rm -rf coverage/

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

echo "✅ Testes executados com sucesso!"
echo "📊 Relatórios de cobertura gerados em: tests/ColinhoDaCa.TestesUnitarios/coverage/"
echo ""
echo "Para visualizar o relatório HTML, instale o ReportGenerator:"
echo "dotnet tool install -g dotnet-reportgenerator-globaltool"
echo ""
echo "E execute:"
echo "reportgenerator -reports:\"tests/ColinhoDaCa.TestesUnitarios/coverage/**/coverage.cobertura.xml\" -targetdir:\"tests/ColinhoDaCa.TestesUnitarios/coverage/html\" -reporttypes:Html"