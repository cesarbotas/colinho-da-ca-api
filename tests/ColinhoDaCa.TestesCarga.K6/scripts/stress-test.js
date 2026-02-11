import http from 'k6/http';
import { check, sleep } from 'k6';

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5163';

export const options = {
  stages: [
    { duration: '2m', target: 100 },   // Ramp-up: 0 -> 100 usuários
    { duration: '5m', target: 100 },   // Estável: 100 usuários
    { duration: '2m', target: 200 },   // Pico: 100 -> 200 usuários
    { duration: '5m', target: 200 },   // Estável: 200 usuários
    { duration: '2m', target: 300 },   // Stress: 200 -> 300 usuários
    { duration: '5m', target: 300 },   // Estável: 300 usuários
    { duration: '2m', target: 0 },     // Ramp-down: 300 -> 0
  ],
  thresholds: {
    http_req_duration: ['p(99)<2000'],
    http_req_failed: ['rate<0.1'],
  },
};

export function setup() {
  const registerPayload = JSON.stringify({
    nome: 'Stress Test User',
    email: `stress${Date.now()}@test.com`,
    celular: '11999999999',
    cpf: '98765432100',
    senha: 'Senha@123'
  });

  const registerRes = http.post(`${BASE_URL}/api/v1/auth/registrar`, registerPayload, {
    headers: { 'Content-Type': 'application/json' },
  });

  const loginPayload = JSON.stringify({
    email: JSON.parse(registerPayload).email,
    senha: 'Senha@123'
  });

  const loginRes = http.post(`${BASE_URL}/api/v1/auth/login`, loginPayload, {
    headers: { 'Content-Type': 'application/json' },
  });

  return { token: loginRes.json('token') };
}

export default function (data) {
  const headers = {
    'Authorization': `Bearer ${data.token}`,
    'Content-Type': 'application/json'
  };

  // Teste de leitura intensiva
  const racasRes = http.get(`${BASE_URL}/api/v1/racas`, { headers });
  
  check(racasRes, {
    'status 200': (r) => r.status === 200,
    'tempo < 2s': (r) => r.timings.duration < 2000,
  });

  sleep(0.5);
}
