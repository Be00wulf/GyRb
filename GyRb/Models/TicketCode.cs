using System.ComponentModel.DataAnnotations;

namespace GyRb.Models
{
    public class TicketCode
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Code { get; set; } = string.Empty;
        public DateTime? UsedAt { get; set; }
        public bool IsUsed => UsedAt.HasValue;
        public int? PostId { get; set; }
        public Post? Post { get; set; }
    }
}
