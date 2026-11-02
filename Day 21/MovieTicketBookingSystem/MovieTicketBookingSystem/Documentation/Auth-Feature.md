# Authentication and Authorization

## 1. Overview

This feature implements Authentication and Authorization in the Movie Ticket Booking System using JWT (JSON Web Token).

- **Authentication** verifies the identity of a user.
- **Authorization** determines what actions an authenticated user is allowed to perform.

The system supports three roles:

1. Admin
2. TheatreManager
3. Customer

## 2. Authentication

Authentication allows users to register and log in to the application.

### 2.1 User Registration

**Endpoint:** `POST /api/auth/register`

A new customer can register using:

- FullName
- Email
- Password

The password is hashed using ASP.NET Core's `IPasswordHasher<User>` before storing it in the database.

The system checks whether the email is already registered before creating a new account.

**Example Request:**

```json
{
  "fullName": "Sample Customer",
  "email": "customer@example.com",
  "password": "Customer@123"
}
```

**Example Response:**

```json
{
  "message": "Registration successful."
}
```

The role is assigned by the backend as `Customer`. Users cannot assign themselves an Admin or TheatreManager role during public registration.

### 2.2 User Login

**Endpoint:** `POST /api/auth/login`

Registered users can log in using their email and password.

The system:

1. Checks whether the email exists.
2. Verifies the submitted password against the stored password hash.
3. Generates a JWT if the credentials are valid.
4. Returns the token to the client.

**Example Request:**

```json
{
  "email": "customer@example.com",
  "password": "Customer@123"
}
```

**Example Response:**

```json
{
  "message": "Login successful.",
  "token": "<JWT_TOKEN>"
}
```

Invalid credentials return `401 Unauthorized`.

## 3. Password Hashing

Passwords are never stored as plain text.

The application uses:

- `IPasswordHasher<User>`
- `HashPassword()`
- `VerifyHashedPassword()`

During registration, the password is converted into a secure hash and stored in the `PasswordHash` column.

During login, the submitted password is verified against the stored hash.

## 4. JWT Authentication

JWT stands for JSON Web Token.

A JWT is generated after successful login and is used to authenticate subsequent API requests.

### 4.1 JWT Structure

A JWT contains three parts:

1. Header – Contains token information and the signing algorithm.
2. Payload – Contains claims such as UserId, Name, Email and Role.
3. Signature – Used to verify that the token has not been modified.

### 4.2 JWT Claims

The token contains the following claims:

- UserId
- FullName
- Email
- Role

### 4.3 JWT Configuration

The JWT configuration contains:

- Issuer
- Audience
- ExpiryMinutes
- Signing Key

The signing key and account passwords are stored using .NET User Secrets during local development.

Example configuration in `appsettings.json`:

```json
{
  "Jwt": {
    "Issuer": "MovieTicketBooking",
    "Audience": "MovieTicketBookingUsers",
    "ExpiryMinutes": "30"
  }
}
```

The actual signing key must not be committed to GitHub.

## 5. Authorization

Authorization controls access to API endpoints based on the authenticated user's role.

The system uses role-based authorization through ASP.NET Core's `[Authorize]` attribute.

### 5.1 User Roles

| Role           | Responsibilities                                      |
| -------------- | ----------------------------------------------------- |
| Admin          | Manage movies, theatres and administrative operations |
| TheatreManager | Manage theatre operations, screens, seats and shows   |
| Customer       | Access customer booking functionality                 |

### 5.2 Authorization Attributes

**Any authenticated user:**

```csharp
[Authorize]
```

**Admin only:**

```csharp
[Authorize(Roles = UserRoles.Admin)]
```

**Admin or TheatreManager:**

```csharp
[Authorize(Roles = UserRoles.Admin + "," + UserRoles.TheatreManager)]
```

**Public endpoint:**

```csharp
[AllowAnonymous]
```

### 5.3 Suggested Access Control

