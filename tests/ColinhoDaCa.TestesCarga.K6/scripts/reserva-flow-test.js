import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Trend } from 'k6/metrics';

const errorRate = new Rate('errors');
const reservaDuration = new Trend('reserva_duration');
const BASE_URL = __ENV.BASE_URL || 'http://localhost:5163';

export const options = {
  scenarios: {
    fluxo_completo: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '1m', target: 20 },
        { duration: '2m', target: 20 },
        { duration: '1m', target: 0 },
      ],
      gracefulRampDown: '30s',
    },
  },
  thresholds: {
    http_req_duration: ['p(95)<1000'],
    http_req_failed: ['rate<0.05'],
    errors: ['rate<0.1'],
    reserva_duration: ['p(95)<3000'],
  },
};

function gerarCPF() {
  const n = Math.floor(Math.random() * 999999999) + 1;
  return String(n).padStart(11, '0');
}

export default function () {
  const timestamp = Date.now();
  const cpf = gerarCPF();

  // 1. Registrar
  const registerPayload = JSON.stringify({
    nome: `Cliente ${timestamp}`,
    email: `cliente${timestamp}@test.com`,
    celular: '11999999999',
    cpf: cpf,
    senha: 'Senha@123'
  });

  const registerRes = http.post(`${BASE_URL}/api/v1/auth/registrar`, registerPayload, {
    headers: { 'Content-Type': 'application/json' },
  });

  if (!check(registerRes, { 'registro ok': (r) => r.status === 200 })) {
    errorRate.add(1);
    return;
  }

  // 2. Login
  const loginPayload = JSON.stringify({
    email: `cliente${timestamp}@test.com`,
    senha: 'Senha@123'
  });

  const loginRes = http.post(`${BASE_URL}/api/v1/auth/login`, loginPayload, {
    headers: { 'Content-Type': 'application/json' },
  });

  if (!check(loginRes, { 'login ok': (r) => r.status === 200 })) {
    errorRate.add(1);
    return;
  }

  const token = loginRes.json('token');
  const headers = {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  };

  // 3. Listar Clientes
  const clientesRes = http.get(`${BASE_URL}/api/v1/clientes?numeroPagina=1&quantidadeRegistros=10`, {
    headers: headers,
  });

  check(clientesRes, { 'listar clientes ok': (r) => r.status === 200 });

  // 4. Listar Raças
  const racasRes = http.get(`${BASE_URL}/api/v1/racas`, {
    headers: headers,
  });

  if (!check(racasRes, { 'listar raças ok': (r) => r.status === 200 })) {
    errorRate.add(1);
    return;
  }

  const racas = racasRes.json();
  const racaId = racas[0].id;

  // 5. Cadastrar Pet
  const petPayload = JSON.stringify({
    nome: `Pet ${timestamp}`,
    racaId: racaId,
    idade: 3,
    peso: 10.5,
    porte: 'M',
    observacoes: 'Pet de teste de carga',
    clienteId: 1
  });

  const petRes = http.post(`${BASE_URL}/api/v1/pets`, petPayload, {
    headers: headers,
  });

  check(petRes, { 'cadastrar pet ok': (r) => r.status === 200 });

  // 6. Criar Reserva
  const startReserva = Date.now();
  
  const reservaPayload = JSON.stringify({
    clienteId: 1,
    usuarioId: 1,
    dataInicial: new Date(Date.now() + 86400000).toISOString(),
    dataFinal: new Date(Date.now() + 172800000).toISOString(),
    quantidadeDiarias: 2,
    quantidadePets: 1,
    valorTotal: 200.00,
    observacoes: 'Reserva de teste de carga',
    petIds: [1]
  });

  const reservaRes = http.post(`${BASE_URL}/api/v1/reservas`, reservaPayload, {
    headers: headers,
  });

  const reservaSuccess = check(reservaRes, { 
    'criar reserva ok': (r) => r.status === 200 
  });

  reservaDuration.add(Date.now() - startReserva);
  errorRate.add(!reservaSuccess);

  sleep(1);
}
