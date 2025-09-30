using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using global::SurveyManagement.Application.DTOS;
using global::SurveyManagement.Domain.Entities;
using global::SurveyManagement.Infrastructure.Repository;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyManagement.Application.Services
{

        public class SurveyService : ISurveyService
        {
            private readonly ISurveyRepository _surveyRepository;
       
            private readonly IMapper _mapper;
        private readonly IProductRepository productRepository;

        public SurveyService(ISurveyRepository surveyRepository, IMapper mapper, IProductRepository productRepository)
            {
                _surveyRepository = surveyRepository;
                _mapper = mapper;
            this.productRepository = productRepository;
        }

            public async Task<IEnumerable<SurveyDto>> GetAllAsync()
            {
                var surveys = await _surveyRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<SurveyDto>>(surveys);
            }

            public async Task<SurveyDetailDto?> GetByIdAsync(Guid id)
            {
                var survey = await _surveyRepository.GetByIdAsync(id);
            if(survey == null)
            {
                throw new KeyNotFoundException($"Survey {id} not exist");
            }
            return _mapper.Map<SurveyDetailDto>(survey);
            }

        public async Task<SurveyDto> CreateAsync(CreateSurveyDto dto, Guid userId)
        {
            // ✅ Ensure product exists before creating survey
            var product = await productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {dto.ProductId} was not found.");

            var survey = new Survey
            {
                SurveyId = Guid.NewGuid(),
                Title = dto.Title,
                UserId = userId,
                ProductId = dto.ProductId
            };

            await _surveyRepository.AddAsync(survey);
            return _mapper.Map<SurveyDto>(survey);
        }

        public async Task<SurveyDto?> UpdateAsync(Guid id, UpdateSurveyDto dto)
            {
                var survey = await _surveyRepository.GetByIdAsync(id);
            if (survey == null)
            {
                throw new KeyNotFoundException($"Survey {id} not exist");
            }

            survey.Title = dto.Title;
                survey.ProductId = dto.ProductId;

                await _surveyRepository.UpdateAsync(survey);
                return _mapper.Map<SurveyDto>(survey);
            }

            public async Task<bool> DeleteAsync(Guid id)
            {
                var survey = await _surveyRepository.GetByIdAsync(id);
            if (survey == null)
            {
                throw new KeyNotFoundException($"Survey {id} not exist");
            }

            await _surveyRepository.DeleteAsync(id);
                return true;
            }
        }
    }

