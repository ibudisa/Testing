using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL
{
    public class DBInitializerSeedData
    {
        public static void InitializeDatabase(UserDBContext uDbContext)
        {
            if (uDbContext.WorkPositions.Any())
                return;

            var wpone = new WorkPosition
            {
                Name = "Software developer",
                Description = "Development"

            };
            var wptwo = new WorkPosition
            {
                Name = "Stone mason",
                Description = "Working with stone"

            };
            var wpThree = new WorkPosition
            {
                Name = "Baker",
                Description = "Baking bread"

            };
            uDbContext.WorkPositions.Add(wpone);
            uDbContext.WorkPositions.Add(wptwo);
            uDbContext.WorkPositions.Add(wpThree);

            uDbContext.SaveChanges();

            if (uDbContext.Users.Any())
                return;

            User user1 = new User();
            user1.FirstName = "Mile";
            user1.LastName = "Hekip";
            user1.Email = "mile@aol.com";
            user1.Password = "qwer12";
            User user2 = new User();
            user2.FirstName = "Ana";
            user2.LastName = "Tadić";
            user2.Email = "anat@gmail.com";
            user2.Password = "dfgh345";
            User user3 = new User();
            user3.FirstName = "Ivo";
            user3.LastName = "Derit";
            user3.Email = "iderit@yahoo.com";
            user3.Password = "cvbn1";
            User user4 = new User();
            user4.FirstName = "Damir";
            user4.LastName = "Belas";
            user4.Email = "dbelas@yahoo.com";
            user4.Password = "uiop2";


            User user5 = new User();
            user5.FirstName = "Patrik";
            user5.LastName = "Helod";
            user5.Email = "phelod@vip.hr";
            user5.Password = "qwsdf";

            User user6 = new User();
            user6.FirstName = "Irma";
            user6.LastName = "Feld";
            user6.Email = "ifeld@yahoo.com";
            user6.Password = "xcvbn";

            uDbContext.Users.Add(user1);
            uDbContext.Users.Add(user2);
            uDbContext.Users.Add(user3);
            uDbContext.Users.Add(user4);

            uDbContext.Users.Add(user5);
            uDbContext.Users.Add(user6);
            uDbContext.SaveChanges();


            if (uDbContext.UserRoles.Any())
                return;

            UserRole role1 = new UserRole();
            role1.Value = "Admin";
            UserRole role2 = new UserRole();
            role2.Value = "User";

            uDbContext.UserRoles.Add(role1);
            uDbContext.UserRoles.Add(role2);
            uDbContext.SaveChanges();

            if (uDbContext.User_UserRoles.Any())
                return;

            User_UserRole userRole1 = new User_UserRole();
            userRole1.UserId = 1;
            userRole1.UserRoleId = 1;
            User_UserRole userRole2 = new User_UserRole();
            userRole2.UserId = 2;
            userRole2.UserRoleId = 2;
            User_UserRole userRole3 = new User_UserRole();
            userRole3.UserId = 3;
            userRole3.UserRoleId = 2;
            User_UserRole userRole4 = new User_UserRole();
            userRole4.UserId = 4;
            userRole4.UserRoleId = 1;

            uDbContext.User_UserRoles.Add(userRole1);
            uDbContext.User_UserRoles.Add(userRole2);
            uDbContext.User_UserRoles.Add(userRole3);
            uDbContext.User_UserRoles.Add(userRole4);
            uDbContext.SaveChanges();

            if (uDbContext.User_WorkPositions.Any())
                return;

            User_WorkPosition userWorkPosition1 = new User_WorkPosition();
            userWorkPosition1.UserId = 1;
            userWorkPosition1.WorkPositionId = 1;
            User_WorkPosition userWorkPosition2 = new User_WorkPosition();
            userWorkPosition2.UserId = 2;
            userWorkPosition2.WorkPositionId = 1;
            User_WorkPosition userWorkPosition3 = new User_WorkPosition();
            userWorkPosition3.UserId = 3;
            userWorkPosition3.WorkPositionId = 2;
            User_WorkPosition userWorkPosition4 = new User_WorkPosition();
            userWorkPosition4.UserId = 4;
            userWorkPosition4.WorkPositionId = 3;

            uDbContext.User_WorkPositions.Add(userWorkPosition1);
            uDbContext.User_WorkPositions.Add(userWorkPosition2);
            uDbContext.User_WorkPositions.Add(userWorkPosition3);
            uDbContext.User_WorkPositions.Add(userWorkPosition4);
            uDbContext.SaveChanges();



        }
    }
}
