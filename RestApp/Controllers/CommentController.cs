using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApp.Entities;
using RestApp.Models;
using RestApp.Service;
using System.Xml.Linq;

namespace RestApp.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet("post/{postId}/comment")]
        public ActionResult GetAll([FromRoute] int postId)
        {
            var comments = commentService.GetAll(postId);

            return Ok(comments);
        }

        [HttpGet("comment/{id}")]
        public ActionResult Get([FromRoute] int id)
        {
            var comment = commentService.GetById(id);

            return Ok(comment);
        }

        [HttpPost("post/{postId}/comment")]
        public ActionResult CreateComment([FromBody] CreateCommentDto dto, [FromRoute] int postId)
        {

            var id = commentService.Create(dto, postId);

            return Created($"api/post/{postId}/comment/{id}", null);
        }

        [HttpDelete("comment/{id}")]
        [Authorize(Roles = "Admin,User")]
        public ActionResult DeleteComment([FromRoute] int id)
        {
            commentService.Delete(id);

            return NoContent();
        }

        [HttpPut("comment/{id}")]
        public ActionResult UpdateComment([FromBody] UpdateCommentDto dto, [FromRoute] int id)
        {

            commentService.Update(dto, id);

            return Ok();
        }
    }
}
