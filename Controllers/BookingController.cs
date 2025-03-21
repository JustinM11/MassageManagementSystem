﻿using MassageManagementSystem.Models;
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

            // Send a confirmation email (replace with actual user email).
            await _emailService.SendEmailAsync("user@example.com", "Booking Confirmation", "Your booking has been confirmed.");
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
    }
}
