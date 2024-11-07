using System.ComponentModel.DataAnnotations;

namespace RestApp.Models
{
    public class CreatePostDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        public DateTime CreatedTime = DateTime.Now;
        [Required]
        public int UserId { get; set; }

    }
}
