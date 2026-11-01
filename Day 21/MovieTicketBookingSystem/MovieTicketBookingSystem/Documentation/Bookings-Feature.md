# Feature 6 – Bookings Module

## 1. Overview

The Bookings Module is responsible for creating, retrieving and cancelling customer bookings in the Movie Ticket Booking System.

It connects the Show, ShowSeat, Seat and Booking entities.

**Technology Stack**
- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- Fluent API
- Repository Pattern
- Service Layer
- Dependency Injection

## 2. Project Structure

```text
DTOs/
└── Bookings/
    ├── CreateBookingRequest.cs
    └── BookingResponse.cs

Models/
├── Booking.cs
└── BookingSeat.cs

Repositories/
├── Interfaces/
│   └── IBookingRepository.cs
└── BookingRepository.cs

Services/
├── Interfaces/
│   └── IBookingService.cs
└── BookingService.cs

Controllers/
└── BookingsController.cs
```

## 3. Database Design

### Bookings table

| Column | SQL Server type | Constraint |
|---|---|---|
| BookingId | int | Primary key, Identity |
| BookingReference | nvarchar(30) | Required, Unique |
| ShowId | int | Foreign key |
| CustomerName | nvarchar(100) | Required |
| CustomerEmail | nvarchar(150) | Required |
| CustomerPhone | nvarchar(15) | Required |
| BookingDate | datetime2 | Required |
| TotalAmount | decimal(10,2) | Required, >= 0 |
| Status | nvarchar(20) | Required |

### BookingSeats table

| Column | SQL Server type | Constraint |
|---|---|---|
| BookingSeatId | int | Primary key, Identity |
| BookingId | int | Foreign key |
| SeatId | int | Foreign key |
| Price | decimal(10,2) | Required, >= 0 |

### Relationships

- One Booking can contain multiple BookingSeats.
- One BookingSeat references one physical Seat.
- One Booking belongs to one Show.
- BookingReference must be unique.
- SeatId must not be globally unique in BookingSeats because the same physical seat can be booked for different shows.

## 4. Booking Workflow

1. Customer selects a show.
2. Customer requests the show's seat layout.
3. Customer selects one or more available seats.
4. Customer sends customer details, ShowId and SeatIds.
5. Backend checks that the show exists.
6. Backend checks that the requested seat IDs are distinct.
7. Backend verifies that every selected seat is available for that show.
8. Backend calculates the total using the show's TicketPrice.
9. Backend creates the Booking and BookingSeats records.
10. Backend marks the selected ShowSeats as Booked.
11. All database changes are committed in one transaction.

## 5. Booking Amount Calculation

The frontend does not provide the total amount.

Formula:

`TotalAmount = NumberOfSelectedSeats × ShowTicketPrice`

Example:

- Ticket price: ₹250
- Selected seats: 3
- Total amount: ₹750

The backend calculates and stores the amount.

## 6. Cancellation Workflow

1. Receive the BookingId.
2. Retrieve the booking and its seat details.
3. Check that the booking exists.
4. Check that its status is Confirmed.
5. Start a database transaction.
6. Change the booking status to Cancelled.
7. Change the associated ShowSeats statuses to Available.
8. Save and commit the transaction.

Cancellation does not delete the booking history.

## 7. API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/bookings` | Create a booking |
| GET | `/api/bookings` | Retrieve all bookings |
| GET | `/api/bookings?showId=1` | Filter by show |
| GET | `/api/bookings?status=Confirmed` | Filter by status |
| GET | `/api/bookings/{id}` | Retrieve booking details |
| GET | `/api/bookings/show/{showId}` | Retrieve bookings for a show |
| PUT | `/api/bookings/{id}/cancel` | Cancel a booking |

## 8. Example Booking Request

`POST /api/bookings`

```json
{
  "customerName": "Arun",
  "customerEmail": "arun@example.com",
  "customerPhone": "9876543210",
  "showId": 1,
  "seatIds": [1, 2, 3]
}
```

The backend calculates the total and generates the BookingReference.

## 9. Expected Response

HTTP Status: `201 Created`

```json
{
  "bookingId": 1,
  "bookingReference": "MOVABC123456789",
  "showId": 1,
  "customerName": "Arun",
  "customerEmail": "arun@example.com",
  "customerPhone": "9876543210",
  "totalAmount": 750.00,
  "status": "Confirmed",
  "seats": ["A1", "A2", "A3"]
}
```

## 10. HTTP Status Codes

| Status | Meaning |
|---|---|
| 200 | Successful retrieval or cancellation |
| 201 | Booking successfully created |
| 400 | Invalid request data |
| 404 | Booking or show not found |
| 409 | Seat unavailable or booking conflict |

## 11. Database Verification

```sql
SELECT * FROM Bookings;

SELECT * FROM BookingSeats;

SELECT * FROM ShowSeats
WHERE ShowId = 1;
```

Verify that seats become Booked after booking and Available after cancellation.

## 12. Important Concepts Learned

- Entity relationships in EF Core
- One-to-many relationships
- Fluent API configuration
- Unique indexes and check constraints
- DTOs and request validation
- Repository Pattern
- Service Layer
- Dependency Injection
- LINQ filtering
- Transactions
- Async and Await
- Business rule validation
- Booking reference generation
- HTTP status codes

## 13. Version 1 Limitations

- No customer authentication or authorization.
- No payment gateway or refund processing.
- No temporary seat holds.
- No advanced concurrency protection.
- Basic availability validation and database transactions are used.

A later version can introduce optimistic concurrency protection to prevent simultaneous booking conflicts.

## 14. Git and GitHub Commands

Run these commands from the project directory after building and testing:

```bash
git status

git add .

git commit -m "feat: implement Bookings module with booking and cancellation"

git push origin main

git status
```

## 15. Completion Checklist

- [ ] Booking and BookingSeat models created.
- [ ] Fluent API configuration completed.
- [ ] Migration created and database updated.
- [ ] Booking DTOs created.
- [ ] Booking repository implemented.
- [ ] Booking service implemented.
- [ ] Booking controller implemented.
- [ ] Dependency injection registered.
- [ ] Booking creation tested.
- [ ] Multiple-seat booking tested.
- [ ] Duplicate and unavailable seats tested.
- [ ] Booking cancellation tested.
- [ ] Database records verified.
- [ ] Git commit and push completed.

**Feature 6 Deliverable:** Customer booking and cancellation APIs with database transaction handling and preserved booking history.