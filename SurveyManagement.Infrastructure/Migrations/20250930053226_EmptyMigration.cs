using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmptyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var adminRoleId = 1;
            var userRoleId = 2;

            var users = new (Guid Id, string Name, string Email, string Password, int RoleId, string Address)[]
            {
                (Guid.NewGuid(), "Admin Alpha", "admin1@survey.com", "Admin@123", adminRoleId, "Admin Block A"),
                (Guid.NewGuid(), "Admin Beta", "admin2@survey.com", "Admin@123", adminRoleId, "Admin Block B"),
                (Guid.NewGuid(), "Admin Gamma", "admin3@survey.com", "Admin@123", adminRoleId, "Admin Block C"),

                (Guid.NewGuid(), "User Delta", "user1@survey.com", "User@123", userRoleId, "User Colony A"),
                (Guid.NewGuid(), "User Epsilon", "user2@survey.com", "User@123", userRoleId, "User Colony B"),
                (Guid.NewGuid(), "User Zeta", "user3@survey.com", "User@123", userRoleId, "User Colony C"),
            };

            foreach (var user in users)
            {
                var hash = BCrypt.Net.BCrypt.HashPassword(user.Password);

                migrationBuilder.InsertData(
                    table: "Users",
                    columns: new[] { "Id", "Name", "Email", "Password", "RoleId", "Address" },
                    values: new object[]
                    {
                        user.Id,
                        user.Name,
                        user.Email,
                        hash,
                        user.RoleId,
                        user.Address
                    }
                );
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Users] WHERE Email LIKE '%@survey.com'");
        }
    }
}
