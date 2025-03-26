using MassageManagementSystem.Models;
using MassageManagementSystem.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MassageManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PromoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /api/promo/apply
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyPromoCode([FromBody] string code)
        {
            var promo = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code && p.IsActive);
            if (promo == null)
                return BadRequest("Invalid or inactive promo code.");

            return Ok(new { discount = promo.DiscountAmount });
        }
    }
}
