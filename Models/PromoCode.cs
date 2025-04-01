namespace MassageManagementSystem.Models
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int DiscountAmount { get; set; }
        // Set default status to false (inactive)
        public bool IsActive { get; set; } = false;
    }
}
