using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBookingSystem.Data;

namespace MovieTicketBooking.Repositories;

public class UserRepository(ApplicationDbContext db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email)
    {
        return db.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public Task AddAsync(User user)
    {
        return db.Users.AddAsync(user).AsTask();
    }

    public Task SaveChangesAsync()
    {
        return db.SaveChangesAsync();
    }
}