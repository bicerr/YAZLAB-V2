// Prometheus Remote Write ile çalıştırmak için:
// K6_PROMETHEUS_RW_SERVER_URL=http://localhost:9090/api/v1/write k6 run --out experimental-prometheus-rw load-test.js

import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '30s', target: 50 },
    { duration: '30s', target: 100 },
    { duration: '30s', target: 200 },
    { duration: '30s', target: 500 },
    { duration: '30s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<500'],
    http_req_failed: ['rate<0.05'],
  },
};

const BASE_URL = 'http://localhost:5100';

const TEST_USERS = [
  { email: 'test@test.com',       password: '123456' },
  { email: 'ahmet@yazlab.com',    password: 'Ahmet123!' },
  { email: 'mehmet@yazlab.com',   password: 'Mehmet123!' },
  { email: 'ayse@yazlab.com',     password: 'Ayse123!' },
  { email: 'fatma@yazlab.com',    password: 'Fatma123!' },
  { email: 'ali@yazlab.com',      password: 'Ali123!' },
  { email: 'veli@yazlab.com',     password: 'Veli123!' },
  { email: 'zeynep@yazlab.com',   password: 'Zeynep123!' },
  { email: 'elif@yazlab.com',     password: 'Elif123!' },
];

export function setup() {
  // Admin token al
  const loginRes = http.post(`${BASE_URL}/auth/login`, JSON.stringify({
    email: 'admin@yazlab.com',
    password: 'Admin123!',
  }), { headers: { 'Content-Type': 'application/json' } });

  const token = JSON.parse(loginRes.body).token;

  // Ürünleri çek
  const productsRes = http.get(`${BASE_URL}/api/products`, {
    headers: { 'Authorization': `Bearer ${token}` },
  });

  let productIds = [];
  try {
    const products = JSON.parse(productsRes.body);
    productIds = products.map(p => p.id).filter(id => id);
  } catch (_) {}

  return { token, productIds };
}

export default function (data) {
  // Her VU rastgele bir kullanıcı ve ürün seçer
  const user = TEST_USERS[__VU % TEST_USERS.length];

  // Login
  const loginRes = http.post(`${BASE_URL}/auth/login`, JSON.stringify({
    email: user.email,
    password: user.password,
  }), { headers: { 'Content-Type': 'application/json' } });

  check(loginRes, { 'login 200': (r) => r.status === 200 });

  let token = data.token;
  try {
    token = JSON.parse(loginRes.body).token || data.token;
  } catch (_) {}

  const headers = {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`,
  };

  // Ürünleri listele
  const productsRes = http.get(`${BASE_URL}/api/products`, { headers });
  check(productsRes, {
    'products 200': (r) => r.status === 200,
    'products < 500ms': (r) => r.timings.duration < 500,
  });

  // Rastgele bir ürün ID seç
  const productIds = data.productIds;
  if (productIds && productIds.length > 0) {
    const productId = productIds[Math.floor(Math.random() * productIds.length)];

    // Sipariş oluştur
    const orderRes = http.post(`${BASE_URL}/api/orders`, JSON.stringify({
      productId: productId,
      quantity: Math.floor(Math.random() * 3) + 1,
      customerEmail: user.email,
    }), { headers });
    check(orderRes, {
      'order 201': (r) => r.status === 201,
      'order < 500ms': (r) => r.timings.duration < 500,
    });

    // Zaman zaman ödeme oluştur (önce sipariş ID'sini al)
    if (__ITER % 3 === 0 && orderRes.status === 201) {
      let orderId = null;
      try { orderId = JSON.parse(orderRes.body).id; } catch (_) {}
      if (orderId) {
        const paymentRes = http.post(`${BASE_URL}/api/payments`, JSON.stringify({
          orderId: orderId,
          amount: Math.floor(Math.random() * 5000) + 500,
          paymentMethod: 'credit_card',
        }), { headers });
        check(paymentRes, { 'payment 201': (r) => r.status === 201 });
      }
    }
  }

  // Bildirim oluştur
  if (__ITER % 5 === 0) {
    http.post(`${BASE_URL}/api/notifications`, JSON.stringify({
      userId: user.email,
      message: `Siparişiniz alındı - ${new Date().toISOString()}`,
      type: 'order',
    }), { headers });
  }

  sleep(1);
}
