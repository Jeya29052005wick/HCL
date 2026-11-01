# Feature 5 – Shows Module
## Movie Ticket Booking System

### 1. Overview

The Shows Module manages movie show scheduling within theatre screens.

It allows the system to:
- Schedule a movie on a particular screen.
- Define show start time and end time.
- Set the ticket price.
- Prevent overlapping shows on the same screen.
- Automatically generate ShowSeats records for every physical seat on the selected screen.
- Retrieve shows using different filters.
- Display seat availability for a particular show.

The module connects Movies, Screens, Seats, Shows, and ShowSeats.

### 2. Technology Stack

- **Backend:** ASP.NET Core Web API (.NET 10)
- **Language:** C#
- **ORM:** Entity Framework Core 10
- **Database:** Microsoft SQL Server
- **Architecture:** Controller – Service – Repository
- **API Testing:** Postman

### 3. Database Design

This feature introduces two tables.

#### Table 1: Shows

| Column | Data Type | Description |
|---|---|---|
| ShowId | int | Primary key, identity |
| MovieId | int | Foreign key referencing Movies |
| ScreenId | int | Foreign key referencing Screens |
| StartTime | datetime2 | Show start time |
| EndTime | datetime2 | Show end time |
| TicketPrice | decimal(10,2) | Ticket price |

**Constraints:**

1. ShowId is the primary key.
2. MovieId references Movies.
3. ScreenId references Screens.
4. EndTime must be later than StartTime.
5. TicketPrice must be greater than or equal to zero.
6. Deleting a Movie or Screen with dependent shows is restricted.

#### Table 2: ShowSeats

| Column | Data Type | Description |
|---|---|---|
| ShowSeatId | int | Primary key, identity |
| ShowId | int | Foreign key referencing Shows |
| SeatId | int | Foreign key referencing Seats |
| Status | nvarchar(20) | Seat availability status |

**Constraints:**

1. ShowSeatId is the primary key.
2. ShowId references Shows.
3. SeatId references Seats.
4. Status is required.
5. The combination of ShowId and SeatId must be unique.
6. Initial status is `Available`.
7. ShowSeats records are removed when their show is deleted.
8. Physical seats cannot be deleted while dependent ShowSeats records exist.

### 4. Entity Relationships

The Shows Module introduces these relationships:

| Relationship | Cardinality | Meaning |
|---|---|---|
| Movie → Shows | One-to-many | A movie can have multiple shows |
| Screen → Shows | One-to-many | A screen can host multiple shows at different times |
| Show → ShowSeats | One-to-many | Each show has multiple seat availability records |
| Seat → ShowSeats | One-to-many | A physical seat can appear in multiple shows |

**Important distinction:**

- `Seats` stores the physical seating arrangement of a screen.
- `ShowSeats` stores the availability of those physical seats for an individual show.

For example, physical seat A1 can be used in multiple shows. Each show has its own ShowSeats record for A1.

### 5. Entity Models

#### Show.cs

**Location:** `Models/Show.cs`

Properties:

- `ShowId`: Unique show identifier.
- `MovieId`: Identifies the scheduled movie.
- `ScreenId`: Identifies the screen hosting the show.
- `StartTime`: Starting time of the show.
- `EndTime`: Ending time of the show.
- `TicketPrice`: Price per ticket.
- `Movie`: Navigation property for the Movie relationship.
- `Screen`: Navigation property for the Screen relationship.

#### ShowSeat.cs

**Location:** `Models/ShowSeat.cs`

Properties:

- `ShowSeatId`: Unique identifier for the availability record.
- `ShowId`: Identifies the show.
- `SeatId`: Identifies the physical seat.
- `Status`: Indicates whether the seat is Available or Booked.
- `Show`: Navigation property for Show.
- `Seat`: Navigation property for Seat.

### 6. Entity Framework Core Configuration

**File:** `Data/ApplicationDbContext.cs`

Two DbSet properties are introduced:

- `Shows`
- `ShowSeats`

Fluent API configurations are used instead of Data Annotations on entity models.

Important configurations:

