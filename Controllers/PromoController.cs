using MassageManagementSystem.Models;
using MassageManagementSystem.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MassageManagementSystem.Services;
using System.Threading.Tasks;
using System.Linq;

namespace MassageManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public PromoController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // POST: /api/promo/create
        [HttpPost("create")]
        public async Task<IActionResult> CreatePromoCode([FromBody] PromoCode promo)
        {
            if (string.IsNullOrWhiteSpace(promo.Code) || promo.DiscountAmount <= 0)
            {
                return BadRequest(new { message = "Code and discount must be provided and valid." });
            }

            var exists = await _context.PromoCodes.AnyAsync(p => p.Code == promo.Code);
            if (exists)
            {
                return BadRequest(new { message = "Promo code already exists." });
            }

            // Created promos are inactive by default
            _context.PromoCodes.Add(promo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Promo code created successfully." });
        }

        [HttpPost("send/{code}")]
        public async Task<IActionResult> SendPromoToMembers(string code)
        {
            var promo = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code && p.IsActive);
            if (promo == null)
                return NotFound(new { message = "Promo code not found or inactive." });

            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "🎉 A Promo Code Just for You!",
                    $"<p>Hi {user.UserName},</p>" +
                    $"<p>Enjoy <strong>{promo.DiscountAmount}% off</strong> your next massage!</p>" +
                    $"<p>Use this code at checkout: <strong>{promo.Code}</strong></p>" +
                    "<p>We hope to see you soon!</p>"
                );
            }

            return Ok(new { message = "Promo code emailed to all users." });
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetAllPromoCodes()
        {
            var promos = await _context.PromoCodes
                .Select(p => new
                {
                    p.Code,
                    p.DiscountAmount,
                    p.IsActive
                })
                .ToListAsync();

            return Ok(promos);
        }

        [HttpPut("update/{code}")]
        public async Task<IActionResult> UpdatePromoCode(string code, [FromBody] PromoCode updated)
        {
            var promo = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code);
            if (promo == null)
                return NotFound(new { message = "Promo code not found." });

            promo.DiscountAmount = updated.DiscountAmount;
            promo.IsActive = updated.IsActive;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Promo code updated successfully." });
        }

        [HttpDelete("delete/{code}")]
        public async Task<IActionResult> DeletePromoCode(string code)
        {
            var promo = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code);
            if (promo == null)
                return NotFound(new { message = "Promo code not found." });

            _context.PromoCodes.Remove(promo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Promo code deleted successfully." });
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePromoCodes()
        {
            var promos = await _context.PromoCodes
                .Where(p => p.IsActive)
                .Select(p => new
                {
                    p.Code,
                    p.DiscountAmount
                })
                .ToListAsync();

            return Ok(promos);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyPromoCode([FromBody] string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest(new { message = "Promo code is required." });
            }

            var promo = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code && p.IsActive);
            if (promo == null)
            {
                return BadRequest(new { message = "Invalid or inactive promo code." });
            }

            return Ok(new
            {
                discount = promo.DiscountAmount,
                message = "Promo code applied successfully."
            });
        }
    }
}