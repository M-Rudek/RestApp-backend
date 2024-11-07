using System.ComponentModel.DataAnnotations;

namespace RestApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
