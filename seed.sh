#!/bin/bash
# Seed script: Kullanıcılar ve ürünler oluşturur
# Kullanım: ./seed.sh

BASE_URL="http://localhost:5100"

echo "=== YAZLAB-V2 Seed Script ==="
echo ""

# --- KULLANICILAR ---
echo ">> Kullanıcılar oluşturuluyor..."

register() {
  curl -s -o /dev/null -w "%{http_code}" -X POST "$BASE_URL/auth/register" \
    -H "Content-Type: application/json" \
    -d "{\"email\":\"$1\",\"password\":\"$2\"}"
}

register "admin@yazlab.com"    "Admin123!"
register "test@test.com"       "123456"
register "ahmet@yazlab.com"    "Ahmet123!"
register "mehmet@yazlab.com"   "Mehmet123!"
register "ayse@yazlab.com"     "Ayse123!"
register "fatma@yazlab.com"    "Fatma123!"
register "ali@yazlab.com"      "Ali123!"
register "veli@yazlab.com"     "Veli123!"
register "zeynep@yazlab.com"   "Zeynep123!"
register "elif@yazlab.com"     "Elif123!"

echo ""
echo ">> Login token alınıyor..."
TOKEN=$(curl -s -X POST "$BASE_URL/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@yazlab.com","password":"Admin123!"}' | grep -o '"token":"[^"]*"' | cut -d'"' -f4)

if [ -z "$TOKEN" ]; then
  echo "HATA: Token alınamadı. Sistem çalışıyor mu? (docker compose up)"
  exit 1
fi

echo "Token alındı."
echo ""

# --- ÜRÜNLER ---
echo ">> Ürünler oluşturuluyor..."

add_product() {
  curl -s -o /dev/null -w "%{http_code}" -X POST "$BASE_URL/api/products" \
    -H "Content-Type: application/json" \
    -H "Authorization: Bearer $TOKEN" \
    -d "{\"name\":\"$1\",\"price\":$2,\"stock\":$3,\"category\":\"$4\"}"
}

# Elektronik
add_product "Apple MacBook Pro 14 M3"       54999  15  "Elektronik"
add_product "Apple MacBook Air M2"           34999  25  "Elektronik"
add_product "Dell XPS 15"                    42999  10  "Elektronik"
add_product "Lenovo ThinkPad X1 Carbon"      38999  12  "Elektronik"
add_product "HP Spectre x360"                29999  20  "Elektronik"
add_product "ASUS ROG Zephyrus G14"          35999   8  "Elektronik"
add_product "Microsoft Surface Pro 9"        27999  18  "Elektronik"
add_product "Samsung Galaxy Book 3 Pro"      24999  22  "Elektronik"

# Telefon
add_product "iPhone 15 Pro Max 256GB"        54999  30  "Telefon"
add_product "iPhone 15 128GB"                39999  45  "Telefon"
add_product "Samsung Galaxy S24 Ultra"       44999  28  "Telefon"
add_product "Samsung Galaxy S24"             29999  35  "Telefon"
add_product "Google Pixel 8 Pro"             31999  15  "Telefon"
add_product "OnePlus 12"                     22999  20  "Telefon"
add_product "Xiaomi 14 Pro"                  24999  18  "Telefon"

# Tablet
add_product "iPad Pro 12.9 M2"               32999  20  "Tablet"
add_product "iPad Air 5. Nesil"              18999  30  "Tablet"
add_product "Samsung Galaxy Tab S9 Ultra"    28999  12  "Tablet"
add_product "Microsoft Surface Pro 9"        26999  15  "Tablet"

# Aksesuar
add_product "Apple AirPods Pro 2. Nesil"      7999  50  "Aksesuar"
add_product "Sony WH-1000XM5 Kulaklık"        8999  40  "Aksesuar"
add_product "Apple Watch Series 9 45mm"      13999  35  "Aksesuar"
add_product "Samsung Galaxy Watch 6"          8499  30  "Aksesuar"
add_product "Logitech MX Master 3S"           3499  60  "Aksesuar"
add_product "Apple Magic Keyboard"            3299  45  "Aksesuar"

echo ""
echo "=== Seed tamamlandı! ==="
echo "Ürün ve kullanıcılar başarıyla oluşturuldu."
echo ""
echo "Test kullanıcısı: test@test.com / 123456"
echo "Admin kullanıcısı: admin@yazlab.com / Admin123!"
