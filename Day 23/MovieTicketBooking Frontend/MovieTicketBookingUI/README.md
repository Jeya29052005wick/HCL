# Secret Tomatoes — Angular Movie Booking Frontend

A premium dark-navy / electric-blue Angular frontend designed against the supplied **MovieTicketBookingSystem** ASP.NET Core API source.

## Stack
- Angular 21 standalone components and Angular Router
- TypeScript, HttpClient, template-driven forms
- Responsive CSS (no UI framework required)
- JWT bearer token interceptor

## Setup

1. Install Node.js 22.17+ and npm.
2. Open this folder in a terminal.
3. Install dependencies:

   ```bash
   npm install
   ```

4. Confirm your backend is running. The supplied backend launch profile uses:
   - HTTPS: `https://localhost:7203`
   - HTTP: `http://localhost:5124`

5. The API base URL is configured in `src/environments/environment.ts`:

   ```ts
   apiBaseUrl: 'https://localhost:7203/api'
   ```

   Change it if your backend runs on another port.

6. Start the frontend:

   ```bash
   npm start
   ```

7. Open `http://localhost:4200`.

## Backend CORS requirement

The uploaded backend's `Program.cs` does not currently register a CORS policy. The browser will block Angular requests from `http://localhost:4200` until you enable CORS.

Add this before `var app = builder.Build();` in backend `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

Then add this to the middleware pipeline after `app.UseHttpsRedirection();` and before authentication/authorization:

```csharp
app.UseCors("AngularClient");
```

Restart the API. For a deployed frontend, replace the origin with its exact domain. Do not use `AllowAnyOrigin` with credentials.

If the HTTPS development certificate is not trusted, run `dotnet dev-certs https --trust` from a terminal and restart the browser.

## Pages and API mapping

| Frontend | Backend contract |
|---|---|
| Discover / Movies | `GET /api/movies?pageNumber&pageSize&title` |
| Movie details | `GET /api/movies/{id}` |
| Show schedule | `GET /api/shows?date&movieId&theatreId` |
| Seat map | `GET /api/shows/{showId}/seats` |
| Register | `POST /api/auth/register` (`fullName`, `email`, `password`) |
| Login | `POST /api/auth/login` (`email`, `password`) |
| Booking | `POST /api/bookings` (`customerName`, `customerEmail`, `customerPhone`, `showId`, `seatIds`) |
| Booking details | `GET /api/bookings/{id}` |
| Cancellation | `PUT /api/bookings/{id}/cancel` |
| Admin catalogue | Movies and theatres CRUD endpoints |
| Theatre operations | Screens, bulk seats and shows endpoints |

All request payloads use the DTO field names from the uploaded backend. The backend calculates the booking total and validates availability; the frontend only sends seat IDs and customer details.

## Role behaviour

- **Customer**: browse movies, view showtimes, choose seats, register/sign in and submit bookings.
- **Admin**: manage movies, theatres, screens, physical seats and shows.
- **TheatreManager**: manage screens, physical seats and shows. The backend remains the authority for permissions.

New registrations are assigned the `Customer` role by the supplied API. Admin and TheatreManager accounts must already exist with those roles in the backend. The frontend does not provide a role-elevation feature.

## Important API boundaries

The supplied backend has no movie poster/image field, no payment endpoint, and no customer-scoped booking-history endpoint. Accordingly:

- Poster artwork is a decorative frontend mapping for a few known titles with a fallback; it is not persisted by the API.
- The booking flow confirms the backend booking only. It does not claim that a payment was processed.
- A secure personal booking-history page is omitted because the current API does not expose a customer-scoped history endpoint. `GET /api/bookings` is not used as a customer history feed.
- The backend's booking creation and cancellation endpoints require JWT authentication. The app sends the token through an HTTP interceptor.

## Notes

This is a frontend source project. The API must be running separately, CORS must be enabled, and the SQL Server database must contain movies, theatres, screens, seats and shows for populated screens to appear. No backend source files are changed by this package.
