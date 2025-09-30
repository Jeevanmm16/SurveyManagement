using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SurveyManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyManagement.Application.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> AddUserAsync(UserCreateDto dto);
        Task<UserResponseDto?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> UpdateUserAsync(Guid id, UserUpdateDto dto);
        Task<UserResponseDto?> DeleteUserAsync(Guid id);
    }
}
