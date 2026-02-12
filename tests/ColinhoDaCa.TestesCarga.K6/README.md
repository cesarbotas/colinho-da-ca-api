# Testes de Carga com K6 - Colinho da Cá API

Projeto completo de testes de carga e performance usando K6.

## 📋 Pré-requisitos

### Instalar K6

**Windows (Chocolatey):**
```bash
choco install k6
```

**Windows (Scoop):**
```bash
scoop install k6
```

**macOS:**
```bash
brew install k6
```

**Linux:**
```bash
sudo gpg -k
sudo gpg --no-default-keyring --keyring /usr/share/keyrings/k6-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
echo "deb [signed-by=/usr/share/keyrings/k6-archive-keyring.gpg] https://dl.k6.io/deb stable main" | sudo tee /etc/apt/sources.list.d/k6.list
sudo apt-get update
sudo apt-get install k6
```

## 🚀 Executar Testes

### 1. Teste de Autenticação (Load Test)
```bash
k6 run scripts/auth-load-test.js
```

**Cenário:**
- Ramp-up: 0 → 10 usuários (30s)
- Estável: 10 usuários (1min)
- Pico: 10 → 50 usuários (30s)
- Estável: 50 usuários (1min)
- Ramp-down: 50 → 0 (30s)

**Endpoints testados:**
- POST /api/v1/auth/registrar
- POST /api/v1/auth/login
- GET /api/v1/racas

### 2. Teste de Fluxo Completo (Reserva)
```bash
k6 run scripts/reserva-flow-test.js
```

**Cenário:**
- Ramp-up: 0 → 20 usuários (1min)
- Estável: 20 usuários (2min)
- Ramp-down: 20 → 0 (1min)

**Fluxo testado:**
1. Registrar usuário
2. Login
3. Listar clientes
4. Listar raças
5. Cadastrar pet
6. Criar reserva

### 3. Teste de Stress
```bash
k6 run scripts/stress-test.js
```

**Cenário:**
- Ramp-up: 0 → 100 usuários (2min)
- Estável: 100 usuários (5min)
- Pico: 100 → 200 usuários (2min)
- Estável: 200 usuários (5min)
- Stress: 200 → 300 usuários (2min)
- Estável: 300 usuários (5min)
- Ramp-down: 300 → 0 (2min)

### 4. Executar com URL customizada
```bash
k6 run -e BASE_URL=https://api.production.com scripts/auth-load-test.js
```

### 5. Gerar relatório HTML
```bash
k6 run --out json=results/result.json scripts/auth-load-test.js
```

## 📊 Entendendo os Resultados

### Exemplo de Saída do K6

```
     ✓ login status 200
     ✓ token presente
     ✓ listar raças status 200

     checks.........................: 100.00% ✓ 1500      ✗ 0
     data_received..................: 2.1 MB  35 kB/s
     data_sent......................: 450 kB  7.5 kB/s
     http_req_blocked...............: avg=1.2ms    min=0s      med=0s      max=150ms   p(90)=0s      p(95)=0s
     http_req_connecting............: avg=800µs    min=0s      med=0s      max=100ms   p(90)=0s      p(95)=0s
     http_req_duration..............: avg=245ms    min=50ms    med=200ms   max=1.2s    p(90)=400ms   p(95)=500ms
       { expected_response:true }...: avg=245ms    min=50ms    med=200ms   max=1.2s    p(90)=400ms   p(95)=500ms
     http_req_failed................: 0.00%   ✓ 0         ✗ 1500
     http_req_receiving.............: avg=500µs    min=0s      med=0s      max=50ms    p(90)=1ms     p(95)=2ms
     http_req_sending...............: avg=200µs    min=0s      med=0s      max=20ms    p(90)=0s      p(95)=1ms
     http_req_tls_handshaking.......: avg=0s       min=0s      med=0s      max=0s      p(90)=0s      p(95)=0s
     http_req_waiting...............: avg=244ms    min=50ms    med=199ms   max=1.2s    p(90)=399ms   p(95)=499ms
     http_reqs......................: 1500    25/s
     iteration_duration.............: avg=1.24s    min=1.05s   med=1.2s    max=2.5s    p(90)=1.4s    p(95)=1.5s
     iterations.....................: 500     8.33/s
     vus............................: 10      min=10      max=50
     vus_max........................: 50      min=50      max=50
```

### 📈 Métricas Principais

#### 1. **checks** ✓
- **O que é**: Percentual de validações que passaram
- **Ideal**: 100%
- **Problema se**: < 95%
- **Exemplo**: `checks: 100.00% ✓ 1500 ✗ 0` = Todas as 1500 validações passaram

#### 2. **http_req_duration**
- **O que é**: Tempo total de resposta da requisição
- **Métricas importantes**:
  - `avg`: Tempo médio
  - `p(95)`: 95% das requisições foram mais rápidas que este valor
  - `p(99)`: 99% das requisições foram mais rápidas que este valor
- **Ideal**: 
  - p(95) < 500ms
  - p(99) < 1000ms
- **Exemplo**: `p(95)=500ms` = 95% das requisições levaram menos de 500ms

#### 3. **http_req_failed**
- **O que é**: Taxa de requisições que falharam (status 4xx, 5xx)
- **Ideal**: < 1%
- **Problema se**: > 5%
- **Exemplo**: `http_req_failed: 0.00%` = Nenhuma requisição falhou

