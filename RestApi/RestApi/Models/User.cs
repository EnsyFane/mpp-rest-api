using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string EnctyptedPassword { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}