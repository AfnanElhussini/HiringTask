namespace HiringTask.DTO
{
    public class CategoryProductDTO
    {
       public int Id { get; set; }
         public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string ? State { get; set; }
        public string ? UserId { get; set; }

        public List<string> Products { get; set; }
    }
}
