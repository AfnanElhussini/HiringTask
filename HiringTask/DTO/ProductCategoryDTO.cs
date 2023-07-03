
namespace HiringTask.DTO
{
    public class ProductCategoryDTO
    {
       public int Id { get; set; }
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        
        public string? State { get; set; }

       public int CategoryId { get; set; }
       public string? name { get; set; }

    }

}
