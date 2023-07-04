using System.ComponentModel.DataAnnotations;

namespace HiringTask.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string? ArabicName { get; set; }
        [StringLength(50)]
        public string? EnglishName { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreationDate { get; set; }
        public int? UpdateUserId { get; set; }
       
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime? UpdateDate
        {
            get; set;
        }
        public string? State { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
