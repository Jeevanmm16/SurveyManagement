using Moq;
using NUnit.Framework;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Tests.Services
{
    [TestFixture]
    public class ResponseServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IResponseRepository> _responseRepoMock;
        private IResponseService _service;

        [SetUp]
        public void Setup()
        {
            _responseRepoMock = new Mock<IResponseRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Responses).Returns(_responseRepoMock.Object);

            _service = new ResponseService(_unitOfWorkMock.Object);
        }

        // ---------------------- CREATE TESTS ----------------------

        [Test]
        public async Task CreateAsync_ShouldCreateTextResponse()
        {
            var question = new Question { QuestionType = QuestionType.Text };
            var dto = new CreateResponseDto
            {
                FeedbackText = "Test feedback",
                QuestionId = Guid.NewGuid(),
                UserSurveyId = Guid.NewGuid()
            };

            var result = await _service.CreateAsync(dto, question);

            Assert.AreEqual("Test feedback", result.FeedbackText);
            Assert.IsNull(result.Rating);
            Assert.That(result.OptionIds, Is.Null.Or.Empty); // ✅ updated
        }
        [Test]
        public void CreateAsync_ShouldThrow_WhenTextMissing()
        {
            var question = new Question { QuestionType = QuestionType.Text };
            var dto = new CreateResponseDto { FeedbackText = null, QuestionId = Guid.NewGuid(), UserSurveyId = Guid.NewGuid() };

            Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto, question));
        }

        [Test]
        public async Task CreateAsync_ShouldCreateRatingResponse()
        {
            var question = new Question { QuestionType = QuestionType.Rating };
            var dto = new CreateResponseDto
            {
                Rating = 5,
                QuestionId = Guid.NewGuid(),
                UserSurveyId = Guid.NewGuid()
            };

            var result = await _service.CreateAsync(dto, question);

            Assert.AreEqual(5, result.Rating);
            Assert.That(result.FeedbackText, Is.Null.Or.Empty); // ✅ updated
            Assert.That(result.OptionIds, Is.Null.Or.Empty);    // ✅ updated
        }

        [Test]
        public void CreateAsync_ShouldThrow_WhenRatingInvalid()
        {
            var question = new Question { QuestionType = QuestionType.Rating };
            var dto = new CreateResponseDto { Rating = 10, QuestionId = Guid.NewGuid(), UserSurveyId = Guid.NewGuid() };

            Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto, question));
        }

        [Test]
        public async Task CreateAsync_ShouldCreateMultipleChoiceResponse()
        {
            var question = new Question { QuestionType = QuestionType.Radio };
            var optionId = Guid.NewGuid();
            var dto = new CreateResponseDto { OptionIds = new List<Guid> { optionId }, QuestionId = Guid.NewGuid(), UserSurveyId = Guid.NewGuid() };

            var result = await _service.CreateAsync(dto, question);

            Assert.IsNotNull(result.OptionIds);
            Assert.AreEqual(1, result.OptionIds.Count);
            Assert.Contains(optionId, result.OptionIds);
            Assert.IsNull(result.FeedbackText);
            Assert.IsNull(result.Rating);
        }

        [Test]
        public void CreateAsync_ShouldThrow_WhenMultipleChoiceHasMultipleOptions()
        {
            var question = new Question { QuestionType = QuestionType.Radio };
            var dto = new CreateResponseDto { OptionIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }, QuestionId = Guid.NewGuid(), UserSurveyId = Guid.NewGuid() };

            Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto, question));
        }

        [Test]
        public async Task CreateAsync_ShouldCreateCheckboxResponse()
        {
            var question = new Question { QuestionType = QuestionType.Checkbox };
            var options = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var dto = new CreateResponseDto { OptionIds = options, QuestionId = Guid.NewGuid(), UserSurveyId = Guid.NewGuid() };

            var result = await _service.CreateAsync(dto, question);

            Assert.AreEqual(2, result.OptionIds.Count);
            Assert.IsNull(result.FeedbackText);
            Assert.IsNull(result.Rating);
        }

        // ---------------------- GET TESTS ----------------------

        [Test]
        public async Task GetByIdAsync_ShouldReturnResponse()
        {
            var responseId = Guid.NewGuid();
            var response = new Response { ResponseId = responseId, UserSurveyId = Guid.NewGuid(), QuestionId = Guid.NewGuid() };
            _responseRepoMock.Setup(r => r.GetByIdAsync(responseId)).ReturnsAsync(response);

            var result = await _service.GetByIdAsync(responseId);

            Assert.AreEqual(responseId, result.ResponseId);
        }

        [Test]
        public void GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _responseRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Response)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(id));
        }

        // ---------------------- UPDATE TESTS ----------------------

        [Test]
        public async Task UpdateAsync_ShouldUpdateTextResponse()
        {
            var responseId = Guid.NewGuid();
            var response = new Response { ResponseId = responseId, QuestionId = Guid.NewGuid(), UserSurveyId = Guid.NewGuid() };
            _responseRepoMock.Setup(r => r.GetByIdAsync(responseId)).ReturnsAsync(response);

            var question = new Question { QuestionType = QuestionType.Text };
            var dto = new UpdateResponseDto { FeedbackText = "Updated text" };

            var result = await _service.UpdateAsync(responseId, dto, question);

            Assert.AreEqual("Updated text", result.FeedbackText);
        }

        [Test]
        public void UpdateAsync_ShouldThrow_WhenResponseNotFound()
        {
            var id = Guid.NewGuid();
            _responseRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Response)null);

            var question = new Question { QuestionType = QuestionType.Text };
            var dto = new UpdateResponseDto { FeedbackText = "Updated text" };

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(id, dto, question));
        }

        // ---------------------- DELETE TESTS ----------------------

        [Test]
        public async Task DeleteAsync_ShouldRemoveResponse()
        {
            var responseId = Guid.NewGuid();
            var response = new Response { ResponseId = responseId };
            _responseRepoMock.Setup(r => r.GetByIdAsync(responseId)).ReturnsAsync(response);

            await _service.DeleteAsync(responseId);

            _responseRepoMock.Verify(r => r.Remove(response), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public void DeleteAsync_ShouldThrow_WhenResponseNotFound()
        {
            var id = Guid.NewGuid();
            _responseRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Response)null);

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(id));
        }
    }
}