| Method | Purpose |
|---|---|
| `HasKey()` | Defines the primary key |
| `UseIdentityColumn()` | Enables identity generation |
| `HasColumnType()` | Specifies SQL Server column type |
| `HasPrecision(10, 2)` | Configures ticket price precision |
| `HasCheckConstraint()` | Enforces valid time and price conditions |
| `HasOne()` | Configures entity relationships |
| `HasForeignKey()` | Defines foreign keys |
| `OnDelete(DeleteBehavior.Restrict)` | Prevents deletion of dependent Movie, Screen, and Seat records |
| `OnDelete(DeleteBehavior.Cascade)` | Deletes ShowSeats when a Show is deleted |
| `HasIndex().IsUnique()` | Prevents duplicate ShowId and SeatId combinations |

### 7. Database Migration

After configuring both entities in ApplicationDbContext, create and apply the migration.

```bash
dotnet ef migrations add AddShowsAndShowSeats
```

Update the database:

```bash
dotnet ef database update
```

The migration creates:

- Shows
- ShowSeats

Both tables are created in `MovieTicketBookingDB`.

### 8. DTOs (Data Transfer Objects)

**Location:** `DTOs/Shows/`

#### CreateShowRequest

Used to receive show creation details.

Properties:
- MovieId
- ScreenId
- StartTime
- EndTime
- TicketPrice

#### UpdateShowRequest

Used to receive updated show details.

Properties:
- MovieId
- ScreenId
- StartTime
- EndTime
- TicketPrice

#### ShowResponse

Returns show information:

- ShowId
- MovieId
- MovieTitle
- ScreenId
- ScreenName
- TheatreId
- TheatreName
- StartTime
- EndTime
- TicketPrice

#### ShowSeatResponse

Returns seat availability information:

- SeatId
- RowName
- SeatNumber
- SeatLabel
- SeatType
- Price
- Status

DTOs keep API request and response contracts separate from EF Core entities.

### 9. Repository Layer

**Files:**
- `Repositories/Interfaces/IShowRepository.cs`
- `Repositories/ShowRepository.cs`

The repository layer manages database operations.

| Method | Purpose |
|---|---|
| GetAllAsync() | Retrieves shows with optional filters |
| GetByIdAsync() | Retrieves a show by ID |
| GetByMovieIdAsync() | Retrieves shows for a movie |
| GetByTheatreIdAsync() | Retrieves shows for a theatre |
| HasOverlapAsync() | Checks for overlapping shows |
| AddWithSeatsAsync() | Saves a show and its ShowSeats records |
| UpdateAsync() | Updates show details |
| DeleteAsync() | Deletes a show |
| GetShowSeatsAsync() | Retrieves seat availability records |
| HasShowSeatsAsync() | Checks whether availability records exist |

Important EF Core methods used:

- `Include()`
- `ThenInclude()`
- `Where()`
- `OrderBy()`
- `AnyAsync()`
- `ToListAsync()`
- `BeginTransactionAsync()`
- `CommitAsync()`
- `RollbackAsync()`
- `AddRangeAsync()`

### 10. Service Layer

**Files:**
- `Services/Interfaces/IShowService.cs`
- `Services/ShowService.cs`

The service layer contains show scheduling business logic.

Responsibilities:

1. Validate that EndTime is later than StartTime.
2. Validate that TicketPrice is not negative.
3. Verify that the requested Movie exists.
4. Verify that the requested Screen exists.
5. Check whether the screen already has an overlapping show.
6. Retrieve all physical seats belonging to the selected screen.
7. Create the Show entity.
8. Generate one ShowSeat record for every physical seat.
9. Set every newly generated ShowSeat status to Available.
10. Save the Show and ShowSeats records within a database transaction.
11. Map entities to response DTOs.
12. Retrieve shows and seat availability information.

#### Show overlap validation

The overlap query uses:

```csharp
s.StartTime < endTime &&
s.EndTime > startTime
```

This checks whether an existing show overlaps with the requested show on the same screen.

Example:

| Existing Show | New Show | Result |
|---|---|---|
| 10:00–12:00 | 11:00–13:00 | Overlap |
| 10:00–12:00 | 11:30–12:30 | Overlap |
| 10:00–12:00 | 12:00–14:00 | No overlap |
| 10:00–12:00 | 13:00–15:00 | No overlap |

The same screen can host another show when the new show does not overlap with an existing show.

#### Automatic ShowSeats generation

