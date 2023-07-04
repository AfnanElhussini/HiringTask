using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HiringTask.Models
{
    public class User : IdentityUser
    {
      
      
        public string? ArabicName { get; set; }

        public string? EnglishName { get; set; }
     
   
        //Realtionships
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
