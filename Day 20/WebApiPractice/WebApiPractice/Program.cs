using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApiPractice.Data;
using WebApiPractice.Repositories;
using WebApiPractice.Services;

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

// Build the application
var app = builder.Build();

// 6. Configure OpenAPI and Scalar in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// 7. Configure Middleware
app.UseHttpsRedirection();

app.UseAuthorization();

// 8. Map Controller Endpoints
app.MapControllers();

// 9. Run the application
app.Run();