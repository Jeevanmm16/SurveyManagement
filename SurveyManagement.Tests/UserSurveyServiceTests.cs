using AutoMapper;
using Moq;
using NUnit.Framework;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;

namespace SurveyManagement.Tests.Services
{
    [TestFixture]
    public class UserSurveyServiceTests
    {
        private Mock<IUserSurveyRepository> _userSurveyRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ISurveyRepository> _surveyRepoMock;
        private IMapper _mapper;
        private UserSurveyService _service;

        [SetUp]
        public void Setup()
        {
            _userSurveyRepoMock = new Mock<IUserSurveyRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _surveyRepoMock = new Mock<ISurveyRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserSurvey, UserSurveyDto>();
            });
            _mapper = config.CreateMapper();

            _service = new UserSurveyService(
                _userSurveyRepoMock.Object,
                _userRepoMock.Object,
                _surveyRepoMock.Object,
                _mapper
            );
        }

        // ---------------------------
        // CREATE TESTS
        // ---------------------------

        [Test]
        public void CreateAsync_ShouldThrow_WhenUserNotFound()
        {
            var dto = new UserSurveyCreateDto { UserId = Guid.NewGuid(), SurveyId = Guid.NewGuid() };
            _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId))
                         .ReturnsAsync((User?)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_ShouldThrow_WhenUserIsAdmin()
        {
            var dto = new UserSurveyCreateDto { UserId = Guid.NewGuid(), SurveyId = Guid.NewGuid() };

            _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId))
                         .ReturnsAsync(new User { Id = dto.UserId, RoleId = 1 });

            _surveyRepoMock.Setup(r => r.GetByIdAsync(dto.SurveyId))
                           .ReturnsAsync(new Survey { SurveyId = dto.SurveyId });

            Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        }

        [Test]
        public void CreateAsync_ShouldThrow_WhenSurveyNotFound()
        {
            var dto = new UserSurveyCreateDto { UserId = Guid.NewGuid(), SurveyId = Guid.NewGuid() };

            _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId))
                         .ReturnsAsync(new User { Id = dto.UserId, RoleId = 2 });

            _surveyRepoMock.Setup(r => r.GetByIdAsync(dto.SurveyId))
                           .ReturnsAsync((Survey?)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnDto_WhenValid()
        {
            var dto = new UserSurveyCreateDto { UserId = Guid.NewGuid(), SurveyId = Guid.NewGuid() };

            _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId))
                         .ReturnsAsync(new User {Id = dto.UserId, RoleId = 2 });

            _surveyRepoMock.Setup(r => r.GetByIdAsync(dto.SurveyId))
                           .ReturnsAsync(new Survey { SurveyId = dto.SurveyId });

            _userSurveyRepoMock.Setup(r => r.AddAsync(It.IsAny<UserSurvey>()))
                               .ReturnsAsync((UserSurvey us) => us);

            var result = await _service.CreateAsync(dto);

            Assert.That(result.UserId, Is.EqualTo(dto.UserId));
            Assert.That(result.SurveyId, Is.EqualTo(dto.SurveyId));
        }

        // ---------------------------
        // GETBYID TESTS
        // ---------------------------

        [Test]
        public void GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _userSurveyRepoMock.Setup(r => r.GetByIdAsync(id))
                               .ReturnsAsync((UserSurvey?)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(id));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnDto_WhenFound()
        {
            var id = Guid.NewGuid();
            var userSurvey = new UserSurvey
            {
                UserSurveyId = id,
                UserId = Guid.NewGuid(),
                SurveyId = Guid.NewGuid()
            };

            _userSurveyRepoMock.Setup(r => r.GetByIdAsync(id))
                               .ReturnsAsync(userSurvey);

            var result = await _service.GetByIdAsync(id);

            Assert.That(result.UserSurveyId, Is.EqualTo(id));
        }

        // ---------------------------
        // GETALL TESTS
        // ---------------------------

        [Test]
        public async Task GetAllAsync_ShouldReturnDtos()
        {
            var list = new List<UserSurvey>
            {
                new UserSurvey { UserSurveyId = Guid.NewGuid(), UserId = Guid.NewGuid(), SurveyId = Guid.NewGuid() },
                new UserSurvey { UserSurveyId = Guid.NewGuid(), UserId = Guid.NewGuid(), SurveyId = Guid.NewGuid() }
            };

            _userSurveyRepoMock.Setup(r => r.GetAllAsync())
                               .ReturnsAsync(list);

            var result = await _service.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        // ---------------------------
        // DELETE TESTS
        // ---------------------------

        [Test]
        public void DeleteAsync_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _userSurveyRepoMock.Setup(r => r.GetByIdAsync(id))
                               .ReturnsAsync((UserSurvey?)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(id));
        }

        [Test]
        public async Task DeleteAsync_ShouldCallRepository_WhenFound()
        {
            var id = Guid.NewGuid();
            var userSurvey = new UserSurvey { UserSurveyId = id };

            _userSurveyRepoMock.Setup(r => r.GetByIdAsync(id))
                               .ReturnsAsync(userSurvey);

            await _service.DeleteAsync(id);

            _userSurveyRepoMock.Verify(r => r.DeleteAsync(userSurvey), Times.Once);
        }
    }
}