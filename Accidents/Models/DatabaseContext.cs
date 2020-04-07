using Accidents.Models.Storage.UserModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accidents.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("Database") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}