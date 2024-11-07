using RestApp.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace RestApp.Entities
{
    public class Post : IContent
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }

        public virtual List<Comment> Comments { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}
