using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApp.Entities;
using RestApp.Models;
using RestApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApp.Controllers
{
    [Route("api/Account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("User/{id}")]

        public ActionResult<User> GetUser([FromRoute] int id)
        {
            var user = accountService.GetUser(id);
            return Ok(user);
        }

        [HttpGet("UserByName/{name}")]
        public ActionResult<User> GetUserByName(string name)
        {
            var user = accountService.GetUserByName(name);
            return Ok(user);
        }

        [HttpGet("UserDate")]
        public ActionResult<UsersDate> GetUserDate()
        {
            var user = accountService.GetUserDate();
            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            Token token = accountService.GenerateJwt(dto);
            return Ok(token);
        }       

        [HttpPut("EditDateUser")]
        public ActionResult EditDateUser([FromBody] EditUser editUser)
        {
            accountService.EditDateUser(editUser);
            return Ok();
        }

        [HttpPut("EditPassword")]
        public ActionResult EditPassword([FromBody] EditPassword editUser)
        {
            accountService.EditPassword(editUser);

            return Ok();
        }
    }
}