| Module                        | Access                 |
| ----------------------------- | ---------------------- |
| Browse Movies                 | Public                 |
| Browse Theatres               | Public                 |
| Browse Shows                  | Public                 |
| Create/Update/Delete Movies   | Admin                  |
| Create/Update/Delete Theatres | Admin                  |
| Manage Screens and Seats      | Admin / TheatreManager |
| Manage Shows                  | Admin / TheatreManager |
| Create Booking                | Customer               |
| View Own Bookings             | Customer               |
| View All Bookings             | Admin                  |
| Register and Login            | Public                 |

Resource ownership must also be checked in the service layer. For example, a Customer must only be able to view or cancel their own bookings.

## 6. Development Role Seeding

`RoleSeeder` is used to create initial Admin and TheatreManager accounts for development and testing.

The seeded accounts are:

| User            | Email                                                        | Role           |
| --------------- | ------------------------------------------------------------ | -------------- |
| System Admin    | [admin@moviebooking.com](mailto\:admin@moviebooking.com)     | Admin          |
| Theatre Manager | [manager@moviebooking.com](mailto\:manager@moviebooking.com) | TheatreManager |

Their passwords are read from User Secrets.

The seeder:

1. Checks whether the account already exists.
2. Creates the account if it does not exist.
3. Hashes the password.
4. Saves the user in the database.

The seeder runs in Development mode.

## 7. Dependency Injection

The following services are registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IJwtService, JwtService>();
```

These registrations allow ASP.NET Core Dependency Injection to provide the required implementations to controllers and services.

## 8. Authentication Middleware

JWT authentication and authorization are configured in `Program.cs`.

```csharp
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
```

**Important:** `UseAuthentication()` must be called before `UseAuthorization()`.

- `UseAuthentication()` validates the JWT and identifies the user.
- `UseAuthorization()` checks whether the user has permission to access the endpoint.

## 9. API Testing

Authentication and authorization can be tested using Postman or Scalar.

### Test Cases

| Test                                    | Expected Result     |
| --------------------------------------- | ------------------- |
| Register with valid details             | 200 OK              |
| Register with an existing email         | 400 Bad Request     |
| Login with valid credentials            | 200 OK with JWT     |
| Login with invalid credentials          | 401 Unauthorized    |
| Access protected endpoint without token | 401 Unauthorized    |
| Customer accesses Admin-only endpoint   | 403 Forbidden       |
| Admin accesses Admin-only endpoint      | Successful response |
| Access public endpoint without token    | Successful response |

### Using JWT in Postman

1. Send a login request.
2. Copy the JWT returned in the response.
3. Open the protected API request.
4. Select the **Authorization** tab.
5. Choose **Bearer Token**.
6. Paste the JWT.
7. Send the request.

## 10. Important HTTP Status Codes

| Status Code      | Description                                              |
| ---------------- | -------------------------------------------------------- |
| 200 OK           | Request completed successfully                           |
| 400 Bad Request  | Invalid request or duplicate email                       |
| 401 Unauthorized | Missing, invalid or expired token                        |
| 403 Forbidden    | Authenticated user does not have the required permission |

## 11. Security Considerations

- Never store plain-text passwords.
- Never expose password hashes in API responses.
- Do not allow users to assign their own roles during registration.
- Keep JWT signing keys and passwords outside source control.
- Use HTTPS for API communication.
- Validate JWT issuer, audience, expiry and signing key.
- Verify booking ownership before allowing customers to view or cancel bookings.
- Restrict TheatreManager access to their assigned theatre resources.

## 12. Feature Completion Checklist

- [ ] User registration
- [ ] User login
- [ ] Password hashing
- [ ] JWT generation
- [ ] JWT validation
- [ ] Role-based authorization
- [ ] Admin role seeding
- [ ] TheatreManager role seeding
- [ ] Dependency Injection configuration
- [x] Authentication middleware
- [ ] Protected API endpoints
- [ ] Complete Postman testing
- [ ] Verify resource ownership restrictions

---

**Completed: Authentication and Authorization**

Technology Stack: C# | ASP.NET Core Web API | .NET 10 | EF Core 10 | SQL Server | JWT
