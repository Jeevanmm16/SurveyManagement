using Microsoft.EntityFrameworkCore;
using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Infrastructure.Data
{
    public static class DbInitializer
    {

        public static void Seed(SurveyDbContext context)
        {

            // Use migrations instead of EnsureCreated
            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                  
                );
                context.SaveChanges();
            }

            if (!context.Users.Any(u => u.RoleId == 1))
            {
                var adminId = Guid.NewGuid();

                var admin = new User
                {
                    Id = adminId,
                    Name = "Jeevan m m",
                    Email = "Jeevan@Survey.com",
                    Password = "Jeevan123",
                    Address = "Davanagere",
                    RoleId = 1
                };

              

                context.Users.Add(admin);
              

                context.SaveChanges();
            }
        }

    }
}
