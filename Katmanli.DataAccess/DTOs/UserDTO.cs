using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.DataAccess.DTOs
{
    public class UserCreate
    {
        public string Username { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public int? RoleId { get; set; }
    }

    public class UserDelete
    {

    }

    public class UserUpdate
    {
        //Username güncellenmemeli
     //   public string Username { get; set; }

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }

      //public string? Password { get; set; }
        public int? RoleId { get; set; }
    }

    public class UserQuery
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Email { get; set; }
        public int RoleId { get; set;}
    }

    public class UserLoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}

