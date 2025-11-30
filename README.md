# Firmeza Project

## 1. Project Overview
Firmeza is a comprehensive construction supply and industrial vehicle rental system. It consists of:
- **Admin Module**: A Razor Pages application for managing products, customers, and sales.
- **API**: A RESTful API built with .NET 8, featuring JWT authentication and Swagger documentation.
- **Client Module**: A React-based frontend for customers to browse products and make purchases.
- **Deployment**: Fully containerized using Docker and Docker Compose.

### Key Features
- **Authentication**: Secure Identity-based login for Admins and JWT-based auth for Customers.
- **Product Management**: Full CRUD operations for inventory.
- **Sales Processing**: Real-time stock validation and sales tracking.
- **Reporting**: Excel/PDF export capabilities (planned).
- **Modern Tech Stack**: .NET 8, PostgreSQL, React, Docker.

## 2. Technologies Used

| Category | Technologies |
|----------|--------------|
| **Backend** | .NET 8, ASP.NET Core, Entity Framework Core |
| **Frontend** | React (Vite), TailwindCSS (planned) |
| **Database** | PostgreSQL |
| **Auth** | ASP.NET Core Identity, JWT Bearer |
| **Testing** | xUnit, Moq |
| **Deployment** | Docker, Docker Compose |

## 3. Setup Instructions

### Prerequisites
- .NET 8 SDK
- Node.js (v18+)
- Docker & Docker Compose
- PostgreSQL (if running locally without Docker)

### Local Setup

1.  **Clone the repository**
    ```bash
    git clone https://github.com/ApoloKn/Firmeza_.git
    cd Firmeza
    ```

2.  **Backend Setup**
    ```bash
    cd Firmeza.Api
    dotnet restore
    # Update connection string in appsettings.json if needed
    dotnet ef database update
    dotnet run
    ```

3.  **Frontend Setup**
    ```bash
    cd ../Firmeza.Client
    npm install
    npm run dev
    ```

### Docker Setup (Recommended)

To spin up the entire environment (Database, API, Admin, Client):

```bash
docker-compose up --build
```

> **Note**: The `docker-compose.yml` is configured to run tests first. If tests fail, the deployment will stop.

## 4. Project Structure

```
Firmeza/
├── Firmeza.Admin/       # (Merged into Firmeza.Web) Razor Pages admin module
├── Firmeza.Api/         # RESTful API & Business Logic
├── Firmeza.Client/      # Frontend (React)
├── Firmeza.Data/        # Shared Data Models & Context
├── Firmeza.Tests/       # xUnit Tests
├── Firmeza.Web/         # Admin Web Interface
├── docker-compose.yml   # Docker orchestration
└── README.md            # Project documentation
```

## 5. API Documentation

The API is documented using Swagger. Once running, visit:
- **Local**: `http://localhost:5000/swagger`
- **Docker**: `http://localhost:5000/swagger`

### Key Endpoints
- `POST /api/auth/login`: Authenticate as Customer.
- `GET /api/products`: List available products.
- `POST /api/sales`: Create a new sale (requires Auth).

## 6. Testing

We use xUnit for backend testing.

**Run tests:**
```bash
cd Firmeza.Tests
dotnet test
```

**Test Coverage:**
- Authentication Logic
- Email Service
- Sale Service (Stock validation, Totals)

## 7. Deployment

Deployment is handled via Docker Compose.

```bash
docker-compose up -d
```

### Environment Variables
- `ConnectionStrings__DefaultConnection`: PostgreSQL connection string.
- `Jwt__Key`: Secret key for token generation.
- `Jwt__Issuer`: Token issuer.
- `Jwt__Audience`: Token audience.

## 8. Troubleshooting

- **Database Connection Failed**: Ensure PostgreSQL is running and credentials in `appsettings.json` match.
- **Docker Build Fails**: Check if `dotnet test` passes locally. Docker build aborts on test failure.
- **Frontend Errors**: Ensure `npm install` was run successfully.

## 9. License

Distributed under the MIT License. See `LICENSE` for more information.
