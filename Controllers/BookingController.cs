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
                .Include(b => b.Therapist) // This brings in the related therapist
                .ToListAsync();

            var result = bookings.Select(b => new
            {
                b.Name,
                b.UserId,
                b.AppointmentTime,
                b.IsConfirmed,
                TherapistName = b.Therapist != null ? b.Therapist.Name : "Unknown"
            });

            return Ok(result);
        }


    }
}
