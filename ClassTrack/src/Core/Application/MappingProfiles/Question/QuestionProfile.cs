using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.MappingProfiles
{
    internal class QuestionProfile:Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, GetQuestionItemDTO>()
                .ForCtorParam(nameof(GetQuestionItemDTO.Type)
                            , opt => opt.MapFrom(t => t.GetType().Name));

            CreateMap<ChoiceQuestion, GetQuestionItemDTO>().IncludeBase<Question, GetQuestionItemDTO>();
            CreateMap<OpenQuestion, GetQuestionItemDTO>().IncludeBase<Question, GetQuestionItemDTO>();


            CreateMap<Question,GetQuestionDTO>()
                .ForCtorParam(nameof(GetQuestionDTO.Type),
                             opt => opt.MapFrom(t => t.GetType().Name))
                .ForCtorParam(nameof(GetQuestionDTO.QuizId),
                                opt => opt.MapFrom(q => q.QuizId))
                .ForCtorParam(nameof(GetQuestionDTO.ClassId),
                                opt => opt.MapFrom(q => q.Quiz.ClassId))
                .ForCtorParam(nameof(GetQuestionDTO.QuizName),
                                opt => opt.MapFrom(q => q.Quiz.Name))
                .ForCtorParam(nameof(GetQuestionDTO.ClassName),
                                opt => opt.MapFrom(q => q.Quiz.Class.Name))

                .Include<ChoiceQuestion, GetChoiceQuestionDTO>()
                .Include<OpenQuestion, GetOpenQuestionDTO>();

            CreateMap<ChoiceQuestion, GetChoiceQuestionDTO>()
                .IncludeBase<Question, GetQuestionDTO>();

            CreateMap<OpenQuestion, GetOpenQuestionDTO>()
               .IncludeBase<Question, GetQuestionDTO>();



            CreateMap< PostChoiceQuestionDTO, ChoiceQuestion>();
            CreateMap<PostOpenQuestionDTO, OpenQuestion>();

        }
    }
}
