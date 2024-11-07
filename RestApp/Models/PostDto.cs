using RestApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace RestApp.Models
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual List<CommentDto> Comments { get; set; }

        public string UserName { get; set; }
    }
}
