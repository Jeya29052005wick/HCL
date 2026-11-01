# Feature 3 – Screens Module

## 1. Overview

The Screens Module manages cinema screens within theatres in the Movie Ticket Booking System.

A theatre can contain multiple screens, but each screen belongs to one theatre.

The module supports:
- Creating screens.
- Retrieving all screens.
- Retrieving a screen by ID.
- Retrieving screens belonging to a specific theatre.
- Updating screen details.
- Deleting screens.

**Technology Stack:**
- ASP.NET Core Web API (.NET 10)
- C#
- Entity Framework Core 10
- SQL Server
- Fluent API
- Data Transfer Objects (DTOs)

## 2. Database Design

**Table: Screens**

| Column | Data Type | Constraints |
|---|---|---|
| ScreenId | int | Primary Key, Identity |
| TheatreId | int | Foreign Key |
| ScreenName | nvarchar(50) | NOT NULL |

**Database:** MovieTicketBookingDB

### Constraints

- ScreenId is the primary key.
- TheatreId references Theatres(TheatreId).
- ScreenName is required.
- ScreenName is limited to 50 characters.
- Unique constraint on (TheatreId, ScreenName).

The unique constraint prevents duplicate screen names within the same theatre. Different theatres can have screens with the same name.

## 3. Entity Relationship

**Theatre → Screens (One-to-Many)**

Example:

```text
PVR Cinemas
    |
    |---- Screen 1
    |
    |---- Screen 2
    |
    |---- Screen 3
```

One theatre can have multiple screens. Each screen is associated with one theatre using TheatreId.

## 4. Project Structure

```text
MovieTicketBookingSystem/
│
├── Controllers/
│   └── ScreensController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── DTOs/
│   └── Screens/
│       ├── CreateScreenRequest.cs
│       ├── UpdateScreenRequest.cs
│       └── ScreenResponse.cs
│
├── Models/
│   └── Screen.cs
│
├── Repositories/
│   ├── Interfaces/
│   │   └── IScreenRepository.cs
│   └── ScreenRepository.cs
│
├── Services/
│   ├── Interfaces/
│   │   └── IScreenService.cs
│   └── ScreenService.cs
│
└── Migrations/
```

## 5. Model

### Screen.cs

Represents a physical screen within a theatre.

Properties:

- ScreenId
- TheatreId
- ScreenName
- Theatre (Navigation Property)

The Theatre navigation property represents the relationship with the Theatre entity.

## 6. DTOs

### CreateScreenRequest

Receives information when creating a screen.

Properties:
- TheatreId
- ScreenName

Validation:
- TheatreId must be greater than zero.
- ScreenName is required.
- ScreenName must not exceed 50 characters.

### UpdateScreenRequest

Receives updated screen information.

Properties:
- TheatreId
- ScreenName

### ScreenResponse

Returns screen information to the API client.

Properties:
- ScreenId
- TheatreId
- ScreenName

## 7. Entity Framework Core Configuration

The Screen entity is configured using Fluent API inside `ApplicationDbContext.cs`.

Important configurations:

- Primary key configuration.
- Identity column configuration.
- Required ScreenName property.
- Maximum length of 50 characters.
- Theatre foreign key relationship.
- Restrictive delete behavior.
- Unique composite index on TheatreId and ScreenName.

### Relationship Configuration

```csharp
entity.HasOne(s => s.Theatre)
      .WithMany()
      .HasForeignKey(s => s.TheatreId)
      .OnDelete(DeleteBehavior.Restrict);
```

### Unique Index

```csharp
entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
      .IsUnique();
```

## 8. Database Migration

Migration commands:

```bash
dotnet ef migrations add AddScreens
dotnet ef database update
```

The migration creates the Screens table in the existing MovieTicketBookingDB database and configures its foreign key and unique index.

## 9. Architecture

The module follows:

**Controller → Service → Repository → DbContext**

### Controller

- Receives HTTP requests.
- Calls the service.
- Returns appropriate HTTP responses.

### Service

- Handles business validation.
- Checks whether the requested theatre exists.
- Converts DTOs into Screen entities.
- Converts Screen entities into response DTOs.

### Repository

- Performs database operations.
- Retrieves, inserts, updates and deletes Screen entities.

### DbContext

- Manages entity configurations.
- Defines the relationship between Theatre and Screen.
- Communicates with SQL Server through EF Core.

## 10. Dependency Injection

The following dependencies are registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();
```

The ScreenService also receives ITheatreRepository to validate the parent Theatre.

## 11. API Endpoints

Base URL:

```text
/api/screens
```

| Method | Endpoint | Description | Expected Status |
|---|---|---|---|
| POST | `/api/screens` | Create screen | 201 / 404 / 409 |
| GET | `/api/screens` | Get all screens | 200 |
| GET | `/api/screens?theatreId=1` | Filter screens by theatre | 200 |
| GET | `/api/screens/{id}` | Get screen by ID | 200 / 404 |
| GET | `/api/screens/theatre/{theatreId}` | Get screens in a theatre | 200 / 404 |
| PUT | `/api/screens/{id}` | Update screen | 200 / 404 / 409 |
| DELETE | `/api/screens/{id}` | Delete screen | 204 / 404 / 409 |

## 12. Sample API Requests

### Create Screen

**POST** `/api/screens`

Request:

```json
{
    "theatreId": 1,
    "screenName": "Screen 1"
}
```

Expected response:

```json
{
    "screenId": 1,
    "theatreId": 1,
    "screenName": "Screen 1"
}
```

### Get All Screens

**GET** `/api/screens`

### Filter Screens by Theatre

**GET** `/api/screens?theatreId=1`

### Get Screen by ID

**GET** `/api/screens/1`

### Get Screens by Theatre ID

**GET** `/api/screens/theatre/1`

### Update Screen

**PUT** `/api/screens/1`

Request:

```json
{
    "theatreId": 1,
    "screenName": "Screen 2"
}
```

### Delete Screen

**DELETE** `/api/screens/1`

## 13. Testing Checklist

- [ ] Create a screen using POST.
- [ ] Retrieve all screens using GET.
- [ ] Filter screens using TheatreId.
- [ ] Retrieve a screen using its ID.
- [ ] Retrieve screens belonging to a theatre.
- [ ] Update screen details using PUT.
- [ ] Verify invalid TheatreId returns 404.
- [ ] Verify duplicate screen names within the same theatre return 409.
- [ ] Verify the same screen name is allowed in another theatre.
- [ ] Delete a screen using DELETE.
- [ ] Verify dependent records prevent deletion.

## 14. Important Concepts Learned

- One-to-Many Relationships
- Foreign Keys
- Navigation Properties
- Fluent API
- Composite Unique Index
- DeleteBehavior.Restrict
- Nullable types (`int?`)
- LINQ filtering
- Repository Pattern
- Service Layer
- Dependency Injection
- Async and Await
- HTTP status codes
- CRUD operations

## 15. Next Module

**Feature 4 – Seats Module**

The Seats Module will manage physical seats within each screen.

It will include:
- Creating an individual seat.
- Bulk seat creation.
- Retrieving seats.
- Updating seat details.
- Deleting seats.
- Preventing duplicate seat coordinates within the same screen.

The seat coordinates will use ScreenId, RowName and SeatNumber.