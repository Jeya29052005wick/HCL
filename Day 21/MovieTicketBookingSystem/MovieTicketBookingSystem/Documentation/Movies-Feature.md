# Movie Ticket Booking System
## Feature 01 – Movies Module

**Technology Stack**
- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core 10
- SQL Server
- REST API
- Postman

### 1. Feature Overview

The Movies module manages movie information in the Movie Ticket Booking System.

It supports creating, retrieving, updating, deleting and searching movies.

### 2. Architecture

The module follows the Controller–Service–Repository architecture.

```text
Client / Postman
       |
       v
MoviesController
       |
       v
MovieService
       |
       v
MovieRepository
       |
       v
ApplicationDbContext
       |
       v
SQL Server
```

### 3. Project Structure

```text
MovieTicketBookingSystem/
│
├── Controllers/
│   └── MoviesController.cs
│
├── Models/
│   └── Movie.cs
│
├── DTOs/
│   └── Movies/
│       ├── CreateMovieRequest.cs
│       ├── UpdateMovieRequest.cs
│       └── MovieResponse.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Repositories/
│   ├── Interfaces/
│   │   └── IMovieRepository.cs
│   └── MovieRepository.cs
│
├── Services/
│   ├── Interfaces/
│   │   └── IMovieService.cs
│   └── MovieService.cs
│
├── Migrations/
│
├── Program.cs
└── appsettings.json
```

### 4. Movie Entity

The Movie entity contains:

| Property | Data Type |
|---|---|
| MovieId | int |
| Title | string |
| Description | string? |
| DurationMinutes | int |
| ReleaseDate | DateTime |
| Language | string |
| Genre | string |

Database configuration is implemented using EF Core Fluent API.

### 5. DTOs

- **CreateMovieRequest:** Receives movie creation details.
- **UpdateMovieRequest:** Receives movie update details.
- **MovieResponse:** Returns movie information to the client.

Data Annotations are used for DTO validation.

### 6. API Endpoints

| HTTP Method | Endpoint | Description |
|---|---|---|
| POST | /api/movies | Create a movie |
| GET | /api/movies | Retrieve all movies |
| GET | /api/movies/{id} | Retrieve movie by ID |
| PUT | /api/movies/{id} | Update a movie |
| DELETE | /api/movies/{id} | Delete a movie |

Additional GET query parameters:

- pageNumber
- pageSize
- title

### 7. HTTP Response Codes

| Status Code | Description |
|---|---|
| 200 | Successful retrieval or update |
| 201 | Movie created successfully |
| 204 | Movie deleted successfully |
| 400 | Invalid request |
| 404 | Movie not found |

### 8. Testing

All five Movie API endpoints were tested using Postman.

| Operation | Test Result |
|---|---|
| Create Movie | Passed |
| Get All Movies | Passed |
| Get Movie By ID | Passed |
| Update Movie | Passed |
| Delete Movie | Passed |

### 9. Key Implementation Concepts

- Dependency Injection
- Repository Pattern
- Service Layer
- DTO Pattern
- Asynchronous Programming
- LINQ
- Pagination
- EF Core Fluent API
- Entity Framework Core Migrations
- REST API development

### 10. Feature Status

**Status: Completed and Tested**

The Movies module is the first completed feature of the Movie Ticket Booking System.

**Next Planned Feature:** Theatres Module.