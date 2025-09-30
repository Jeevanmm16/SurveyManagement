using Microsoft.EntityFrameworkCore;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Data;
using SurveyManagement.Infrastructure.Repository;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace SurveyManagement.Tests.Repository
{
    // Custom DbContext for testing, ignores navigation properties
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

    public class UserRepositoryTests
    {
        private UserRepository GetRepository(out SurveyDbContext context)
        {
            var options = new DbContextOptionsBuilder<SurveyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // fresh DB for each test
                .Options;

            context = new TestSurveyDbContext(options);
            return new UserRepository(context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_User()
        {
            // Arrange
            var repo = GetRepository(out var context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Email = "test@example.com",
                Password = "pass123",
                RoleId = 1,
                Address = "Hyderabad"
            };

            // Act
            var result = await repo.AddAsync(user);

            // Assert
            var userInDb = await context.Users.FindAsync(user.Id);
            userInDb.Should().NotBeNull();
            userInDb!.Name.Should().Be("Test User");

            // Optional: check shadow properties
            var createdAt = context.Entry(userInDb).Property("CreatedAt").CurrentValue;
            var modifiedAt = context.Entry(userInDb).Property("ModifiedAt").CurrentValue;
            createdAt.Should().NotBeNull();
            modifiedAt.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User_When_Exists()
        {
            var repo = GetRepository(out var context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "User1",
                Email = "user1@example.com",
                Password = "pass",
                RoleId = 1,
                Address = "Bangalore"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(user.Id);

            result.Should().NotBeNull();
            result!.Email.Should().Be("user1@example.com");
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Users()
        {
            var repo = GetRepository(out var context);
            context.Users.AddRange(
                new User { Id = Guid.NewGuid(), Name = "A", Email = "a@test.com", Password = "x", RoleId = 1, Address = "Delhi" },
                new User { Id = Guid.NewGuid(), Name = "B", Email = "b@test.com", Password = "y", RoleId = 1, Address = "Chennai" }
            );
            await context.SaveChangesAsync();

            var users = await repo.GetAllAsync();

            users.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_User()
        {
            var repo = GetRepository(out var context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Old Name",
                Email = "old@test.com",
                Password = "123",
                RoleId = 1,
                Address = "Mumbai"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            user.Name = "New Name";
            var updated = await repo.UpdateAsync(user);

            updated.Should().NotBeNull();
            updated!.Name.Should().Be("New Name");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_User()
        {
            var repo = GetRepository(out var context);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Delete Me",
                Email = "delete@test.com",
                Password = "xyz",
                RoleId = 1,
                Address = "Pune"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var deleted = await repo.DeleteAsync(user.Id);

            deleted.Should().NotBeNull();
            (await context.Users.FindAsync(user.Id)).Should().BeNull();
        }
    }
}
