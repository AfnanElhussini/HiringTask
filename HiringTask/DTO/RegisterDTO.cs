using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HiringTask.DTO
{
    public class RegisterDTO
    {
        public string? ArabicName { get; set; }

        public string? EnglishName { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
