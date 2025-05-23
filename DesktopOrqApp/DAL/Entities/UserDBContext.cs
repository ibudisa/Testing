using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserDBContext:DbContext
    {
        public UserDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User_UserRole> User_UserRoles { get; set; }
        public DbSet<WorkPosition> WorkPositions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User_WorkPosition> User_WorkPositions { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string connectionString = "server=localhost; port=3306; database=userdata; user=root; password=dlswer_2asdlkj; Persist Security Info=False; Connect Timeout=300;";
        //    optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new System.Version(8, 0, 41)));
        //}
    }
}
