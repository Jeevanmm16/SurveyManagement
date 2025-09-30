using Moq;
using NUnit.Framework;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Data;
using SurveyManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyManagement.Tests.Services
{
    [TestFixture]
    public class OptionServiceTests
    {
        private Mock<IOptionRepository> _optionRepoMock;
        private SurveyDbContext _context;
        private OptionService _optionService;

        private Question _mcQuestion;
        private Question _textQuestion;

        [SetUp]
        public void Setup()
        {
            // Use InMemory database for SurveyDbContext
            var options = new DbContextOptionsBuilder<SurveyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SurveyDbContext(options);

            // Seed sample questions with required fields
            _mcQuestion = new Question
            {
                QuestionId = Guid.NewGuid(),
                QuestionType = QuestionType.Checkbox,
                QuestionText = "Sample multiple choice question",
                IsMandatory = true,
                SurveyId = Guid.NewGuid(),
                Options = new List<Option>()
            };

            _textQuestion = new Question
            {
                QuestionId = Guid.NewGuid(),
                QuestionType = QuestionType.Text,
                QuestionText = "Sample text question",
                IsMandatory = false,
                SurveyId = Guid.NewGuid(),
                Options = new List<Option>()
            };

            _context.Questions.AddRange(_mcQuestion, _textQuestion);
            _context.SaveChanges();

            // Mock OptionRepository
            _optionRepoMock = new Mock<IOptionRepository>();

            _optionService = new OptionService(_optionRepoMock.Object, _context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task CreateOptionAsync_ShouldCreateOption_WhenQuestionIsValid()
        {
            var dto = new OptionCreateDto
            {
                QuestionId = _mcQuestion.QuestionId,
                OptionValue = "Option 1",
                Order = 1
            };

            _optionRepoMock.Setup(r => r.AddAsync(It.IsAny<Option>())).Returns(Task.CompletedTask);
            _optionRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _optionService.CreateOptionAsync(dto);

            Assert.AreEqual(dto.OptionValue, result.OptionValue);
            Assert.AreEqual(dto.Order, result.Order);
            Assert.AreEqual(dto.QuestionId, result.QuestionId);

            _optionRepoMock.Verify(r => r.AddAsync(It.IsAny<Option>()), Times.Once);
            _optionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void CreateOptionAsync_ShouldThrow_WhenQuestionNotFound()
        {
            var dto = new OptionCreateDto
            {
                QuestionId = Guid.NewGuid(),
                OptionValue = "Option 1",
                Order = 1
            };

            Assert.ThrowsAsync<KeyNotFoundException>(() => _optionService.CreateOptionAsync(dto));
        }

        [Test]
        public void CreateOptionAsync_ShouldThrow_WhenQuestionIsTextOrRating()
        {
            var dto = new OptionCreateDto
            {
                QuestionId = _textQuestion.QuestionId,
                OptionValue = "Option 1",
                Order = 1
            };

            Assert.ThrowsAsync<InvalidOperationException>(() => _optionService.CreateOptionAsync(dto));
        }

        [Test]
        public async Task UpdateOptionAsync_ShouldUpdateOption_WhenOptionExists()
        {
            var option = new Option
            {
                OptionId = Guid.NewGuid(),
                OptionValue = "Old Value",
                Order = 1,
                QuestionId = _mcQuestion.QuestionId
            };

            var dto = new OptionUpdateDto
            {
                OptionId = option.OptionId,
                OptionValue = "New Value"
            };

            _optionRepoMock.Setup(r => r.GetByIdAsync(option.OptionId)).ReturnsAsync(option);
            _optionRepoMock.Setup(r => r.UpdateAsync(option)).Returns(Task.CompletedTask);
            _optionRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _optionService.UpdateOptionAsync(dto);

            Assert.AreEqual("New Value", result.OptionValue);
            _optionRepoMock.Verify(r => r.UpdateAsync(option), Times.Once);
            _optionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void UpdateOptionAsync_ShouldThrow_WhenOptionNotFound()
        {
            var dto = new OptionUpdateDto
            {
                OptionId = Guid.NewGuid(),
                OptionValue = "New Value"
            };

            _optionRepoMock.Setup(r => r.GetByIdAsync(dto.OptionId)).ReturnsAsync((Option)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() => _optionService.UpdateOptionAsync(dto));
        }

        [Test]
        public async Task GetOptionsByQuestionIdAsync_ShouldReturnOptions_WhenExist()
        {
            var questionId = _mcQuestion.QuestionId;

            var options = new List<Option>
         {
             new Option { OptionId = Guid.NewGuid(), OptionValue = "O1", Order = 1, QuestionId = questionId },
             new Option { OptionId = Guid.NewGuid(), OptionValue = "O2", Order = 2, QuestionId = questionId }
         };

            _optionRepoMock.Setup(r => r.GetByQuestionIdAsync(questionId)).ReturnsAsync(options);

            var result = await _optionService.GetOptionsByQuestionIdAsync(questionId);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("O1", result[0].OptionValue);
            Assert.AreEqual("O2", result[1].OptionValue);
        }

        [Test]
        public void GetOptionsByQuestionIdAsync_ShouldThrow_WhenNoOptions()
        {
            var questionId = Guid.NewGuid();
            _optionRepoMock.Setup(r => r.GetByQuestionIdAsync(questionId)).ReturnsAsync(new List<Option>());

            Assert.ThrowsAsync<KeyNotFoundException>(() => _optionService.GetOptionsByQuestionIdAsync(questionId));
        }

        [Test]
        public async Task CreateOptionsBulkAsync_ShouldCreateMultipleOptions_WhenValid()
        {
            var dtos = new List<OptionCreateDto>
         {
             new OptionCreateDto { QuestionId = _mcQuestion.QuestionId, OptionValue = "O1", Order = 1 },
             new OptionCreateDto { QuestionId = _mcQuestion.QuestionId, OptionValue = "O2", Order = 2 }
         };

            _optionRepoMock.Setup(r => r.AddAsync(It.IsAny<Option>())).Returns(Task.CompletedTask);
            _optionRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _optionService.CreateOptionsBulkAsync(dtos);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("O1", result[0].OptionValue);
            Assert.AreEqual("O2", result[1].OptionValue);

            _optionRepoMock.Verify(r => r.AddAsync(It.IsAny<Option>()), Times.Exactly(2));
            _optionRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void CreateOptionsBulkAsync_ShouldThrow_WhenNoOptionsProvided()
        {
            var dtos = new List<OptionCreateDto>();
            Assert.ThrowsAsync<InvalidOperationException>(() => _optionService.CreateOptionsBulkAsync(dtos));
        }
    }
}
