using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SurveyManagement.Domain.Entities;

namespace SurveyManagement.Infrastructure.Data
{
    public class SurveyDbContext : DbContext
    {
        public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Role> Roles { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Survey> Surveys { get; set; } = default!;
        public DbSet<Question> Questions { get; set; } = default!;
        public DbSet<Option> Options { get; set; } = default!;
        public DbSet<UserSurvey> UserSurveys { get; set; } = default!;
        public DbSet<Response> Responses { get; set; } = default!;
        public DbSet<ResponseOption> ResponseOptions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Role → User (1-M)
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting role from deleting users

            // User → Survey (1-M)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Surveys)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user from deleting surveys

            // Product → Survey (1-M)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Surveys)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting product from deleting surveys

            // Survey → Question (1-M)
            modelBuilder.Entity<Survey>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Survey)
                .HasForeignKey(q => q.SurveyId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting survey deletes its questions

            // Question → Option (1-M)
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting question deletes its options

            // UserSurvey (join table)
            modelBuilder.Entity<UserSurvey>()
                .HasKey(us => us.UserSurveyId);

            modelBuilder.Entity<UserSurvey>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSurveys)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting user deletes related UserSurveys

            modelBuilder.Entity<UserSurvey>()
                .HasOne(us => us.Survey)
                .WithMany(s => s.UserSurveys)
                .HasForeignKey(us => us.SurveyId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting survey deletes related UserSurveys

            // UserSurvey → Response (1-M)
            modelBuilder.Entity<Response>()
                .HasOne(r => r.UserSurvey)
                .WithMany(us => us.Responses)
                .HasForeignKey(r => r.UserSurveyId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting UserSurvey deletes responses

            // Question → Response (1-M)
            modelBuilder.Entity<Response>()
                .HasOne(r => r.Question)
                .WithMany()
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting question from deleting responses

            // ResponseOption (many-to-many between Response ↔ Option)
            modelBuilder.Entity<ResponseOption>()
                .HasKey(ro => new { ro.ResponseId, ro.OptionId });

            modelBuilder.Entity<ResponseOption>()
                .HasOne(ro => ro.Response)
                .WithMany(r => r.ResponseOptions)
                .HasForeignKey(ro => ro.ResponseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResponseOption>()
                .HasOne(ro => ro.Option)
                .WithMany()
                .HasForeignKey(ro => ro.OptionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting option from deleting ResponseOptions

            // Additional configuration: Unique email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            // Additional configuration (optional)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Enforce unique emails


                Type[] entitiesWithAudit = new Type[]
              {
                typeof(User),
                typeof(Product),
                typeof(Survey),
                typeof(Question),
                typeof(UserSurvey),
                typeof(Response),
                typeof(Option),
                 typeof(ResponseOption),
              };

            foreach (var entityType in entitiesWithAudit)
            {
                modelBuilder.Entity(entityType)
                    .Property<DateTime>("CreatedAt")
                    .HasDefaultValueSql("GETDATE()");

                modelBuilder.Entity(entityType)
                    .Property<DateTime>("ModifiedAt")
                    .HasDefaultValueSql("GETDATE()");
            }

            // Example data seeding
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );
        }

        public override int SaveChanges()
        {
            ApplyAuditing();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditing();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditing()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Metadata.FindProperty("CreatedAt") != null)
                        entry.Property("CreatedAt").CurrentValue = now;

                    if (entry.Metadata.FindProperty("ModifiedAt") != null)
                        entry.Property("ModifiedAt").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Metadata.FindProperty("ModifiedAt") != null)
                        entry.Property("ModifiedAt").CurrentValue = now;

                    // Prevent changing CreatedAt
                    if (entry.Metadata.FindProperty("CreatedAt") != null)
                        entry.Property("CreatedAt").IsModified = false;
                }
            }
        }
    }
  }



