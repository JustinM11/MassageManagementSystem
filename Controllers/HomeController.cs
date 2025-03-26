using MassageManagementSystem.Models;
using MassageManagementSystem.Models.Data;
using MassageManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MassageManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public HomeController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }


        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            List<Therapists> therapists = await _context.Therapists.ToListAsync();
            return View(therapists);
        }

        // GET: /Home/Book
        public async Task<IActionResult> Book()
        {
            List<Therapists> therapists = await _context.Therapists.ToListAsync();
            return View("~/Views/Home/Book.cshtml",therapists);
        }

        // POST: /Home/Book
        [HttpPost]
        public async Task<IActionResult> Book(int therapistId, DateTime appointmentTime)
        {
            // Create a new booking using the posted data.
            Booking booking = new Booking
            {
                UserId = "User123", // In a real app, use the authenticated user's ID.
                TherapistId = therapistId,
                AppointmentTime = appointmentTime,
                IsConfirmed = false
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Send a confirmation email.
            // Replace "customer@example.com" with the actual customer's email address.
            await _emailService.SendEmailAsync(
                "customer@example.com",
                "Booking Confirmation",
                "Your massage booking has been confirmed!"
            );

            TempData["Message"] = "Booking created successfully and email sent!";
            return RedirectToAction("Index");
        }

    }
}
