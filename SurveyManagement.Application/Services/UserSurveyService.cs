using AutoMapper;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;

public class UserSurveyService : IUserSurveyService
{
    private readonly IUserSurveyRepository _userSurveyRepo;
    private readonly IUserRepository _userRepo;
    private readonly ISurveyRepository _surveyRepo;
    private readonly IMapper _mapper;

    public UserSurveyService(
        IUserSurveyRepository userSurveyRepo,
        IUserRepository userRepo,
        ISurveyRepository surveyRepo,
        IMapper mapper)
    {
        _userSurveyRepo = userSurveyRepo;
        _userRepo = userRepo;
        _surveyRepo = surveyRepo;
        _mapper = mapper;
    }

    public async Task<UserSurveyDto> CreateAsync(UserSurveyCreateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(dto.UserId);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");

        // 🔹 Block Admin (RoleId = 1)
        if (user.RoleId == 1)
            throw new InvalidOperationException("Admin users are not eligible to take surveys.");

        var survey = await _surveyRepo.GetByIdAsync(dto.SurveyId);
        if (survey == null)
            throw new KeyNotFoundException($"Survey with ID {dto.SurveyId} not found.");

        var entity = new UserSurvey
        {
            UserSurveyId = Guid.NewGuid(),
            UserId = dto.UserId,
            SurveyId = dto.SurveyId
        };

        await _userSurveyRepo.AddAsync(entity);
        return _mapper.Map<UserSurveyDto>(entity);
    }

    public async Task<UserSurveyDto> GetByIdAsync(Guid id)
    {
        var entity = await _userSurveyRepo.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"UserSurvey with ID {id} not found.");
        return _mapper.Map<UserSurveyDto>(entity);
    }

    public async Task<IEnumerable<UserSurveyDto>> GetAllAsync()
    {
        var list = await _userSurveyRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<UserSurveyDto>>(list);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _userSurveyRepo.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"UserSurvey with ID {id} not found.");

        await _userSurveyRepo.DeleteAsync(entity);
    }
}