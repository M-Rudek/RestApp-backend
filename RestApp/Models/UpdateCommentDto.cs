using System.ComponentModel.DataAnnotations;

namespace RestApp.Models
{
    public class UpdateCommentDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

    }
}
