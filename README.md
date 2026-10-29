# IdentityDemo

![CI](https://github.com/WayoGFX/IdentityDemo/actions/workflows/ci.yml/badge.svg)

API in ASP.NET Core 9 with ASP.NET Core Identity and JWT authentication, role-based access control (RBAC), account lockout protection, and persistence in SQL Server with Entity Framework Core.

## Features

- **User Management**: Registration and login using ASP.NET Core Identity (`UserManager<IdentityUser>`, `SignInManager<IdentityUser>`).
- **Security & Password Policy**: Configured password rules (length, digit requirement) and account lockout protection after 5 consecutive failed attempts (15-minute lockout).
- **JWT Authentication**: Token generation (HS256) with custom claims (`NameIdentifier`, `Name`, `Email`, `Role`) and validation of issuer, audience, lifetime, and signing key.
- **Role-Based Authorization**: Protected endpoints using `[Authorize(Roles = "Admin")]`.
- **Automatic Role Seeding**: Startup seeder (`RoleSeeder`) initializing default roles (`Admin`, `User`).
- **API Documentation**: Integrated OpenAPI specification with modern interactive Scalar UI (`Scalar.AspNetCore`).
- **Persistence**: Code-First migrations with Entity Framework Core and SQL Server (`IdentityDbContext<IdentityUser>`).
- **CI Pipeline**: Automated build and test workflow configured with GitHub Actions for pushes and pull requests to `main`.

## Endpoints

| Method | Endpoint | Authorization | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | Public | Register a new user |
| `POST` | `/api/auth/login` | Public | Authenticate user and return JWT token |
| `GET` | `/api/admin/panel` | `[Authorize(Roles = "Admin")]` | Protected panel restricted to Admin role |

## Stack

C# (.NET 9) · ASP.NET Core · ASP.NET Core Identity · Entity Framework Core · SQL Server · Scalar OpenAPI · GitHub Actions

## How to run locally

```bash
git clone https://github.com/WayoGFX/IdentityDemo.git
cd IdentityDemo

# 1. Configure appsettings.Development.json with your JWT secret and connection string
# 2. Apply EF Core database migrations
dotnet ef database update --project IdentityDemo.Api

# 3. Run the API
dotnet run --project IdentityDemo.Api
```

Once running in development mode, access the interactive Scalar API documentation at:
```
https://localhost:<port>/scalar/v1
```

## Running tests

```bash
dotnet test
```
