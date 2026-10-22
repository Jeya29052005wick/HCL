# Training Management System

A full-stack web application for managing training courses, learner registrations, and course enrollments. Built using **ASP.NET Core Web API, C#, HTML, CSS, and JavaScript**, with a responsive dashboard and a cinematic dark-themed user interface.

## Features

### Dashboard
- View total courses and registered learners.
- Track active enrollments and available seats.
- View a summary of training courses.

### Course Management
- Display all available courses.
- Search courses by code or title.
- Sort courses by fee in ascending or descending order.
- Add new courses.
- Track course duration, fees, total seats, and available seats.

### Learner Management
- Register new learners.
- View all registered learners.
- Search learners by ID, name, or email.
- Validate learner information and prevent duplicate learner IDs and email addresses.

### Enrollment Management
- Enroll learners in courses.
- View enrollment records and registration dates.
- Prevent duplicate active enrollments.
- Prevent enrollment when a course has no available seats.
- Cancel enrollments and update seat availability.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Backend programming |
| ASP.NET Core Web API | REST API development |
| HTML5 | Frontend structure |
| CSS3 | Responsive styling and UI design |
| JavaScript | API integration and frontend interactions |
| In-memory collections | Temporary data storage |
| Visual Studio | Development environment |
| Git and GitHub | Version control |

## Project Structure

```text
TrainingManagementApi/
├── Controllers/
│   ├── CoursesController.cs
│   ├── LearnersController.cs
│   └── EnrollmentsController.cs
├── Models/
│   ├── Course.cs
│   ├── Learner.cs
│   └── Enrollment.cs
├── Services/
│   └── TrainingStore.cs
├── wwwroot/
│   ├── index.html
│   ├── style.css
│   └── app.js
├── Program.cs
├── appsettings.json
└── TrainingManagementApi.csproj
```

## Sample Courses

The application starts with five sample training courses.

| Course Code | Course Title | Fee | Duration | Total Seats |
|---|---|---:|---:|---:|
| CS101 | C# Programming | ₹5,000 | 30 days | 40 |
| CS102 | Java Programming | ₹6,000 | 35 days | 35 |
| WD101 | Web Development | ₹4,500 | 25 days | 30 |
| PY101 | Python Programming | ₹5,500 | 30 days | 25 |
| DS101 | Data Science | ₹8,000 | 45 days | 20 |

## Getting Started

### Prerequisites

- Visual Studio
- .NET SDK compatible with the project's target framework
- A modern web browser
- Git (optional)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/Jeya29052005wick/HCL.git
   ```

2. Open the solution or project in Visual Studio.

3. Set `TrainingManagementApi` as the startup project.

4. Build the solution to restore dependencies and verify compilation.

5. Run the application using Visual Studio or the .NET CLI:

   ```bash
   dotnet run
   ```

6. Open the local URL displayed in the terminal, such as:

   ```text
   https://localhost:7102
   ```

   The actual port may differ depending on your launch settings.

## API Endpoints

### Courses

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/courses` | Get all courses |
| GET | `/api/courses?search=Python` | Search courses |
| GET | `/api/courses?sortBy=fee` | Sort by fee, low to high |
| GET | `/api/courses?sortBy=fee-desc` | Sort by fee, high to low |
| GET | `/api/courses/CS101` | Get a course by code |
| POST | `/api/courses` | Add a course |

### Learners

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/learners` | Get all learners |
| GET | `/api/learners?search=John` | Search learners |
| POST | `/api/learners` | Register a learner |

### Enrollments

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/enrollments` | Get enrollment records |
| POST | `/api/enrollments` | Enroll a learner in a course |
| DELETE | `/api/enrollments/{id}` | Cancel an enrollment |

## Example API Requests

### Add a Course

`POST /api/courses`

```json
{
  "courseCode": "CS201",
  "title": "Advanced C#",
  "fee": 7000,
  "durationDays": 30,
  "totalSeats": 25
}
```

### Register a Learner

`POST /api/learners`

```json
{
  "learnerId": "L001",
  "name": "Arun Kumar",
  "contactNumber": "9876543210",
  "email": "arun@example.com"
}
```

### Enroll a Learner

`POST /api/enrollments`

```json
{
  "learnerId": "L001",
  "courseCode": "CS101"
}
```

## UI Highlights

- Cinematic dark theme with midnight backgrounds.
- Indigo and violet gradient accents.
- Glass-inspired cards and subtle glowing borders.
- Smooth transitions and hover animations.
- Responsive layout for desktop, tablet, and mobile screens.
- Dashboard statistics and searchable data tables.

## Data Storage

This version uses in-memory collections to store courses, learners, and enrollment records. Data is temporary and resets when the API application restarts. A database is not currently configured.

## Future Enhancements

- Integrate SQL Server or another persistent database.
- Add edit and delete operations for courses and learners.
- Implement authentication and role-based authorization.
- Add pagination and advanced filtering.
- Add automated unit and integration tests.
- Deploy the application to a cloud hosting platform.

## Learning Outcomes

This project demonstrates:

- RESTful API development using ASP.NET Core.
- Object-oriented programming with C# models and services.
- HTTP methods and JSON-based communication.
- Frontend integration using JavaScript `fetch()`.
- Form validation and error handling.
- CRUD-style workflows and enrollment business rules.
- Responsive UI design and Git-based version control.

## Author

**Jeyachandran**

Developed as a training project to practice C#, ASP.NET Core Web API, frontend development, and full-stack application integration.