#### 4. **http_reqs**
- **O que é**: Total de requisições e taxa por segundo
- **Exemplo**: `http_reqs: 1500 25/s` = 1500 requisições, 25 por segundo

#### 5. **vus (Virtual Users)**
- **O que é**: Número de usuários virtuais simultâneos
- **Exemplo**: `vus: 10 min=10 max=50` = Variou de 10 a 50 usuários

#### 6. **iteration_duration**
- **O que é**: Tempo total de uma iteração completa do teste
- **Ideal**: Depende do cenário
- **Exemplo**: `avg=1.24s` = Cada iteração levou em média 1.24s

### 🎯 Thresholds (Limites)

Thresholds definem critérios de sucesso/falha:

```javascript
thresholds: {
  http_req_duration: ['p(95)<500'],      // ✓ PASSOU: 95% < 500ms
  http_req_failed: ['rate<0.05'],        // ✓ PASSOU: Taxa de erro < 5%
  errors: ['rate<0.1'],                  // ✗ FALHOU: Taxa de erro > 10%
}
```

**Resultado:**
```
✓ http_req_duration..............: p(95)=450ms  [threshold: p(95)<500]
✓ http_req_failed................: 2.5%         [threshold: rate<0.05]
✗ errors.........................: 12%          [threshold: rate<0.1]
```

### 🚦 Interpretação de Cores

- **Verde (✓)**: Threshold passou
- **Vermelho (✗)**: Threshold falhou
- **Amarelo**: Aviso, próximo do limite

### 📉 Análise de Performance

#### Excelente Performance
```
http_req_duration: avg=100ms p(95)=200ms p(99)=300ms
http_req_failed: 0.00%
checks: 100.00%
```

#### Performance Aceitável
```
http_req_duration: avg=300ms p(95)=500ms p(99)=800ms
http_req_failed: 1.5%
checks: 98.5%
```

#### Performance Ruim
```
http_req_duration: avg=800ms p(95)=1500ms p(99)=3000ms
http_req_failed: 8%
checks: 92%
```

#### Sistema Sobrecarregado
```
http_req_duration: avg=2000ms p(95)=5000ms p(99)=10000ms
http_req_failed: 25%
checks: 75%
```

## 🔍 Análise Detalhada

### 1. Identificar Gargalos

**Sintomas:**
- `http_req_duration` aumenta com mais usuários
- `http_req_failed` > 5%
- `http_req_waiting` muito alto

**Possíveis causas:**
- Banco de dados lento
- Queries não otimizadas
- Falta de índices
- Pool de conexões insuficiente
- CPU/Memória insuficiente

### 2. Analisar Latência de Rede

```
http_req_blocked........: 1.2ms   (DNS lookup + TCP connection)
http_req_connecting.....: 800µs   (TCP handshake)
http_req_tls_handshaking: 0s      (TLS handshake)
http_req_sending........: 200µs   (Envio de dados)
http_req_waiting........: 244ms   (Tempo de processamento no servidor)
http_req_receiving......: 500µs   (Recebimento de dados)
```

**Análise:**
- `http_req_waiting` alto = Servidor lento
- `http_req_connecting` alto = Problemas de rede
- `http_req_receiving` alto = Resposta muito grande

### 3. Capacidade Máxima

Execute teste de stress até o sistema falhar:

```bash
k6 run scripts/stress-test.js
```

**Observe:**
- Em quantos VUs o sistema começa a degradar?
- Qual a taxa de requisições por segundo máxima?
- Quando começam a aparecer erros?

## 📊 Relatórios Avançados

### 1. Exportar para JSON
```bash
k6 run --out json=results/result.json scripts/auth-load-test.js
```

### 2. Exportar para InfluxDB + Grafana
```bash
k6 run --out influxdb=http://localhost:8086/k6 scripts/auth-load-test.js
```

### 3. Exportar para CSV
```bash
k6 run --out csv=results/result.csv scripts/auth-load-test.js
```

## 🎯 Metas de Performance

### API REST Típica
- **Latência p(95)**: < 500ms
- **Latência p(99)**: < 1000ms
- **Taxa de erro**: < 1%
- **Throughput**: > 100 req/s
- **Disponibilidade**: > 99.9%

### Colinho da Cá API (Metas)
- **Autenticação**: p(95) < 300ms
- **Listagens**: p(95) < 200ms
- **Cadastros**: p(95) < 500ms
- **Fluxo completo**: p(95) < 3000ms
- **Usuários simultâneos**: > 100
- **Taxa de erro**: < 0.5%

## 🐛 Troubleshooting

### Erro: "connection refused"
```bash
# Verificar se API está rodando
curl http://localhost:5163/api/v1/racas
```

### Erro: "too many open files"
```bash
# Aumentar limite (Linux/Mac)
ulimit -n 10000
```

### Performance inconsistente
- Executar múltiplas vezes
- Usar ambiente isolado
- Desabilitar antivírus temporariamente

## 📚 Recursos

- [Documentação K6](https://k6.io/docs/)
- [K6 Examples](https://k6.io/docs/examples/)
- [K6 Cloud](https://k6.io/cloud/)
- [Grafana K6](https://grafana.com/docs/k6/latest/)

## 🎓 Próximos Passos

1. Integrar com CI/CD
2. Configurar alertas automáticos
3. Criar dashboard Grafana
4. Testes de soak (longa duração)
5. Testes de spike (picos repentinos)
