using System.ComponentModel.DataAnnotations;

namespace RestApp.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual List<User> User { get; set; }
    }
}
