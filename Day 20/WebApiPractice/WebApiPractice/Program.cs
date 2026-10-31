using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApiPractice.Data;
using WebApiPractice.Repositories;
using WebApiPractice.Services;
using WebApiPractice.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Controllers
builder.Services.AddControllers();

// 2. Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Register OpenAPI
builder.Services.AddOpenApi();

// 4. Register Service Layer
builder.Services.AddScoped<IStudentService, StudentService>();

// 5. Register Repository Layer
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// 6. Register Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// 7. Register Problem Details
builder.Services.AddProblemDetails();

// Build the application
var app = builder.Build();

// 8. Configure Global Exception Handling
app.UseExceptionHandler();

// 9. Configure OpenAPI and Scalar in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// 10. Configure Middleware
app.UseHttpsRedirection();

app.UseAuthorization();

// 11. Map Controller Endpoints
app.MapControllers();

// 12. Run the application
app.Run();