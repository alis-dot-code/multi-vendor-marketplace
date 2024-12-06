# MarketNest - Multi-Vendor Marketplace

A production-ready multi-vendor marketplace platform with booking engine, Stripe Connect payments, vendor storefronts, reviews, admin panel, and email notifications.

## 🎯 Overview

MarketNest is a full-stack marketplace application enabling vendors to list services, manage bookings, and accept payments. Buyers can discover services, make reservations, and leave reviews. Admins oversee platform operations, manage disputes, and monitor analytics.

## 🛠️ Tech Stack

### Backend
- **.NET 10** - Modern C# framework with Clean Architecture
- **Entity Framework Core 9** - ORM for PostgreSQL
- **MediatR** - CQRS pattern implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Request validation
- **JWT Bearer** - Authentication
- **Stripe.NET** - Payment processing
- **SendGrid** - Email service
- **Cloudinary** - Image management

### Frontend
- **React 18** - UI library
- **TypeScript** - Type safety
- **Vite** - Build tool
- **Tailwind CSS** - Styling
- **Redux Toolkit** - State management
- **React Query** - Data fetching
- **Axios** - HTTP client
- **React Router v6** - Navigation
- **Stripe.js** - Payment UI
- **Lucide React** - Icons

### Infrastructure
- **PostgreSQL 16** - Database
- **Redis 7** - Caching
- **Docker** - Containerization
- **Docker Compose** - Orchestration

## 📁 Project Structure

```
MarketNest/
├── src/
│   ├── MarketNest.Domain/              # Entity definitions, enums, repository interfaces
│   ├── MarketNest.Application/         # CQRS handlers, DTOs, business logic
│   ├── MarketNest.Infrastructure/      # EF Core, external services, background jobs
│   └── MarketNest.API/                 # REST controllers, middleware, API configuration
└── client/                              # React frontend (Vite + TypeScript + Tailwind)
    ├── src/
    │   ├── api/                        # API service layer
    │   ├── components/                 # Reusable UI components
    │   ├── hooks/                      # Custom React hooks
    │   ├── pages/                      # Page components
    │   ├── store/                      # Redux slices
    │   ├── types/                      # TypeScript type definitions
    │   └── utils/                      # Helper functions
    └── public/                         # Static assets
```

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- Node.js 20+ and npm
- Docker Desktop
- Git

### Backend Setup

```bash
# Clone repository
git clone https://github.com/FaseehUllahJafar/MarketNest.git
cd MarketNest

# Start Docker services (PostgreSQL, Redis)
docker-compose up -d

# Install/restore .NET dependencies
dotnet restore

# Apply database migrations
dotnet ef database update --project src/MarketNest.Infrastructure --startup-project src/MarketNest.API

# Run API (port 5000)
dotnet run --project src/MarketNest.API
```

Access Swagger documentation: `http://localhost:5000/swagger`

### Frontend Setup

```bash
# Navigate to client folder
cd client

# Install dependencies
npm install

# Start dev server (port 5173)
npm run dev

# Build for production
npm run build
```

Access frontend: `http://localhost:5173`

## 📋 API Endpoints

### Authentication
- `POST /api/v1/auth/register` - User registration
- `POST /api/v1/auth/login` - Login
- `POST /api/v1/auth/google` - Google OAuth
- `POST /api/v1/auth/refresh` - Refresh token
- `GET /api/v1/auth/me` - Current user profile

### Vendors
- `POST /api/v1/vendors/apply` - Apply as vendor
- `GET /api/v1/vendors/{slug}` - Public vendor profile
- `GET /api/v1/vendors/me/dashboard` - Vendor dashboard
- `POST /api/v1/vendors/{id}/approve` - [Admin] Approve vendor

### Listings
- `GET /api/v1/listings` - Search listings
- `POST /api/v1/listings` - [Vendor] Create listing
- `POST /api/v1/listings/{id}/images` - Upload listing images
- `POST /api/v1/listings/{id}/availability` - Create availability slots

### Bookings
- `POST /api/v1/bookings` - Create booking
- `GET /api/v1/bookings/my` - User bookings
- `POST /api/v1/bookings/{id}/confirm` - [Vendor] Confirm booking
- `GET /api/v1/bookings/export/ical` - Export calendar

### Payments
- `POST /api/v1/payments/create-intent` - Create Stripe payment intent
- `POST /api/v1/payments/{id}/refund` - [Admin] Process refund

### Reviews
- `POST /api/v1/reviews` - Create review (post-booking)
- `GET /api/v1/reviews/listing/{id}` - Get listing reviews
- `POST /api/v1/reviews/{id}/reply` - [Vendor] Reply to review

### Disputes
- `POST /api/v1/disputes` - Open dispute
- `GET /api/v1/disputes` - [Admin] View all disputes
- `POST /api/v1/disputes/{id}/resolve` - [Admin] Resolve dispute

### Admin
- `GET /api/v1/admin/analytics` - Platform analytics
- `GET /api/v1/admin/users` - List users
- `GET /api/v1/admin/settings` - Platform settings

### Notifications
- `GET /api/v1/notifications` - User notifications
- `POST /api/v1/notifications/{id}/read` - Mark notification read
- `GET /api/v1/notifications/unread-count` - Unread count

## 🗄️ Database Schema

