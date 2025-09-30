namespace SurveyManagement.Application.DTOs
{
    // DTO for updating an existing user
    public class UserUpdateDto
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Password { get; set; }  // Optional, only update if provided
        public string Address { get; set; } = default!;
    }
}
