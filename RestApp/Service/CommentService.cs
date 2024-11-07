using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestApp.Authorization;
using RestApp.Entities;
using RestApp.Exceptions;
using RestApp.Models;
using RestApp.Services;

namespace RestApp.Service
{
    public interface ICommentService
    {
        ActionResult<IEnumerable<CommentDto>> GetAll(int PostId);
        CommentDto GetById(int id);
        int Create(CreateCommentDto dto, int PostId);
        void Delete(int id);
        void Update(UpdateCommentDto dto, int id);
    }

    public class CommentService : ICommentService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<CommentService> logger;
        private readonly IUserContextService userContextService;
        private readonly IAuthorizationService authorizationService;

        public CommentService(AppDbContext dbContext, IMapper mapper, ILogger<CommentService> logger
            , IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
            this.userContextService = userContextService;
            this.authorizationService = authorizationService;
        }

        public ActionResult<IEnumerable<CommentDto>> GetAll(int PostId)
        {
            var comments = dbContext.Comments
                .Where(p => p.Id == PostId);

            var commentsDto = mapper.Map<List<CommentDto>>(comments);

            return commentsDto;
        }

        public CommentDto GetById(int id)
        {
            var comment = dbContext.Comments
                .FirstOrDefault(p => p.Id == id);

            if (comment is null)
                throw new NotFoundException("Comment not found");

            var commentDto = mapper.Map<CommentDto>(comment);
            return commentDto;
        }

        public int Create(CreateCommentDto dto, int PostId)
        {
            var comment = mapper.Map<Comment>(dto);
            comment.UserId = (int)userContextService.GetUserId;
            comment.PostId = PostId;

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, comment,
                new ResourceOperationRequirement(ResourceOperation.Create)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Comments.Add(comment);
            dbContext.SaveChanges();

            return comment.Id;
        }
        public void Delete(int id)
        {
            logger.LogWarning($"Comment with id: {id} DELETE action invoked");

            var comment = dbContext.Comments
                .FirstOrDefault(p => p.Id == id);

            if (comment is null)
                throw new NotFoundException("Comment not found");

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, comment,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Remove(comment);
            dbContext.SaveChanges();
        }

        public void Update(UpdateCommentDto dto, int id)
        {
            var comment = dbContext.Comments
                .FirstOrDefault(p => p.Id == id);

            if (comment is null)
                throw new NotFoundException("Comment not found");

            comment.Title = dto.Title;
            comment.Content = dto.Content;

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, comment,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.SaveChanges();
        }
    }
}
