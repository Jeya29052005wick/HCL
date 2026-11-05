using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Authorization
{
    public class BookingOwnerHandler
        : AuthorizationHandler<BookingOwnerRequirement, Booking>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            BookingOwnerRequirement requirement,
            Booking booking)
        {
            var userIdClaim = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdClaim, out int userId) &&
                booking.UserId.HasValue &&
                booking.UserId.Value == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}