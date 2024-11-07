using System.ComponentModel.DataAnnotations;

namespace RestApp.Models
{
    public class CreateCommentDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        
        public DateTime CreatedTime = DateTime.Now;


    }
}
