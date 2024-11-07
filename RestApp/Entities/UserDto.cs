using RestApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApp.Entities.Users
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string HashPassword { get; set; }
        
        public string Email { get; set; }
      
        public string Role { get; set; }

    }
}
