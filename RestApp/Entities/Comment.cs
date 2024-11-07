using RestApp.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace RestApp.Entities
{
    public class Comment : IContent
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}
