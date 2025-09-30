namespace SurveyManagement.Application.DTOs
{
    // DTO for returning user data (response)
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int RoleId { get; set; }
        public string? RoleName { get; set; }  // Optional, can map from Role entity
        public string Address { get; set; } = default!;
    }
}
