# MarketNest - Multi-Vendor Marketplace

A production-ready multi-vendor marketplace with booking engine, Stripe Connect payments, vendor storefronts, reviews, admin panel, and email notifications.

## Tech Stack

- **Frontend**: React 18 + TypeScript + Vite + Tailwind CSS
- **Backend**: .NET 10 Web API + Clean Architecture
- **Database**: PostgreSQL 16
- **Cache**: Redis 7
- **Payments**: Stripe Connect
- **Email**: SendGrid
- **Images**: Cloudinary
- **Containerization**: Docker

## Project Structure

```
MarketNest/
├── src/
│   ├── MarketNest.Domain/          # Entities, Enums, Interfaces
│   ├── MarketNest.Application/     # CQRS, DTOs, Validation
│   ├── MarketNest.Infrastructure/  # EF Core, External Services
│   └── MarketNest.API/             # Controllers, Middleware
└── client/                         # React frontend (coming soon)
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker Desktop
- Node.js 20+ (for frontend)

### Run with Docker

```bash
# Copy environment file
cp .env.example .env

# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api
```

### Run Backend Locally

```bash
# Restore dependencies
dotnet restore

# Run migrations (after Prompt 3)
dotnet ef database update --project src/MarketNest.Infrastructure --startup-project src/MarketNest.API

# Run API
dotnet run --project src/MarketNest.API
```

Access Swagger at: `http://localhost:5000/swagger`

### Run Frontend Locally (coming soon)

```bash
cd client
npm install
npm run dev
```

## Environment Variables

See `.env.example` for required environment variables:

- PostgreSQL credentials
- JWT settings
- Stripe API keys
- Cloudinary credentials
- SendGrid API key

## Development Progress

- [x] Prompt 1: Solution Scaffold + Docker Compose
- [x] Prompt 2: Domain Entities + Enums
- [x] Prompt 3: EF Core DbContext + Entity Configurations
- [x] Prompt 4: Repositories + UnitOfWork
- [x] Prompt 5: Application Layer Setup
- [x] Prompt 6: API Program.cs, Middleware, Swagger
- [ ] Prompt 7: Infrastructure DI + Service Interfaces (completed as part of Prompt 6)
- [ ] Prompt 8: Redis Cache Service + Rate Limiting
- [ ] Prompts 9-16: Backend Features, Frontend, Integration, DevOps

## License

MIT
