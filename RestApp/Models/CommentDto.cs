using RestApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace RestApp.Models
{
    public class CommentDto
    {
        [Required]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
