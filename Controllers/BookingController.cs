using MassageManagementSystem.Models;
using MassageManagementSystem.Models.Data;
using MassageManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MassageManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public BookingController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Create a new booking.
        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                booking.Email,
                "Booking Confirmation",
                $"Hi {booking.Name},<br><br>Your booking has been confirmed.<br><br>" +
                $"<strong>Payment ID:</strong> {booking.PaymentId}<br>" +
                $"<strong>Payer ID:</strong> {booking.PayerId}<br>" +
                $"<strong>Appointment:</strong> {booking.AppointmentTime:dddd, MMMM dd, yyyy h:mm tt}<br><br>" +
                "Thank you for booking with us!"
            );

            return Ok("Booking created and email sent.");
        }


        // Retrieve bookings for a specific user.
        //[Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings(string userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Therapist)
                .Include(b => b.User) // <-- Make sure this includes the user entity
                .ToListAsync();

            var result = bookings.Select(b => new
            {
                b.Name,
                Email = b.User?.Email ?? "—", // Fallback for guest
                TherapistName = b.Therapist?.Name ?? "N/A",
                b.AppointmentTime,
                b.IsConfirmed,
                b.Id
            });

            return Ok(result);
        }


        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] Booking update)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Therapist)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            booking.IsConfirmed = update.IsConfirmed;
            await _context.SaveChangesAsync();

            var recipientEmail = booking.User?.Email ?? booking.Email;

            if (update.IsConfirmed && !string.IsNullOrWhiteSpace(recipientEmail))
            {
                var emailBody = $@"
            <h3>Thank you for Booking with us, {booking.Name}!</h3>
            <p>Your massage session with <strong>{booking.Therapist?.Name ?? "your therapist"}</strong> is now <strong>Completed</strong>.</p>
            <p><strong>Date:</strong> {booking.AppointmentTime:MMMM dd, yyyy - h:mm tt}</p>
            <p><strong>Status:</strong> ✅ PAID</p>
            <br />
            <p>We appreciate your trust in Healing Massage. Hope to see you again soon!</p>";

                await _emailService.SendEmailAsync(
                    recipientEmail,
                    "✅ Booking Completed – Healing Massage",
                    emailBody
                );
            }

            return Ok(new { message = "Booking status updated successfully." });
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok("Booking deleted successfully.");
        }




    }
}
