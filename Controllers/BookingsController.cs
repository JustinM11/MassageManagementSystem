using MassageManagementSystem.Models.Data;
using MassageManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize] // Requires login
public class BookingsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BookingsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var therapists = await _context.Therapists.ToListAsync();
        return View(therapists);
    }
    [HttpPost]
    public async Task<IActionResult> Book(int therapistId, DateTime appointmentTime)
    {
        var user = await _userManager.GetUserAsync(User);
        var booking = new Booking
        {
            UserId = user.Id,
            TherapistId = therapistId,
            AppointmentTime = appointmentTime,
            IsConfirmed = false // Pending payment
        };
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Simulate PayPal payment (replace with real API call)
        bool paymentSuccess = SimulatePayment();
        if (paymentSuccess)
        {
            booking.IsConfirmed = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return Content("Payment failed.");
    }

    private bool SimulatePayment()
    {
        // Placeholder: In reality, use PayPal SDK
        return true; // Assume success for now
    }
}