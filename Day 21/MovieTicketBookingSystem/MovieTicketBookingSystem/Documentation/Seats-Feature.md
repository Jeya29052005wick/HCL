# Feature 4 – Seats Module
## Movie Ticket Booking System

### 1. Overview

The Seats Module manages the physical seats available inside each screen of a theatre.

Each seat belongs to one screen and contains information such as:
- Row name
- Seat number
- Seat type (Regular, Premium, etc.)

The module supports individual seat creation, bulk seat creation, retrieving seats, updating seats, and deleting seats.

### 2. Technology Stack

- **Backend:** ASP.NET Core Web API (.NET 10)
- **Language:** C#
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Architecture:** Controller – Service – Repository
- **API Testing:** Postman

### 3. Database Design

**Table: Seats**

| Column | Data Type | Description |
|---|---|---|
| SeatId | int | Primary key, identity |
| ScreenId | int | Foreign key referencing Screens |
| RowName | nvarchar(5) | Seat row identifier |
| SeatNumber | int | Seat number within the row |
| SeatType | nvarchar(30) | Type of seat |

**Constraints:**

1. SeatId is the primary key.
2. ScreenId is a foreign key referencing Screens.
3. SeatNumber must be greater than zero.
4. RowName and SeatType are required.
5. The combination of ScreenId, RowName, and SeatNumber must be unique.
6. A screen cannot be deleted while seats depend on it.

### 4. Entity Model

**File:** `Models/Seat.cs`

The Seat entity represents a physical seat inside a screen.

Properties:
- `SeatId`: Unique identifier.
- `ScreenId`: Identifies the screen to which the seat belongs.
- `RowName`: Represents the seat row.
- `SeatNumber`: Represents the seat's position in the row.
- `SeatType`: Represents the category of the seat.
- `Screen`: Navigation property connecting Seat to Screen.

### 5. Entity Framework Core Configuration

**File:** `Data/ApplicationDbContext.cs`

The Seats table is configured using Fluent API.

Important configurations:

- `HasKey()` defines the primary key.
- `UseIdentityColumn()` enables identity generation.
- `IsRequired()` makes properties mandatory.
- `HasMaxLength()` specifies maximum string lengths.
- `HasOne()` and `HasForeignKey()` configure the relationship with Screen.
- `DeleteBehavior.Restrict` prevents deleting a screen that has dependent seats.
- `HasIndex().IsUnique()` prevents duplicate seat coordinates.
- `HasCheckConstraint()` ensures SeatNumber is greater than zero.

### 6. Database Migration

After configuring the Seat entity, create and apply the migration.

```bash
dotnet ef migrations add AddSeats
dotnet ef database update
```

The migration creates the Seats table in `MovieTicketBookingDB`.

### 7. DTOs (Data Transfer Objects)

DTOs define the data exchanged between the client and the API.

**CreateSeatRequest**
- ScreenId
- RowName
- SeatNumber
- SeatType

**UpdateSeatRequest**
- ScreenId
- RowName
- SeatNumber
- SeatType

**SeatResponse**
- SeatId
- ScreenId
- RowName
- SeatNumber
- SeatType

**BulkSeatItemRequest**
- RowName
- SeatNumber
- SeatType

**BulkCreateSeatsRequest**
- ScreenId
- List of seats

Data Annotations are used in request DTOs to validate incoming data, including required fields, string lengths, and positive numeric values.

### 8. Repository Layer

**Files:**
- `Repositories/Interfaces/ISeatRepository.cs`
- `Repositories/SeatRepository.cs`

The repository layer handles database operations using `ApplicationDbContext`.

Methods:

| Method | Purpose |
|---|---|
| GetByScreenIdAsync() | Retrieves all seats belonging to a screen |
| GetByIdAsync() | Retrieves a seat using its ID |
| ExistsAsync() | Checks whether a seat coordinate already exists |
| AddAsync() | Adds one seat |
| AddRangeAsync() | Adds multiple seats |
| UpdateAsync() | Updates an existing seat |
| DeleteAsync() | Deletes a seat |

The repository uses EF Core asynchronous methods such as `ToListAsync()`, `FirstOrDefaultAsync()`, `AnyAsync()`, and `SaveChangesAsync()`.

### 9. Service Layer

**Files:**
- `Services/Interfaces/ISeatService.cs`
- `Services/SeatService.cs`

The service layer contains the business logic.

Responsibilities:

1. Verify that the requested screen exists.
2. Check whether a seat already exists at the requested coordinate.
3. Create individual seats.
4. Validate bulk seat requests.
5. Prevent duplicate coordinates within a bulk request.
6. Prevent bulk requests from creating seats that already exist.
7. Update seat details after validation.
8. Delete seats when they exist.
9. Convert Seat entities into SeatResponse DTOs.

**Bulk seat creation logic:**

- Check whether the screen exists.
- Check for duplicate row and seat-number combinations within the request.
- Retrieve existing seats for the screen.
- Check whether requested coordinates already exist.
- Create all seats together using `AddRangeAsync()`.

### 10. Controller Layer

**File:** `Controllers/SeatsController.cs`

The controller exposes HTTP endpoints and delegates business operations to `ISeatService`.

#### API Endpoints