### Key Tables
- **users** - Registered users (Buyer, Vendor, Admin roles)
- **vendors** - Vendor profiles with Stripe integration
- **listings** - Service listings with pricing and availability
- **availability_slots** - Time slots for services
- **bookings** - Customer bookings with status tracking
- **payments** - Payment records with Stripe integration
- **reviews** - Service reviews with vendor replies
- **disputes** - Booking disputes with resolution handling
- **notifications** - User notifications (system and email)
- **categories** - Service categories (hierarchical)

## 🔐 Authentication & Authorization

- **JWT Bearer tokens** for API authentication
- **Role-based access control** (Buyer, Vendor, Admin)
- **Refresh tokens** with rotation (expires 7 days)
- **Google OAuth** integration
- **Password hashing** with BCrypt

## 💳 Payment Processing

- **Stripe Connect** for vendor onboarding
- **Payment Intent API** for booking payments
- **Platform fee calculation** (configurable percentage)
- **Automatic splits** between platform and vendor
- **Webhook handling** for payment confirmations and refunds
- **Refund processing** with admin controls

## 📧 Notifications

- **Email notifications** via SendGrid
- **Database notifications** stored and tracked
- **Notification types**: BookingCreated, BookingConfirmed, ReviewReceived, VendorApproved, DisputeOpened, etc.
- **Background jobs** for scheduled reminders
- **Mark read** functionality for UI

## 🎨 Frontend Features

### Public Pages
- Home with featured listings
- Search & filter by category, price, location
- Listing detail with reviews and vendor info
- Vendor storefront

### Buyer Features
- Dashboard with upcoming bookings
- Browse and book services
- Time slot selection
- Stripe payment checkout
- Leave reviews and ratings
- Open disputes for issues
- Manage bookings and cancel orders

### Vendor Features
- Application and approval workflow
- Vendor dashboard with analytics
- Create and manage listings
- Upload listing images (Cloudinary)
- Availability management (single/recurring slots)
- View and confirm bookings
- Mark bookings complete
- Reply to reviews
- Export bookings to iCal
- Stripe Connect onboarding
- Earnings tracking

### Admin Features
- Approve/reject vendor applications
- View platform analytics (GMV, revenue, bookings)
- Manage categories
- Manage user accounts
- Resolve disputes
- Configure platform settings
- View system notifications

## 🔄 Key Workflows

### Booking Workflow
1. Buyer searches listings
2. Selects service and time slot
3. Creates booking (Pending or Confirmed based on vendor settings)
4. Makes payment via Stripe
5. Vendor confirms booking (if manual confirmation)
6. Service completion
7. Buyer leaves review
8. Vendor replies to review (optional)

### Vendor Onboarding
1. User applies to become vendor
2. Admin approves application
3. Vendor completes Stripe Connect setup
4. Vendor can create listings
5. Listings go live after approval

### Dispute Resolution
1. Buyer opens dispute for booking
2. Admin reviews dispute details
3. Admin determines resolution (refund, denial)
4. Refund processed if applicable
5. Both parties notified


## 🧪 Testing

Currently includes:
- Domain entity definitions with validation rules
- CQRS handler specifications
- Repository interface contracts
- API endpoint contracts (Swagger)

Future additions:
- Unit tests for business logic
- Integration tests for API endpoints
- E2E tests for critical workflows

## 🚢 Deployment

### Docker Production Build

```bash
# Build images
docker build -f src/MarketNest.API/Dockerfile -t marketnest-api:1.0 .
docker build -f client/Dockerfile -t marketnest-client:1.0 .

# Run production compose
docker-compose -f docker-compose.prod.yml up -d
```

### Environment Variables

Key environment variables (see `.env.example`):
- `ASPNETCORE_ENVIRONMENT` - Production/Development
- `ConnectionStrings__DefaultConnection` - PostgreSQL connection
- `ConnectionStrings__Redis` - Redis connection
- `JwtSettings__Key` - JWT signing key
- `StripeSettings__SecretKey` - Stripe API key
- `SendGridSettings__ApiKey` - SendGrid API key
- `CloudinarySettings__CloudName` - Cloudinary account
- `VITE_STRIPE_PUBLISHABLE_KEY` - Frontend Stripe key
- `VITE_GOOGLE_CLIENT_ID` - Google OAuth credentials

## 📚 API Documentation

Full Swagger documentation available at:
```
http://localhost:5000/swagger
```

## 🐛 Known Issues & Future Enhancements

### Known Limitations
- PDF invoice generation not yet implemented
- SMS notifications not integrated
- Advanced analytics dashboard needs enhancement
- Real-time notifications (WebSocket) not yet implemented

### Planned Features
- User profile verification
- Vendor rating system refinements
- Advanced search filters (availability, response time)
- Recommendation engine
- Multi-currency support
- Mobile app (React Native)
- Advanced dispute resolution workflow
- Vendor analytics dashboard enhancements
- Rate limiting per user
- DDoS protection

## 📄 License

MIT License - see LICENSE file for details

## 👨‍💻 Author

Created as a comprehensive marketplace platform demonstrating:
- Clean Architecture principles
- CQRS pattern implementation
- Full-stack development (C# + React)
- Stripe/payment integration
- Email service integration
- Background job processing
- Docker containerization
- Modern web development practices

## 🤝 Contributing

Pull requests welcome! Please ensure:
- Code follows conventions
- Tests pass
- Documentation is updated
- Commits follow conventional commit format