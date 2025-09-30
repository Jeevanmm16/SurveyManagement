using Microsoft.EntityFrameworkCore;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepository.Tests
{
    public class TestSurveyDbContext : SurveyDbContext
    {
        public TestSurveyDbContext(DbContextOptions<SurveyDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore navigation properties to avoid shadow property issues
            modelBuilder.Entity<User>().Ignore(u => u.Role);
            modelBuilder.Entity<User>().Ignore(u => u.Surveys);
            modelBuilder.Entity<User>().Ignore(u => u.UserSurveys);
        }
    }

}
