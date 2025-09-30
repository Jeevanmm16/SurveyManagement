using NUnit.Framework;
using Moq;
using AutoMapper;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyManagement.Tests.Services
{
    [TestFixture]
    public class SurveyServiceTests
    {
        private Mock<ISurveyRepository> _surveyRepoMock;
        private Mock<IProductRepository> _productRepoMock;
        private IMapper _mapper;
        private SurveyService _service;

        [SetUp]
        public void Setup()
        {
            _surveyRepoMock = new Mock<ISurveyRepository>();
            _productRepoMock = new Mock<IProductRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Survey, SurveyDto>();
                cfg.CreateMap<Survey, SurveyDetailDto>();
                cfg.CreateMap<Question, QuestionDto>();
            });

            _mapper = config.CreateMapper();

            _service = new SurveyService(_surveyRepoMock.Object, _mapper, _productRepoMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnListOfSurveyDto()
        {
            // Arrange
            var surveys = new List<Survey>
            {
                new Survey { SurveyId = Guid.NewGuid(), Title = "Survey 1", UserId = Guid.NewGuid(), ProductId = Guid.NewGuid() },
                new Survey { SurveyId = Guid.NewGuid(), Title = "Survey 2", UserId = Guid.NewGuid(), ProductId = Guid.NewGuid() }
            };
            _surveyRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(surveys);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, ((List<SurveyDto>)result).Count);
        }

        [Test]
        public void GetByIdAsync_ShouldThrowKeyNotFound_WhenSurveyDoesNotExist()
        {
            // Arrange
            var surveyId = Guid.NewGuid();
            _surveyRepoMock.Setup(r => r.GetByIdAsync(surveyId)).ReturnsAsync((Survey)null);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.GetByIdAsync(surveyId));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnSurveyDetailDto_WhenSurveyExists()
        {
            // Arrange
            var surveyId = Guid.NewGuid();
            var survey = new Survey { SurveyId = surveyId, Title = "Survey 1", UserId = Guid.NewGuid(), ProductId = Guid.NewGuid() };
            _surveyRepoMock.Setup(r => r.GetByIdAsync(surveyId)).ReturnsAsync(survey);

            // Act
            var result = await _service.GetByIdAsync(surveyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(surveyId, result.SurveyId);
        }

        [Test]
        public void CreateAsync_ShouldThrowKeyNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var createDto = new CreateSurveyDto { Title = "New Survey", ProductId = Guid.NewGuid() };
            var userId = Guid.NewGuid();

            _productRepoMock.Setup(p => p.GetByIdAsync(createDto.ProductId)).ReturnsAsync((Product)null);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.CreateAsync(createDto, userId));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnSurveyDto_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var createDto = new CreateSurveyDto { Title = "New Survey", ProductId = productId };
            var userId = Guid.NewGuid();

            _productRepoMock.Setup(p => p.GetByIdAsync(productId)).ReturnsAsync(new Product { ProductId = productId, ProductName = "Product 1" });
            _surveyRepoMock.Setup(s => s.AddAsync(It.IsAny<Survey>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(createDto, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(createDto.Title, result.Title);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(productId, result.ProductId);
        }

        [Test]
        public void UpdateAsync_ShouldThrowKeyNotFound_WhenSurveyDoesNotExist()
        {
            // Arrange
            var surveyId = Guid.NewGuid();
            var updateDto = new UpdateSurveyDto { Title = "Updated", ProductId = Guid.NewGuid() };
            _surveyRepoMock.Setup(s => s.GetByIdAsync(surveyId)).ReturnsAsync((Survey)null);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.UpdateAsync(surveyId, updateDto));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedSurveyDto_WhenSurveyExists()
        {
            // Arrange
            var surveyId = Guid.NewGuid();
            var existingSurvey = new Survey { SurveyId = surveyId, Title = "Old Title", UserId = Guid.NewGuid(), ProductId = Guid.NewGuid() };
            var updateDto = new UpdateSurveyDto { Title = "New Title", ProductId = Guid.NewGuid() };

            _surveyRepoMock.Setup(s => s.GetByIdAsync(surveyId)).ReturnsAsync(existingSurvey);
            _surveyRepoMock.Setup(s => s.UpdateAsync(existingSurvey)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(surveyId, updateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updateDto.Title, result.Title);
            Assert.AreEqual(updateDto.ProductId, result.ProductId);
        }

        [Test]
        public void DeleteAsync_ShouldThrowKeyNotFound_WhenSurveyDoesNotExist()
        {
            // Arrange
            var surveyId = Guid.NewGuid();
            _surveyRepoMock.Setup(s => s.GetByIdAsync(surveyId)).ReturnsAsync((Survey)null);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.DeleteAsync(surveyId));
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenSurveyExists()
        {
            // Arrange
            var surveyId = Guid.NewGuid();
            var survey = new Survey { SurveyId = surveyId, Title = "To Delete", UserId = Guid.NewGuid(), ProductId = Guid.NewGuid() };

            _surveyRepoMock.Setup(s => s.GetByIdAsync(surveyId)).ReturnsAsync(survey);
            _surveyRepoMock.Setup(s => s.DeleteAsync(surveyId)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(surveyId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
