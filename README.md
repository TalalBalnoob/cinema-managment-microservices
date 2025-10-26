# 🎬 Cinema Management System (Microservices)

A modular **microservices-based cinema management platform** built with **.NET 9**, **PostgreSQL**, and **Docker Compose**.
Includes user authentication, movie management, showtime scheduling, ticket booking, and payments — all routed through an **API Gateway** using **YARP**.

---

## 🧩 Architecture Overview

Each service runs independently, communicating via REST through the API Gateway.

```
┌─────────────────────────────┐
│         API Gateway         │
│   (YARP Reverse Proxy)      │
└──────────────┬──────────────┘
               │
               ▼
┌──────────────┬──────────────┬──────────────┬──────────────┬──────────────┐
│ Auth Service │ User Service │ Movie Service│ Booking Svc  │ Payment Svc  │
│ (JWT Issuer) │ (Profiles)   │ (Catalog)    │ (Reservations│ (Transactions│
│ .NET + PGSQL │ .NET + PGSQL │ .NET + PGSQL │ .NET + PGSQL │ .NET + PGSQL │
└──────────────┴──────────────┴──────────────┴──────────────┴──────────────┘
```

* **Auth Service**: Handles registration, login, and JWT generation.
* **User Service**: Stores and manages user profiles.
* **Movie Service**: Manages movies, genres, and schedules.
* **Booking Service**: Handles seat selection and reservations.
* **Payment Service**: Simulates payments and stores receipts.
* **API Gateway**: Routes requests and manages traffic between services.

---

## 🛠️ Tech Stack

* **.NET 9** (C#)
* **Entity Framework Core 9**
* **PostgreSQL**
* **YARP (Yet Another Reverse Proxy)**
* **Docker Compose**
* **JWT Authentication**

---

## ⚙️ Local Setup

### 1. Clone the repo

```bash
git clone https://github.com/your-username/cinema-system.git
cd cinema-system
```

### 2. Environment Variables

Create a `.env` file in the project root:

```bash
JWT_KEY=super_secret_key_here
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
```

### 3. Docker Compose

Build and run everything:

```bash
docker compose up --build
```

That spins up:

* All services (`auth`, `movies`, `users`, etc.)
* PostgreSQL databases for each
* The API Gateway on **port 5000**

---

## 🧠 Development Notes

### JWT Setup

* `AuthService` issues tokens using the shared `JWT_KEY`.
* Each microservice validates tokens independently using the same key.
* Public routes → `[AllowAnonymous]`
  Private routes → `[Authorize]`

### Databases

Each service uses its own database and volume for isolation:

```yaml
services:
  movies-db:
    image: postgres:16
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: movies_db
    volumes:
      - movies_pgdata:/var/lib/postgresql/data
```

---

## 🧩 Project Structure

```
/cinema-system
│
├── api-gateway/           # YARP reverse proxy configuration
├── auth-service/          # JWT issuing service
├── movie-service/         # Movie catalog and schedules
├── user-service/          # User profiles and accounts
├── booking-service/       # Seat selection and reservations
├── payment-service/       # Payment and receipt simulation
└── docker-compose.yml
```

---

## 🔑 Example Request

Login to get a JWT:

```bash
curl -X POST http://localhost:5000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@test.com","password":"123456"}'
```

Access a protected route:

```bash
curl -X POST http://localhost:5000/movies \
  -H "Authorization: Bearer <your_jwt_here>" \
  -H "Content-Type: application/json" \
  -d '{"title":"Inception","duration":148,"rating":4.9}'
```

---

## 🧰 Useful Commands

Run EF migrations:

```bash
docker compose exec movies-service dotnet ef database update
```

Rebuild a specific service:

```bash
docker compose up --build movies-service
```

---

## 🚀 Roadmap

* [ ] MicorServices:
  * [X] **API Gateway**
  * [X] **Auth Service**
  * [ ] **Movie Service**
  * [ ] **User Service**
  * [ ] **Booking Service**
  * [ ] **Payment Service**

* [ ] Add RabbitMQ for async communication
* [ ] Implement distributed caching with Redis
* [ ] Add frontend (React + TypeScript)
* [ ] Integrate monitoring (Grafana + Prometheus)

---

## 🧑‍💻 Author

**Talal Balnoob** — Web Developer & Systems Designer

> Building things that actually make sense, one container at a time.
