using Microsoft.EntityFrameworkCore;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Data;
using SurveyManagement.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyManagement.Application.Services
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _optionRepo;
        private readonly SurveyDbContext _context;

        public OptionService(IOptionRepository optionRepo, SurveyDbContext context)
        {
            _optionRepo = optionRepo;
            _context = context;
        }

        public async Task<OptionDto1> CreateOptionAsync(OptionCreateDto dto)
        {
            var question = await _context.Questions
                                         .FirstOrDefaultAsync(q => q.QuestionId == dto.QuestionId);

            if (question == null)
                throw new KeyNotFoundException("Question not found.");

            if (question.QuestionType == QuestionType.Text || question.QuestionType == QuestionType.Rating)
                throw new InvalidOperationException("Cannot add options to Text or Rating questions.");

            var option = new Option
            {
                OptionId = Guid.NewGuid(),
                OptionValue = dto.OptionValue,
                Order = dto.Order,
                QuestionId = dto.QuestionId
            };

            await _optionRepo.AddAsync(option);
            await _optionRepo.SaveChangesAsync();

            return new OptionDto1
            {
                OptionId = option.OptionId,
                OptionValue = option.OptionValue,
                Order = option.Order,
                QuestionId = option.QuestionId
            };
        }

        public async Task<List<OptionDto1>> CreateOptionsBulkAsync(List<OptionCreateDto> dtos)
        {
            if (!dtos.Any())
                throw new InvalidOperationException("No options provided.");

            var questionId = dtos.First().QuestionId;
            var question = await _context.Questions
                                         .FirstOrDefaultAsync(q => q.QuestionId == questionId);

            if (question == null)
                throw new KeyNotFoundException("Question not found.");

            if (question.QuestionType == QuestionType.Text || question.QuestionType == QuestionType.Rating)
                throw new InvalidOperationException("Cannot add options to Text or Rating questions.");

            var options = dtos.Select(dto => new Option
            {
                OptionId = Guid.NewGuid(),
                OptionValue = dto.OptionValue,
                Order = dto.Order,
                QuestionId = dto.QuestionId
            }).ToList();

            foreach (var option in options)
                await _optionRepo.AddAsync(option);

            await _optionRepo.SaveChangesAsync();

            return options.Select(o => new OptionDto1
            {
                OptionId = o.OptionId,
                OptionValue = o.OptionValue,
                Order = o.Order,
                QuestionId = o.QuestionId
            }).ToList();
        }

        public async Task<OptionDto1> UpdateOptionAsync(OptionUpdateDto dto)
        {
            var option = await _optionRepo.GetByIdAsync(dto.OptionId);
            if (option == null)
                throw new KeyNotFoundException("Option not found.");

            option.OptionValue = dto.OptionValue;

            await _optionRepo.UpdateAsync(option);
            await _optionRepo.SaveChangesAsync();

            return new OptionDto1
            {
                OptionId = option.OptionId,
                OptionValue = option.OptionValue,
                Order = option.Order,
                QuestionId = option.QuestionId
            };
        }

        public async Task<List<OptionDto1>> GetOptionsByQuestionIdAsync(Guid questionId)
        {
            var options = await _optionRepo.GetByQuestionIdAsync(questionId);
            if(options == null || !options.Any())
                throw new KeyNotFoundException("No options found for the given question.");
            return options.Select(o => new OptionDto1
            {
                OptionId = o.OptionId,
                OptionValue = o.OptionValue,
                Order = o.Order,
                QuestionId = o.QuestionId
            }).ToList();
        }
    }
}
