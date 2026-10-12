# Student Management API

## 1. Project Overview

The Student Management API is a RESTful Web API developed using ASP.NET Core and C#. It allows users to manage student records for a college or training institute.

The API supports creating, retrieving, updating, deleting, and searching student records. Student information is stored in a Microsoft SQL Server database using Entity Framework Core.

## 2. Technologies Used

- C#
- .NET 10
- ASP.NET Core Web API (Controllers)
- Entity Framework Core
- SQL Server Express
- SQL Server Management Studio (SSMS)
- Postman
- OpenAPI

## 3. Features

- Create a new student record.
- Retrieve all student records.
- Retrieve a student by ID.
- Update an existing student's details.
- Delete a student record.
- Search students by name and department.
- Validate student input.
- Prevent duplicate email addresses.
- Store data persistently in SQL Server.
- Return appropriate HTTP status codes.

## 4. Student Model

The Student model contains the following properties:

| Property | Data Type | Validation |
|---|---|---|
| Id | Integer | Database-generated primary key |
| Name | String | Required, maximum 100 characters |
| Email | String | Required, valid email, maximum 254 characters, unique |
| Department | String | Required, maximum 100 characters |
| Age | Integer | Between 17 and 60 |

## 5. Project Structure

```text
StudentManagement.Api/
├── Controllers/
│   └── StudentsController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   └── Student.cs
├── Migrations/
├── Properties/
├── appsettings.json
├── Program.cs
├── StudentManagement.Api.csproj
└── README.md
```

## 6. Database Configuration

**Database name:** `StudentManagementDb`

**SQL Server instance:** `localhost\SQLEXPRESS`

The connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=StudentManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Entity Framework Core manages database operations and migrations.

## 7. Required NuGet Packages

The project uses the following packages:

- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.AspNetCore.OpenApi`

## 8. Database Migration Commands

Run the following commands in the Visual Studio Package Manager Console if you need to recreate or update the database schema.

Create a migration:

```powershell
Add-Migration InitialCreate
```

Apply migrations to the database:

```powershell
Update-Database
```

**Note:** The initial migration has already been created and applied in this project. Do not create another migration with the same name.

## 9. Running the Application

1. Open `StudentManagement.Api` in Visual Studio.
2. Ensure SQL Server Express is running.
3. Build the project using `Ctrl + Shift + B`.
4. Run the application using `Ctrl + F5`.
5. Use Postman to test the API endpoints.

The local development URLs may vary depending on the launch settings.

## 10. API Endpoints

Base URL: `http://localhost:5009`

| Method | Endpoint | Purpose | Expected Status |
|---|---|---|---|
| POST | `/api/Students` | Create a student | 201 Created |
| GET | `/api/Students` | Retrieve all students | 200 OK |
| GET | `/api/Students/{id}` | Retrieve a student by ID | 200 OK / 404 Not Found |
| PUT | `/api/Students/{id}` | Update a student | 204 No Content / 400 / 404 / 409 |
| DELETE | `/api/Students/{id}` | Delete a student | 204 No Content / 404 |
| GET | `/api/Students/search?name=Arun` | Search by name | 200 OK |
| GET | `/api/Students/search?department=IT` | Search by department | 200 OK |

Search can also combine both query parameters:

```text
/api/Students/search?name=Arun&department=IT
```

## 11. Example Requests

### Create a Student

**POST** `/api/Students`

Request body:

```json
{
  "name": "Arun Kumar",
  "email": "arun@example.com",
  "department": "Computer Science",
  "age": 21
}
```

Expected response: `201 Created`, containing the created student and its generated ID.

### Retrieve All Students

**GET** `/api/Students`

Expected response: `200 OK`, containing a JSON array of student records.

### Retrieve a Student by ID

**GET** `/api/Students/1`

Expected response: `200 OK` if the student exists, or `404 Not Found` otherwise.

### Update a Student

**PUT** `/api/Students/1`

Request body:

```json
{
  "id": 1,
  "name": "Arun Kumar Updated",
  "email": "arun@example.com",
  "department": "Information Technology",
  "age": 22
}
```

Expected response: `204 No Content` when the update succeeds.

The ID in the URL must match the ID in the request body.

### Delete a Student

**DELETE** `/api/Students/1`

Expected response: `204 No Content` if the student is deleted successfully.

### Search Students

**GET** `/api/Students/search?name=Arun`

Expected response: `200 OK`, containing matching student records. The search can use the `name` and `department` query parameters.

## 12. Validation and Error Handling

The API validates student input using C# data annotations.

- `400 Bad Request`: Invalid input or validation failure.
- `404 Not Found`: The requested student does not exist.
- `409 Conflict`: Duplicate email address or applicable database conflict.
- `201 Created`: Student created successfully.
- `200 OK`: Successful retrieval.
- `204 No Content`: Successful update or deletion.

Email uniqueness is enforced using a unique database index.

## 13. Testing

The API has been tested using Postman.

Testing includes:

- Creating a student.
- Retrieving all students.
- Retrieving a student by ID.
- Updating student details.
- Deleting a student.
- Searching student records.
- Submitting invalid data.
- Checking duplicate email handling.

The database records can be inspected in SQL Server Management Studio by connecting to `localhost\SQLEXPRESS`, opening `StudentManagementDb`, and viewing the `Students` table.

## 14. OpenAPI Documentation

OpenAPI JSON documentation is available in development mode at:

https://localhost:7113/openapi/v1.json

The HTTPS port may vary according to the project launch settings.

## 15. Learning Outcomes

This project demonstrates:

- Building RESTful APIs using ASP.NET Core controllers.
- Using C# models and data annotations.
- Connecting an API to SQL Server through Entity Framework Core.
- Creating and applying EF Core migrations.
- Implementing asynchronous database operations.
- Performing CRUD operations and searching with LINQ.
- Handling HTTP status codes and validation errors.
- Testing API endpoints using Postman.
