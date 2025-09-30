using Application.Services;
using AutoMapper;
using Moq;
using NUnit.Framework;
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
    public class QuestionServiceTests
    {
        private Mock<IQuestionRepository> _repoMock = null!;
        private IMapper _mapper = null!;
        private QuestionService _service = null!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IQuestionRepository>();

            // AutoMapper configuration
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionDto>().ReverseMap();
                cfg.CreateMap<Question, QuestionDetailDto>().ReverseMap();
            });
            _mapper = mapperConfig.CreateMapper();

            _service = new QuestionService(_repoMock.Object, _mapper);
        }

        [Test]
        public async Task GetAll_ShouldReturnQuestions()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Question>
            {
                new Question
                {
                    QuestionId = Guid.NewGuid(),
                    QuestionText = "What is your favorite color?",
                    QuestionType = QuestionType.Checkbox,
                    IsMandatory = true,
                    SurveyId = Guid.NewGuid()
                }
            });

            var result = await _service.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetById_ShouldThrow_WhenQuestionDoesNotExist()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Question?)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.GetByIdAsync(id));
        }

        [Test]
        public async Task GetById_ShouldReturnQuestionDetail()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Question
            {
                QuestionId = id,
                QuestionText = "Select your favorite fruit",
                QuestionType = QuestionType.Radio,
                IsMandatory = false,
                SurveyId = Guid.NewGuid()
            });

            var result = await _service.GetByIdAsync(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result!.QuestionId);
        }

        [Test]
        public void Create_ShouldThrowException_WhenSurveyIdInvalid()
        {
            _repoMock.Setup(r => r.SurveyExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var dto = new CreateQuestionDto
            {
                QuestionText = "Test Question",
                QuestionType = QuestionType.Text,
                IsMandatory = true,
                SurveyId = Guid.NewGuid()
            };

            Assert.ThrowsAsync<Exception>(async () => await _service.CreateAsync(dto));
        }

        [Test]
        public async Task Create_ShouldReturnQuestion_WhenSurveyIdValid()
        {
            var surveyId = Guid.NewGuid();
            _repoMock.Setup(r => r.SurveyExistsAsync(surveyId)).ReturnsAsync(true);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Question>())).ReturnsAsync((Question q) => q);

            var dto = new CreateQuestionDto
            {
                QuestionText = "Enter your age",
                QuestionType = QuestionType.Text,
                IsMandatory = false,
                SurveyId = surveyId
            };

            var result = await _service.CreateAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(dto.QuestionText, result!.QuestionText);
            Assert.AreEqual(dto.QuestionType, result.QuestionType);
            Assert.AreEqual(dto.SurveyId, result.SurveyId);
        }

        [Test]
        public async Task Update_ShouldReturnUpdatedQuestion_WhenQuestionExists()
        {
            var id = Guid.NewGuid();
            var question = new Question
            {
                QuestionId = id,
                QuestionText = "Old Text",
                QuestionType = QuestionType.Text,
                IsMandatory = false,
                SurveyId = Guid.NewGuid()
            };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(question);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Question>())).ReturnsAsync((Question q) => q);

            var updateDto = new UpdateQuestionDto
            {
                QuestionText = "New Text",
                IsMandatory = true
            };

            var result = await _service.UpdateAsync(id, updateDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(updateDto.QuestionText, result!.QuestionText);
            Assert.AreEqual(updateDto.IsMandatory, result.IsMandatory);
            Assert.AreEqual(QuestionType.Text, result.QuestionType); // QuestionType unchanged
        }

        [Test]
        public void Update_ShouldThrow_WhenQuestionDoesNotExist()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Question?)null);

            var updateDto = new UpdateQuestionDto
            {
                QuestionText = "New Text",
                IsMandatory = true
            };

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _service.UpdateAsync(id, updateDto));
        }

        [Test]
        public async Task Delete_ShouldReturnTrue_WhenQuestionExists()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task Delete_ShouldReturnFalse_WhenQuestionDoesNotExist()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

            var result = await _service.DeleteAsync(id);

            Assert.IsFalse(result);
        }
    }
}