When a show is created:

1. Retrieve all physical seats belonging to the selected screen.
2. Create a ShowSeat object for each physical seat.
3. Assign the ShowId after the show has been saved.
4. Set the initial status to Available.
5. Insert all ShowSeats records using AddRangeAsync.
6. Commit the transaction.

The number of generated ShowSeats records should equal the physical seat count of the selected screen.

### 11. Controller Layer

**File:** `Controllers/ShowsController.cs`

The controller exposes REST API endpoints and delegates operations to `IShowService`.

#### API Endpoints

| HTTP Method | Endpoint | Description |
|---|---|---|
| POST | `/api/shows` | Create a show |
| GET | `/api/shows` | Get all shows |
| GET | `/api/shows/{id}` | Get show by ID |
| GET | `/api/shows/movie/{movieId}` | Get shows for a movie |
| GET | `/api/shows/theatre/{theatreId}` | Get shows for a theatre |
| GET | `/api/shows/search?date=YYYY-MM-DD` | Search shows by date |
| PUT | `/api/shows/{id}` | Update show schedule |
| DELETE | `/api/shows/{id}` | Delete a show |
| GET | `/api/shows/{showId}/seats` | Get complete seat layout |
| GET | `/api/shows/{showId}/seats/available` | Get available seats |
| GET | `/api/shows/{showId}/seats/booked` | Get booked seats |

The main GET endpoint also supports optional filters:

- `date`
- `movieId`
- `theatreId`

These filters can be combined.

### 12. Dependency Injection

**File:** `Program.cs`

Register the repository and service:

```csharp
builder.Services.AddScoped<IShowRepository, ShowRepository>();

builder.Services.AddScoped<IShowService, ShowService>();
```

The ShowService also depends on the existing MovieRepository, ScreenRepository, and SeatRepository registrations.

### 13. Postman Testing

Replace the sample port with the HTTPS port used by the application.

Example base URL:

`https://localhost:7203`

#### Test 1 – Create a Show

**POST** `/api/shows`

Request body:

```json
{
  "movieId": 1,
  "screenId": 1,
  "startTime": "2026-11-03T10:00:00",
  "endTime": "2026-11-03T12:30:00",
  "ticketPrice": 250.00
}
```

Expected result: `201 Created`

On successful creation, ShowSeats records are generated automatically.

#### Test 2 – Get All Shows

**GET** `/api/shows`

Expected result: `200 OK`

#### Test 3 – Filter by Date

**GET** `/api/shows?date=2026-11-03`

Expected result: `200 OK`

#### Test 4 – Filter by Movie

**GET** `/api/shows?movieId=1`

Expected result: `200 OK`

#### Test 5 – Filter by Theatre

**GET** `/api/shows?theatreId=1`

Expected result: `200 OK`

Combined filters:

```text
/api/shows?date=2026-11-03&movieId=1&theatreId=1
```

#### Test 6 – Get Show by ID

**GET** `/api/shows/1`

Expected result: `200 OK`

#### Test 7 – Get Shows by Movie

**GET** `/api/shows/movie/1`

Expected result: `200 OK`

#### Test 8 – Get Shows by Theatre

**GET** `/api/shows/theatre/1`

Expected result: `200 OK`

#### Test 9 – Search Shows by Date

**GET** `/api/shows/search?date=2026-11-03`

Expected result: `200 OK`

#### Test 10 – Update Show

**PUT** `/api/shows/1`

Request body:

```json
{
  "movieId": 1,
  "screenId": 1,
  "startTime": "2026-11-03T14:00:00",
  "endTime": "2026-11-03T16:30:00",
  "ticketPrice": 300.00
}
```

Expected result: `200 OK`

#### Test 11 – Get Show Seat Layout

**GET** `/api/shows/1/seats`

Expected result: `200 OK`

The response contains physical seat details and their availability status.

#### Test 12 – Get Available Seats

**GET** `/api/shows/1/seats/available`

Expected result: `200 OK`

#### Test 13 – Get Booked Seats

**GET** `/api/shows/1/seats/booked`

Expected result: `200 OK`

Before the Booking Module is implemented, this will normally return an empty array.

#### Test 14 – Overlapping Show Validation

Try creating another show on the same screen:

