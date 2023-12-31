﻿namespace HiringTask.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? State { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
