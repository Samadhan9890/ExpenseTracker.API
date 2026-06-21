namespace ExpenseTracker.Services.Models.DTOs
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int? EntryId { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? DeleteId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool? Status { get; set; }
        public int? BuId { get; set; }
        public string? RoleDescription { get; set; }
    }
}
