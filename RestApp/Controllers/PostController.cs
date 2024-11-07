using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestApp.Entities;
using RestApp.Models;
using RestApp.Service;

namespace RestApp.Controllers
{
    [Route("api/Post")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;

        public PostController( IPostService postService )
        {
            this.postService = postService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PostDto>> GetAll() 
        {

            var posts = postService.GetAll();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public ActionResult<PostDto> Get([FromRoute] int id)
        {
            var post = postService.GetById(id);

            return Ok(post);
        }

        [HttpPost]
        public ActionResult CreatePost([FromBody] CreatePostDto dto) {

            var id = postService.Create(dto);

            return Created($"/api/post/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePost([FromRoute] int id)
        {
            postService.Delete(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateComment([FromBody] UpdatePostDto dto, [FromRoute] int id)
        {

            postService.Update(dto, id);

            return Ok();
        }
    }
}