| HTTP Method | Endpoint | Description |
|---|---|---|
| POST | `/api/seats` | Create one seat |
| POST | `/api/seats/bulk` | Create multiple seats |
| GET | `/api/seats/screen/{screenId}` | Get all seats in a screen |
| GET | `/api/seats/{id}` | Get a seat by ID |
| PUT | `/api/seats/{id}` | Update a seat |
| DELETE | `/api/seats/{id}` | Delete a seat |

### 11. Dependency Injection

**File:** `Program.cs`

Register the repository and service:

```csharp
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();
```

`AddScoped()` creates one service instance per HTTP request scope.

### 12. Postman Testing

Replace the base URL below with the HTTPS port used by your application.

Example:

`https://localhost:7203`

#### Test 1 – Create One Seat

**POST** `/api/seats`

Request body:

```json
{
  "screenId": 1,
  "rowName": "A",
  "seatNumber": 1,
  "seatType": "Premium"
}
```

Expected result: `201 Created`

#### Test 2 – Bulk Create Seats

**POST** `/api/seats/bulk`

Request body:

```json
{
  "screenId": 1,
  "seats": [
    {
      "rowName": "A",
      "seatNumber": 2,
      "seatType": "Regular"
    },
    {
      "rowName": "A",
      "seatNumber": 3,
      "seatType": "Regular"
    },
    {
      "rowName": "A",
      "seatNumber": 4,
      "seatType": "Premium"
    },
    {
      "rowName": "B",
      "seatNumber": 1,
      "seatType": "Regular"
    },
    {
      "rowName": "B",
      "seatNumber": 2,
      "seatType": "Regular"
    }
  ]
}
```

Expected result: `201 Created`

#### Test 3 – Get Seats by Screen

**GET** `/api/seats/screen/1`

Expected result: `200 OK`

#### Test 4 – Get Seat by ID

**GET** `/api/seats/1`

Expected result: `200 OK`

#### Test 5 – Update Seat

**PUT** `/api/seats/1`

Request body:

```json
{
  "screenId": 1,
  "rowName": "A",
  "seatNumber": 1,
  "seatType": "Regular"
}
```

Expected result: `200 OK`

#### Test 6 – Duplicate Seat Validation

Try creating a seat using an existing combination of ScreenId, RowName, and SeatNumber.

Expected result: `409 Conflict`

#### Test 7 – Invalid Screen ID

Try creating a seat using a ScreenId that does not exist.

Expected result: `404 Not Found`

#### Test 8 – Delete Seat

**DELETE** `/api/seats/1`

Expected result: `204 No Content`

If the seat has dependent records, deletion may be restricted.

### 13. SQL Verification

View all seats:

```sql
SELECT * FROM Seats;
```

View seats with their screen and theatre details:

```sql
SELECT
    se.SeatId,
    se.RowName,
    se.SeatNumber,
    se.SeatType,
    sc.ScreenName,
    t.Name AS TheatreName
FROM Seats se
INNER JOIN Screens sc
    ON se.ScreenId = sc.ScreenId
INNER JOIN Theatres t
    ON sc.TheatreId = t.TheatreId
ORDER BY
    se.ScreenId,
    se.RowName,
    se.SeatNumber;
```

Check for duplicate seat coordinates:

```sql
SELECT
    ScreenId,
    RowName,
    SeatNumber,
    COUNT(*) AS DuplicateCount
FROM Seats
GROUP BY
    ScreenId,
    RowName,
    SeatNumber
HAVING COUNT(*) > 1;
```

Expected result: No rows for the duplicate check.

### 14. HTTP Status Codes

| Status Code | Meaning |
|---|---|
| 200 | OK – Request completed successfully |
| 201 | Created – Seat or seats created |
| 204 | No Content – Seat deleted |
| 400 | Bad Request – Invalid request data |
| 404 | Not Found – Seat or screen not found |
| 409 | Conflict – Duplicate seat or database constraint conflict |
| 500 | Internal Server Error |

### 15. Git and GitHub

After completing implementation and testing:

```bash
git status
git add .
git commit -m "feat: implement Seats module with bulk creation and CRUD APIs"
git push origin main
git status
```

### 16. Important Concepts Learned

- Entity Framework Core relationships.
- Fluent API configuration.
- Primary keys and foreign keys.
- Unique composite indexes.
- Check constraints.
- DTOs and request validation.
- Repository Pattern.
- Service Layer and business logic.
- Dependency Injection.
- Asynchronous database operations.
- Bulk insertion using `AddRangeAsync()`.
- Duplicate validation.
- REST API testing using Postman.
- SQL joins and grouping.
- Git commits and pushing changes to GitHub.

### 17. Feature Completion Checklist

- [ ] Seat entity created.
- [ ] Fluent API configuration completed.
- [ ] Migration created and applied.
- [ ] Request and response DTOs created.
- [ ] Repository interface and implementation completed.
- [ ] Service interface and implementation completed.
- [ ] Controller endpoints implemented.
- [ ] Dependency Injection configured.
- [ ] Individual seat creation tested.
- [ ] Bulk seat creation tested.
- [ ] Duplicate validation tested.
- [ ] Update and delete tested.
- [ ] SQL database verified.
- [ ] Changes committed and pushed to GitHub.

---

**Feature 4: Seats Module**

The Seats Module establishes the physical seat layout for each screen. This data will be used by the upcoming Shows and Booking modules to manage seat availability and customer bookings.