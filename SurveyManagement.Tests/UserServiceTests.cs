using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SurveyManagement.Application.DTOs;
using SurveyManagement.Application.Services;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyManagement.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IMapper> _mapperMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task AddUserAsync_Should_Add_New_User_When_Email_Does_Not_Exist()
        {
            // Arrange
            var dto = new UserCreateDto { Name = "Test", Email = "test@example.com", Password = "123", RoleId = 1, Address = "Bangalore" };
            var user = new User { Id = Guid.NewGuid(), Name = dto.Name, Email = dto.Email };
            var responseDto = new UserResponseDto { Id = user.Id, Name = user.Name, Email = user.Email };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());
            _mapperMock.Setup(m => m.Map<User>(dto)).Returns(user);
            _userRepoMock.Setup(r => r.AddAsync(user)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(responseDto);

            // Act
            var result = await _userService.AddUserAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email);
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void AddUserAsync_Should_Throw_When_Email_Already_Exists()
        {
            // Arrange
            var dto = new UserCreateDto { Name = "Test", Email = "test@example.com", Password = "123" };
            var existingUser = new User { Id = Guid.NewGuid(), Email = dto.Email };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User> { existingUser });

            // Act
            Func<Task> act = async () => await _userService.AddUserAsync(dto);

            // Assert
            act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Email already exists");
        }

        [Test]
        public async Task GetUserByIdAsync_Should_Return_User_When_Found()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Name = "Test", Email = "test@example.com" };
            var responseDto = new UserResponseDto { Id = user.Id, Name = user.Name, Email = user.Email };

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(responseDto);

            // Act
            var result = await _userService.GetUserByIdAsync(user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
        }

        [Test]
        public void GetUserByIdAsync_Should_Throw_When_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();
            _userRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

            // Act
            Func<Task> act = async () => await _userService.GetUserByIdAsync(id);

            // Assert
            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"User with Id '{id}' was not found.");
        }

        [Test]
        public async Task UpdateUserAsync_Should_Update_And_Return_User()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new UserUpdateDto { Name = "Updated", Email = "updated@test.com", Address = "New Addr" };

            var existingUser = new User { Id = id, Name = "Old", Email = "old@test.com" };
            var updatedUser = new User { Id = id, Name = dto.Name, Email = dto.Email };
            var responseDto = new UserResponseDto { Id = id, Name = dto.Name, Email = dto.Email };

            _userRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingUser);
            _mapperMock.Setup(m => m.Map(dto, existingUser)).Returns(updatedUser);
            _userRepoMock.Setup(r => r.UpdateAsync(existingUser)).ReturnsAsync(updatedUser);
            _mapperMock.Setup(m => m.Map<UserResponseDto>(updatedUser)).Returns(responseDto);

            // Act
            var result = await _userService.UpdateUserAsync(id, dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Updated");
        }

        [Test]
        public async Task DeleteUserAsync_Should_Delete_And_Return_User()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new User { Id = id, Name = "ToDelete" };
            var responseDto = new UserResponseDto { Id = id, Name = user.Name };

            _userRepoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(responseDto);

            // Act
            var result = await _userService.DeleteUserAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }
    }
}
