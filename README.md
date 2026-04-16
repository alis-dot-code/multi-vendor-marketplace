<h1 align="center">🏪 Multi-Vendor Marketplace</h1>

<p align="center">
  <em>A modern, robust, and full-stack multi-vendor marketplace platform built for scale.</em>
</p>

---

## 📖 Introduction

**Multi-Vendor Marketplace** empowers entrepreneurs to launch a fully-featured digital marketplace. It seamlessly connects vendors offering services with buyers looking for quick and reliable bookings. With built-in Stripe payments, dynamic availability scaling, and a powerful admin dashboard, you can build your marketplace with confidence.

---

## ✨ Key Features

- **🛍️ Multi-Vendor Support**: Vendors get their own storefront, dashboard, and management tools.
- **📅 Dynamic Bookings**: Time-slot based booking system with recurring availability.
- **💸 Seamless Payments**: Powered by Stripe Connect for automated splits and secure transactions.
- **🛡️ Dispute & Review Management**: Built-in resolution handling and customer review systems.
- **🔔 Real-time Alerts**: Keep users in the loop with automated email and system notifications.
- **👑 Admin Control**: Comprehensive analytics, user management, and platform settings.

---

## 💻 Technology Stack

### Application Core
* **Backend:** C# / .NET 10 (Clean Architecture & CQRS with MediatR)
* **Frontend:** React 18, TypeScript, Tailwind CSS, Redux Toolkit, React Query
* **Data Layer:** Entity Framework Core 9
* **Integrations:** Stripe.NET / Stripe.js, SendGrid, Cloudinary

### Infrastructure
* **Database:** PostgreSQL 16
* **Cache:** Redis 7
* **Deployment:** Docker & Docker Compose

---

## 🎯 Getting Started

### Prerequisites
Make sure you have installed: `.NET 10 SDK`, `Node.js (v20+)`, `Docker Desktop`, and `Git`.

### 1. Local Infrastructure Setup

Fire up the required databases utilizing Docker:
```bash
git clone https://github.com/FaseehUllahJafar/MarketNest.git
cd MarketNest
docker-compose up -d
```

### 2. Backend Initialization

```bash
# Restore packages & apply migrations
dotnet restore
dotnet ef database update --project src/MarketNest.Infrastructure --startup-project src/MarketNest.API

# Start the API
dotnet run --project src/MarketNest.API
```
> The API will be available at `http://localhost:5000`. You can view the Swagger UI at `http://localhost:5000/swagger`.

### 3. Frontend Initialization

```bash
cd client
npm install
npm run dev
```
> Launch your browser and navigate to `http://localhost:5173`.

---

## 🏗️ Project Architecture

```text
MarketNest/
├── src/
│   ├── MarketNest.Domain/          # Core models, interfaces, enums
│   ├── MarketNest.Application/     # Use cases, DTOs, CQRS Handlers
│   ├── MarketNest.Infrastructure/  # DbContext, Auth, Integrations
│   └── MarketNest.API/             # Controllers, Middlewares
└── client/                          # React + Vite Client Application
    ├── src/
    │   ├── api/                    # Axios instances and API routes
    │   ├── components/             # Reusable UI components
    │   ├── pages/                  # Route views
    │   └── store/                  # Redux configurations
    └── public/
```

---

## 🛡️ Security

* **Authentication:** Handled via JWT Bearer Tokens with expiration and refresh mechanisms.
* **OAuth:** Google Sign-in integrated.
* **Authorization:** Strict Role-Based Access Control (Admin, Vendor, Buyer).
* **Passwords:** Hashed automatically utilizing BCrypt.

---

## 🚀 Deployment

Deploying is straight-forward with Docker:

```bash
# Build the Docker Images
docker build -f src/MarketNest.API/Dockerfile -t marketnest-api:latest .
docker build -f client/Dockerfile -t marketnest-client:latest .

# Run via Production Compose File
docker-compose -f docker-compose.prod.yml up -d
```

> **Note:** Ensure your `.env` file is properly configured with your production keys (Stripe, Cloudinary, SendGrid, etc.).

---

## 💡 Roadmap & Future Updates

- [ ] Interactive PDF Invoicing
- [ ] Push Notifications via WebSockets
- [ ] Advanced User Verification Workflows
- [ ] Multi-currency localization
- [ ] React Native Mobile Application

---

## 📜 License & Contribution

This project is licensed under the **MIT License**.

Contributions are highly encouraged! If you'd like to improve the platform, simply fork the repository, make your changes adhering to conventional commit guidelines, and submit a pull request. Make sure all tests pass before submitting!

<p align="center">Made with ❤️ for the open-source community.</p>
