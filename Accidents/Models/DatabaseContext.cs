using Accidents.Models.Storage;
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
        public DatabaseContext() : base("Accidents.Db") { this.Configuration.UseDatabaseNullSemantics = true; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Danger> Dangers { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<SourceDanger> SourceDangers { get; set; }
        public DbSet<Accident> Accidents { get; set; }
    }
}