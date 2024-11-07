using AutoMapper;
using RestApp.Entities;
using RestApp.Entities.Users;
using RestApp.Models;

namespace RestApp
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<Comment, CommentDto>();

            CreateMap<CreatePostDto, Post>();
            CreateMap<CreateCommentDto, Comment>();

            CreateMap<UpdatePostDto, Post>();
            CreateMap<UpdateCommentDto, Comment>();

            CreateMap<User, UserDto>()
                .ForMember(m => m.Role, c => c.MapFrom(s => s.Role.Name));
        }
    }
}
