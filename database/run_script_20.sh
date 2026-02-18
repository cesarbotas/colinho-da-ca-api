#!/bin/bash

# Script para executar correção das colunas problemáticas
# Execute este script na pasta database

echo "=== Executando correção das colunas problemáticas (Script 20) ==="

# Configurações do banco (ajuste conforme necessário)
DB_HOST="${DB_HOST:-localhost}"
DB_USER="${DB_USER:-postgres}"
DB_NAME="${DB_NAME:-colinho_da_ca_db}"

echo "Conectando ao banco: $DB_NAME em $DB_HOST como usuário $DB_USER"

# Executar script 20
echo "Executando script 20_CorrigirColunasProblematicas.sql..."
psql -h $DB_HOST -U $DB_USER -d $DB_NAME -f scripts/20_CorrigirColunasProblematicas.sql

if [ $? -eq 0 ]; then
    echo "✓ Script 20 executado com sucesso"
    echo "✓ Colunas problemáticas removidas"
    echo ""
    echo "Próximos passos:"
    echo "1. Limpe o cache do .NET: rm -rf src/*/bin src/*/obj"
    echo "2. Recompile a aplicação: dotnet build"
    echo "3. Teste a aplicação"
else
    echo "✗ Erro ao executar script 20"
    exit 1
fi

echo "=== Correção concluída! ==="