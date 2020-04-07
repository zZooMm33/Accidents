using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accidents.Models.Storage.UserModel
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? DateBirth { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}