```json
{
  "movieId": 1,
  "screenId": 1,
  "startTime": "2026-11-03T11:00:00",
  "endTime": "2026-11-03T13:00:00",
  "ticketPrice": 200.00
}
```

The first show is scheduled from 10:00 to 12:30.

Expected result: `409 Conflict`

#### Test 15 – Invalid Time Validation

Try providing an EndTime earlier than StartTime.

Expected result: `409 Conflict`

#### Test 16 – Delete Show

**DELETE** `/api/shows/1`

Expected result: `204 No Content` if there are no dependent booking records.

If booking history exists, deletion should be restricted.

### 14. SQL Verification

View all shows:

```sql
SELECT * FROM Shows;
```

View all show-seat records:

```sql
SELECT * FROM ShowSeats;
```

Verify generated seat counts:

```sql
SELECT
    s.ShowId,
    s.ScreenId,
    COUNT(ss.ShowSeatId) AS GeneratedSeatCount
FROM Shows s
LEFT JOIN ShowSeats ss
    ON s.ShowId = ss.ShowId
GROUP BY
    s.ShowId,
    s.ScreenId;
```

Compare physical seats with generated ShowSeats:

```sql
SELECT
    sh.ShowId,
    sh.ScreenId,
    (SELECT COUNT(*)
     FROM Seats se
     WHERE se.ScreenId = sh.ScreenId) AS PhysicalSeatCount,
    (SELECT COUNT(*)
     FROM ShowSeats ss
     WHERE ss.ShowId = sh.ShowId) AS GeneratedSeatCount
FROM Shows sh;
```

For each newly created show, PhysicalSeatCount and GeneratedSeatCount should match.

Check for duplicate ShowSeats:

```sql
SELECT
    ShowId,
    SeatId,
    COUNT(*) AS DuplicateCount
FROM ShowSeats
GROUP BY ShowId, SeatId
HAVING COUNT(*) > 1;
```

Expected result: no rows.

### 15. HTTP Status Codes

| Status Code | Meaning |
|---|---|
| 200 | OK – Request completed successfully |
| 201 | Created – Show created |
| 204 | No Content – Show deleted |
| 400 | Bad Request – Invalid request data |
| 404 | Not Found – Movie, screen, or show not found |
| 409 | Conflict – Overlapping show or database constraint conflict |
| 500 | Internal Server Error |

### 16. Git and GitHub

After completing implementation and testing:

```bash
git status

git add .

git commit -m "feat: implement Shows module with scheduling and seat availability"

git push origin main

git status
```

### 17. Important Concepts Learned

- Entity Framework Core navigation properties.
- One-to-many relationships.
- Fluent API configuration.
- Check constraints.
- Unique composite indexes.
- DTOs and request validation.
- Repository Pattern.
- Service Layer and business logic.
- Dependency Injection.
- LINQ filtering and query composition.
- Include and ThenInclude.
- Overlapping time interval validation.
- Database transactions.
- Commit and rollback.
- Bulk insertion using AddRangeAsync.
- Automatic generation of related records.
- REST API development.
- Postman testing.
- SQL joins and aggregate queries.
- Git commits and pushing changes to GitHub.

### 18. Feature Completion Checklist

- [ ] Show entity created.
- [ ] ShowSeat entity created.
- [ ] Fluent API configuration completed.
- [ ] Migration created and applied.
- [ ] CreateShowRequest created.
- [ ] UpdateShowRequest created.
- [ ] ShowResponse created.
- [ ] ShowSeatResponse created.
- [ ] Repository interface and implementation completed.
- [ ] Service interface and implementation completed.
- [ ] Overlap validation tested.
- [ ] Automatic ShowSeats generation tested.
- [ ] Controller endpoints implemented.
- [ ] Dependency Injection configured.
- [ ] All Postman endpoints tested.
- [ ] SQL database verified.
- [ ] Changes committed and pushed to GitHub.

---

### 19. Feature 5 Summary

The Shows Module connects movie scheduling with the physical seating arrangement of a theatre.

When a show is created, the system automatically generates a ShowSeats record for every physical seat on the selected screen. These records initially have the status Available.

The upcoming Booking Module will use these records to validate seat availability, create bookings, and update seat statuses.

**Feature 5: Shows Module – Completed implementation milestone.**