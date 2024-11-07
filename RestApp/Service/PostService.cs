using AutoMapper;
using RestApp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApp.Entities;
using RestApp.Models;
using RestApp.Middleware;
using RestApp.Services;
using Microsoft.AspNetCore.Authorization;
using RestApp.Authorization;

namespace RestApp.Service
{
    public interface IPostService
    {
        int Create(CreatePostDto dto);
        ActionResult<IEnumerable<PostDto>> GetAll();
        PostDto GetById(int id);
        void Delete(int id);
        void Update(UpdatePostDto dto, int id);
    }

    public class PostService : IPostService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<PostService> logger;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public PostService(AppDbContext dbContext, IMapper mapper, ILogger<PostService> logger
             , IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
        }

        public ActionResult<IEnumerable<PostDto>> GetAll()
        {
            var posts = dbContext.Posts
                .Include(p => p.Comments)
                .ToList();

            var postsDto = mapper.Map<List<PostDto>>(posts);

            return postsDto;
        }

        public PostDto GetById(int id)
        {
            var post = dbContext.Posts
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == id);

            if (post is null)
                throw new NotFoundException("Post not found");

            var postDto = mapper.Map<PostDto>(post);
            return postDto;
        }

        public int Create(CreatePostDto dto)
        {
            var post = mapper.Map<Post>(dto);
            post.UserId = (int)userContextService.GetUserId;

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, post,
                 new ResourceOperationRequirement(ResourceOperation.Create)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Posts.Add(post);
            dbContext.SaveChanges();

            return post.Id;
        }

        public void Delete(int id)
        {
            logger.LogWarning($"Comment with id: {id} DELETE action invoked");

            var post = dbContext.Posts
                .FirstOrDefault(p => p.Id == id);

            if (post is null)
                throw new NotFoundException("Post not found");

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, post,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Remove(post);
            dbContext.SaveChanges();
        }

        public void Update(UpdatePostDto dto, int id)
        {
            var post = dbContext.Posts
                .FirstOrDefault(p => p.Id == id);

            if (post is null)
                throw new NotFoundException("Post not found");

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, post,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            post.Title = dto.Title;
            post.Content = dto.Content;

            dbContext.SaveChanges();
        }
    }
}
