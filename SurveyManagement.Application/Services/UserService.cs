using AutoMapper;
using SurveyManagement.Application.DTOs;
using SurveyManagement.Application.Services;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserResponseDto> AddUserAsync(UserCreateDto dto)
    {
        var existingUser = await _userRepository.GetAllAsync();
        if (existingUser.Any(u => u.Email == dto.Email))
            throw new InvalidOperationException("Email already exists");

        var user = _mapper.Map<User>(dto);
        var createdUser = await _userRepository.AddAsync(user);
        return _mapper.Map<UserResponseDto>(createdUser);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException($"User with Id '{id}' was not found.");

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto?> UpdateUserAsync(Guid id, UserUpdateDto dto)
    {
        // Get the existing user from repository
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return null; // Will return 404 in controller

        // Map only the updatable fields from DTO to the existing entity
        _mapper.Map(dto, existingUser); // AutoMapper will copy Name, Email, Address, Password if provided

        // Update in repository
        var updatedUser = await _userRepository.UpdateAsync(existingUser);

        return _mapper.Map<UserResponseDto>(updatedUser);
    }


    public async Task<UserResponseDto?> DeleteUserAsync(Guid id)
    {
        var deletedUser = await _userRepository.DeleteAsync(id);
        if (deletedUser == null)
            throw new KeyNotFoundException($"User with Id '{id}' was not found.");
        return _mapper.Map<UserResponseDto>(deletedUser);
    }
}
