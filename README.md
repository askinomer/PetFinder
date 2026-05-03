# PetFinder — Lost & Adoption Pet Listings

A university final-term project built with **ASP.NET Core MVC (.NET 8.0)**, **Entity Framework Core**, **Azure SQL Edge (Docker)**, **Cookie Authentication**, and **Bootstrap 5**.

Designed to run natively on **macOS Apple Silicon (M1 / M2 / M3 / M4 Pro)**.

---

## Architecture

```
PetFinder/
├── Program.cs                  # DI, Cookie Auth, EF Core configuration
├── appsettings.json            # SQL Server (Docker) connection string
├── Models/
│   ├── User.cs                 # id, username, password
│   ├── PetAd.cs                # id, name, species, city, description, ImageBytes (VARBINARY MAX)
│   └── ViewModels/LoginViewModel.cs
├── DataAccessLayer/
│   └── AppDbContext.cs         # DbContext + Fluent API + seed user
├── Services/
│   ├── IUserService.cs / UserService.cs
│   └── IPetAdService.cs / PetAdService.cs   # ALL DB ops go through services (LINQ filtering)
├── Controllers/
│   ├── AccountController.cs    # Login / Logout / Register
│   └── PetAdController.cs      # CRUD + Search
├── Views/
│   ├── Shared/_Layout.cshtml   # Bootstrap navbar
│   ├── Account/                # Login, Register, AccessDenied
│   └── PetAd/                  # Index (search), Create, Edit, Details, Delete
├── docker-compose.yml          # Azure SQL Edge (ARM64)
└── PetFinder.csproj
```

---

## Quick Start (macOS Apple Silicon)

### 1) Start the database

```bash
docker compose up -d
```

This launches Azure SQL Edge on `localhost:1433` with SA password `YourStrong@Passw0rd`.

### 2) Restore + run

```bash
cd PetFinder
dotnet restore
dotnet run
```

The app will:
* Auto-create the `PetFinderDb` database on first run (`db.Database.EnsureCreated()`).
* Seed a default admin: **`admin / admin123`**.

Open: `http://localhost:5000` (or whichever URL the console prints).

---

## Connection String (in `appsettings.json`)

```
Server=localhost,1433;Database=PetFinderDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=True;
```

Why these flags matter on Mac/Docker:
* `TrustServerCertificate=True` — the container uses a self-signed cert.
* `Encrypt=False` — Azure SQL Edge does not negotiate TLS the same way as full SQL Server; this avoids handshake errors on .NET 8 (Microsoft.Data.SqlClient defaults changed).
* `MultipleActiveResultSets=True` — needed for nested EF queries.

> **Note:** Azure SQL Edge is the recommended image for ARM64 Macs. Microsoft's standard `mcr.microsoft.com/mssql/server:2022-latest` image is **not** ARM-native and requires Rosetta 2 emulation.

---

## Critical Requirements Mapping

| Requirement                         | Where it's implemented                                             |
| ----------------------------------- | ------------------------------------------------------------------ |
| Cookie-based Authentication         | `Program.cs` (AddAuthentication / AddCookie) + `AccountController` |
| BLOB image as `VARBINARY(MAX)`      | `PetAd.ImageBytes` + `[Column(TypeName="VARBINARY(MAX)")]`         |
| Image displayed via base64 data URI | All views: `data:image/jpeg;base64,@Convert.ToBase64String(...)`   |
| Search via LINQ (species / city)    | `PetAdService.GetAllAsync` (`Where(p => p.Species == species)`)    |
| Strict layered architecture         | Models / DataAccessLayer / Services / Controllers separation       |
| All DB ops via Service layer        | Controllers depend only on `IUserService` / `IPetAdService`        |
| Bootstrap UI                        | `_Layout.cshtml` loads Bootstrap 5 + Bootstrap Icons via CDN       |

---

## Login Flow (Cookie Auth)

1. POST `/Account/Login` → `IUserService.ValidateAsync(username, password)`.
2. Build `ClaimsPrincipal` with `ClaimTypes.Name` + `ClaimTypes.NameIdentifier`.
3. `HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal)` issues `PetFinder.Auth` cookie.
4. `[Authorize]` actions on `PetAdController` (Create / Edit / Delete) become accessible.
5. POST `/Account/Logout` → `SignOutAsync` clears the cookie.

---

## Stop the Database

```bash
docker compose down              # keep data
docker compose down -v           # also delete the volume
```
