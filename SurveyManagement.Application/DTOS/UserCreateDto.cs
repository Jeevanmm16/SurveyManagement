using System;
using System.Collections.Generic;

namespace SurveyManagement.Application.DTOs
{
    // DTO for creating a new user
    public class UserCreateDto
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int RoleId { get; set; }
        public string Address { get; set; } = default!;
    }
}



