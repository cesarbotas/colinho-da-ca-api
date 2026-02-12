@echo off
echo ========================================
echo Testes de Carga - Colinho da Ca API
echo ========================================
echo.

echo [1/3] Executando teste de autenticacao...
k6 run scripts/auth-load-test.js
echo.

echo [2/3] Executando teste de fluxo completo...
k6 run scripts/reserva-flow-test.js
echo.

echo [3/3] Executando teste de stress...
k6 run scripts/stress-test.js
echo.

echo ========================================
echo Todos os testes foram executados!
echo ========================================
pause
