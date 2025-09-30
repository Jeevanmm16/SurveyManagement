using AutoMapper;
using SurveyManagement.Application.DTOs;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Application.Mappings
{

   
        public class UserProfile : Profile
        {
            public UserProfile()
            {
                // Map User -> UserResponseDto
                CreateMap<User, UserResponseDto>()
                    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));

                // Map UserCreateDto -> User
                CreateMap<UserCreateDto, User>();

                // Map UserUpdateDto -> User
                CreateMap<UserUpdateDto, User>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                  CreateMap<Product, ProductDto>().ReverseMap();
               CreateMap<CreateProductDto, Product>();
               CreateMap<UpdateProductDto, Product>();
            // The condition ensures null values in UpdateDto don't overwrite existing values

            CreateMap<Question, QuestionDto>();
            CreateMap<Question, QuestionDetailDto>();
            CreateMap<Option, OptionDto>();

            CreateMap<Option, OptionDto1>().ReverseMap(); // Option ↔ OptionDto1
            CreateMap<OptionCreateDto, Option>();        // OptionCreateDto → Option
            CreateMap<OptionUpdateDto, Option>();




            CreateMap<Survey, SurveyDto>();

            // Survey → SurveyDetailDto (GetById)
            CreateMap<Survey, SurveyDetailDto>();

            // Question → QuestionDto
            CreateMap<Question, QuestionDto1>();

            // CreateSurveyDto → Survey
            CreateMap<CreateSurveyDto, Survey>()
                .ForMember(dest => dest.SurveyId, opt => opt.Ignore()) // set in service
                .ForMember(dest => dest.UserId, opt => opt.Ignore())   // set from logged-in user
                .ForMember(dest => dest.Questions, opt => opt.Ignore()) // questions not created here
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // UpdateSurveyDto → Survey
            CreateMap<UpdateSurveyDto, Survey>()
                .ForMember(dest => dest.SurveyId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());



            CreateMap<UserSurvey, UserSurveyDto>();

            // DTO → Entity (for creation only)
            CreateMap<UserSurveyCreateDto, UserSurvey>()
                .ForMember(dest => dest.UserSurveyId, opt => opt.Ignore()) // we generate new Guid in service
                .ForMember(dest => dest.User, opt => opt.Ignore())          // navigation handled separately
                .ForMember(dest => dest.Survey, opt => opt.Ignore())        // navigation handled separately
                .ForMember(dest => dest.Responses, opt => opt.Ignore());
        }
        }
    }


