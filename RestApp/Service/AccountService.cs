using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestApp.Authorization;
using RestApp.Entities;
using RestApp.Entities.Users;
using RestApp.Exceptions;
using RestApp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestApp.Services
{
    public interface IAccountService
    {
        public UserDto GetUser(int id);
        public UserDto GetUserByName(string name);
        public UsersDate GetUserDate();
        void RegisterUser(RegisterUserDto dto);
        Token GenerateJwt(LoginDto dto);
        public void EditDateUser(EditUser editUser);
        public void EditPassword(EditPassword editUser);
    }
    public class AccountService : IAccountService
    {
        private readonly AppDbContext context;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly ILogger<AccountService> logger;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;
        private readonly IMapper mapper;

        public AccountService(AppDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings
            , ILogger<AccountService> logger, IAuthorizationService authorizationService, IUserContextService userContextService, IMapper mapper)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
            this.logger = logger;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
            this.mapper = mapper;
        }
        public UserDto GetUserByName(string name)
        {
            var user = context.Users
                .Include(u => u.Role)
                .Include(u => u.Email)
                .FirstOrDefault(u => u.Name.Equals(name));

            if (user is null)
            {
                throw new NotFoundException("Friend not found");
            }

            var userDto = mapper.Map<UserDto>(user);

            return userDto;
        }

        public UserDto GetUser(int id)
        { 
            
            var user = context.Users
                .Include(u => u.Role)               
                .FirstOrDefault(u => u.Id == id);

            var userDto = mapper.Map<UserDto>(user);

            return userDto;
        }

        public UsersDate GetUserDate()
        {
            var usersDate = new UsersDate();
            usersDate.Id = (int)userContextService.GetUserId;
            usersDate.Name = userContextService.GetUserName;
            usersDate.Role = userContextService.GetUserRole;
            return usersDate;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
                RoleId = dto.RoleId,               
            };
            var hashedPassword = passwordHasher.HashPassword(newUser, dto.Password);

            newUser.Password = hashedPassword;
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public Token GenerateJwt(LoginDto dto)
        {
            var user = context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email.Equals(dto.Email));

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            Token tokenValue = new Token() { Value = tokenHandler.WriteToken(token) };
            return tokenValue;

        }

        public void EditDateUser(EditUser editUser)
        {
            var user = context.Users.FirstOrDefault(e => e.Id == editUser.UserId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, editUser,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            if(editUser.Name != null)
            {
                user.Name = editUser.Name;
            }

            if (editUser.Email != null)
            {
                user.Email = editUser.Email;
            }

            context.SaveChanges();
        }

        public void EditPassword(EditPassword editUser)
        {
            var user = context.Users.FirstOrDefault(e => e.Id == editUser.UserId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, editUser,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, editUser.PasswordLast);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid password");
            }

            if (editUser.Password.Equals(editUser.ConfirmPassword) )
            {
               var hashedPassword = passwordHasher.HashPassword(user, editUser.Password);

                user.Password = hashedPassword;
                context.SaveChanges();
            }else
            {
                throw new NotFoundException("Passwords are different");
            }
 
        }
    }
}
