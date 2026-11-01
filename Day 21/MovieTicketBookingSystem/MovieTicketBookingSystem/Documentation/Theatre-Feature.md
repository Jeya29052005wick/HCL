# Feature 2 – Theatres Module

## 1. Overview

The Theatres Module manages theatre information in the Movie Ticket Booking System.

It supports creating, retrieving, updating, and deleting theatre records.

**Technology Stack:**
- ASP.NET Core Web API (.NET 10)
- C#
- Entity Framework Core
- SQL Server
- Fluent API
- Data Transfer Objects (DTOs)

## 2. Database Design

**Table: Theatres**

| Column | Data Type | Constraints |
|---|---|---|
| TheatreId | int | Primary Key, Identity |
| Name | nvarchar(150) | NOT NULL |
| Location | nvarchar(200) | NOT NULL |

**Database:** MovieTicketBookingDB

The Theatre entity is configured using Fluent API in `ApplicationDbContext.cs`.

## 3. Project Structure

```text
MovieTicketBookingSystem/
│
├── Controllers/
│   └── TheatresController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── DTOs/
│   └── Theatres/
│       ├── CreateTheatreRequest.cs
│       ├── UpdateTheatreRequest.cs
│       └── TheatreResponse.cs
│
├── Models/
│   └── Theatre.cs
│
├── Repositories/
│   ├── Interfaces/
│   │   └── ITheatreRepository.cs
│   └── TheatreRepository.cs
│
├── Services/
│   ├── Interfaces/
│   │   └── ITheatreService.cs
│   └── TheatreService.cs
│
└── Migrations/
```

## 4. Architecture

The module follows the Controller → Service → Repository → DbContext architecture.

**Request flow:**

1. Controller receives the HTTP request.
2. Service handles business logic and DTO-to-entity conversion.
3. Repository performs database operations.
4. DbContext communicates with SQL Server.
5. Service converts the entity into a response DTO.
6. Controller returns the HTTP response.

## 5. DTOs

### CreateTheatreRequest
Receives theatre details when creating a new theatre.

Properties:
- Name
- Location

Validation:
- Name is required and limited to 150 characters.
- Location is required and limited to 200 characters.

### UpdateTheatreRequest
Receives updated theatre details.

Properties:
- Name
- Location

### TheatreResponse
Returns theatre information to the API client.

Properties:
- TheatreId
- Name
- Location

## 6. API Endpoints

Base URL:

```text
/api/theatres
```

| Method | Endpoint | Description | Expected Status |
|---|---|---|---|
| POST | `/api/theatres` | Create theatre | 201 Created |
| GET | `/api/theatres` | Get all theatres | 200 OK |
| GET | `/api/theatres/{id}` | Get theatre by ID | 200 / 404 |
| PUT | `/api/theatres/{id}` | Update theatre | 200 / 404 |
| DELETE | `/api/theatres/{id}` | Delete theatre | 204 / 404 |

### Pagination

The GET all endpoint supports pagination through query parameters.

Example:

```http
GET /api/theatres?pageNumber=1&pageSize=10
```

## 7. Sample API Requests

### Create Theatre

**POST** `/api/theatres`

Request:

```json
{
    "name": "PVR Cinemas",
    "location": "Coimbatore"
}
```

Expected response:

```json
{
    "theatreId": 1,
    "name": "PVR Cinemas",
    "location": "Coimbatore"
}
```

### Update Theatre

**PUT** `/api/theatres/1`

Request:

```json
{
    "name": "INOX Cinemas",
    "location": "Chennai"
}
```

### Get Theatre by ID

**GET** `/api/theatres/1`

### Delete Theatre

**DELETE** `/api/theatres/1`

## 8. Dependency Injection

The following services are registered in `Program.cs`:

```csharp
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
```

`AddScoped` creates one instance of the registered service for each HTTP request scope.

## 9. Database Migration

Migration commands used:

```bash
dotnet ef migrations add AddTheatres
dotnet ef database update
```

The migration adds the Theatres table to the existing MovieTicketBookingDB database.

## 10. Testing Checklist

- [ ] Create a theatre using POST.
- [ ] Retrieve all theatres using GET.
- [ ] Retrieve a theatre using its ID.
- [ ] Update theatre details using PUT.
- [ ] Delete a theatre using DELETE.
- [ ] Verify that an invalid theatre ID returns 404.
- [ ] Verify that required-field validation works.

## 11. Important Concepts Learned

- Entity Framework Core DbSet
- Fluent API configuration
- DTOs and request validation
- Repository Pattern
- Service Layer
- Dependency Injection
- Constructor Injection
- Async and Await
- Pagination
- HTTP status codes
- CRUD operations

## 12. Next Module

**Feature 3 – Screens Module**

The Screens Module will manage screens belonging to theatres. Each screen will be associated with a Theatre through a foreign key relationship.

