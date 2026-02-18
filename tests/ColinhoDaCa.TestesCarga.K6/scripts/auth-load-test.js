import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

const errorRate = new Rate('errors');
const BASE_URL = __ENV.BASE_URL || 'http://localhost:5163';

export const options = {
  stages: [
    { duration: '30s', target: 10 },  // Ramp-up: 0 -> 10 usuários em 30s
    { duration: '1m', target: 10 },   // Estável: 10 usuários por 1min
    { duration: '30s', target: 50 },  // Pico: 10 -> 50 usuários em 30s
    { duration: '1m', target: 50 },   // Estável: 50 usuários por 1min
    { duration: '30s', target: 0 },   // Ramp-down: 50 -> 0 usuários em 30s
  ],
  thresholds: {
    http_req_duration: ['p(95)<500', 'p(99)<1000'], // 95% < 500ms, 99% < 1s
    http_req_failed: ['rate<0.05'],                  // Taxa de erro < 5%
    errors: ['rate<0.1'],                            // Taxa de erro customizada < 10%
  },
};

export function setup() {
  // Registrar usuário de teste
  const registerPayload = JSON.stringify({
    nome: 'Load Test User',
    email: `loadtest${Date.now()}@test.com`,
    celular: '11999999999',
    cpf: '12345678901',
    senha: 'Senha@123'
  });

  const registerRes = http.post(`${BASE_URL}/api/v1/auth/registrar`, registerPayload, {
    headers: { 'Content-Type': 'application/json' },
  });

  check(registerRes, {
    'registro bem-sucedido': (r) => r.status === 200,
  });

  return { email: JSON.parse(registerPayload).email, senha: 'Senha@123' };
}

export default function (data) {
  // Login
  const loginPayload = JSON.stringify({
    email: data.email,
    senha: data.senha
  });

  const loginRes = http.post(`${BASE_URL}/api/v1/auth/login`, loginPayload, {
    headers: { 'Content-Type': 'application/json' },
  });

  const loginSuccess = check(loginRes, {
    'login status 200': (r) => r.status === 200,
    'token presente': (r) => r.json('token') !== undefined,
  });

  errorRate.add(!loginSuccess);

  if (!loginSuccess) {
    return;
  }

  const token = loginRes.json('token');

  // Listar Raças
  const racasRes = http.get(`${BASE_URL}/api/v1/racas`, {
    headers: { 
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
  });

  const racasSuccess = check(racasRes, {
    'listar raças status 200': (r) => r.status === 200,
    'raças retornadas': (r) => r.json().length > 0,
  });

  errorRate.add(!racasSuccess);

  sleep(1);
}

export function teardown(data) {
  console.log('Teste de carga finalizado');
}
