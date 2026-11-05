using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;

using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Repositories;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;
using MovieTicketBookingSystem.Authorization;

using MovieTicketBookingSystem.Middleware;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. REGISTER REPOSITORIES
// ==========================================

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ==========================================
// 2. REGISTER SERVICES
// ==========================================

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
builder.Services.AddScoped<IScreenService, ScreenService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUserService, UserService>();

// ==========================================
// 3. PASSWORD HASHING
// ==========================================

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// ==========================================
// 4. JWT SERVICE
// ==========================================

builder.Services.AddScoped<IJwtService, JwtService>();

// ==========================================
// 5. AUTHORIZATION HANDLER
// ==========================================

builder.Services.AddScoped<IAuthorizationHandler, BookingOwnerHandler>();

// ==========================================
// 6. AUTHENTICATION - JWT
// ==========================================

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

// ==========================================
// 7. AUTHORIZATION
// ==========================================

builder.Services.AddAuthorization();

// ==========================================
// 8. CONTROLLERS AND OPENAPI
// ==========================================

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ==========================================
// 9. DATABASE CONNECTION
// ==========================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ==========================================
// 10. CORS CONFIGURATION
// ==========================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ==========================================
// 11. BUILD APPLICATION
// ==========================================

var app = builder.Build();

// ==========================================
// 12. DEVELOPMENT CONFIGURATION
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    await RoleSeeder.SeedAsync(app.Services, app.Configuration);
}

// ==========================================
// 13. MIDDLEWARE PIPELINE
// ==========================================

// 1. HTTPS Redirection
app.UseHttpsRedirection();

// 2. CORS
app.UseCors("AngularClient");

// 3. Routing
app.UseRouting();

// 4. Endpoint Metadata Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<EndpointMetadataMiddleware>();
}

// 5. Authentication
app.UseAuthentication();

// 6. Authentication Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();
}

// 7. Authorization Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthorizationDiagnosticsMiddleware>();
}

// 8. Authorization
app.UseAuthorization();

// ==========================================
// 14. MAP CONTROLLERS
// ==========================================

app.MapControllers();

// ==========================================
// 15. RUN APPLICATION
// ==========================================

app.Run();
