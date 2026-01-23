using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
            CreateMap<Question, GetQuestionItemDTO>();     

            CreateMap<ChoiceQuestion, GetQuestionItemDTO>().IncludeBase<Question, GetQuestionItemDTO>();
            CreateMap<OpenQuestion, GetQuestionItemDTO>().IncludeBase<Question, GetQuestionItemDTO>();


            CreateMap<Question,GetQuestionDTO>()
                .IncludeAllDerived();             

            CreateMap<ChoiceQuestion, GetChoiceQuestionDTO>()
                .ForCtorParam(nameof(GetQuestionDTO.ClassRoomId),
                                opt => opt.MapFrom(q => q.Quiz.ClassRoomId))
                .ForCtorParam(nameof(GetQuestionDTO.ClassRoomName),
                                opt => opt.MapFrom(q => q.Quiz.ClassRoom.Name))
                .IncludeBase<Question, GetQuestionDTO>();
                
            CreateMap<OpenQuestion, GetOpenQuestionDTO>()
                .ForCtorParam(nameof(GetQuestionDTO.ClassRoomId),
                                opt => opt.MapFrom(q => q.Quiz.ClassRoomId))
                .ForCtorParam(nameof(GetQuestionDTO.ClassRoomName),
                                opt => opt.MapFrom(q => q.Quiz.ClassRoom.Name))
                .IncludeBase<Question, GetQuestionDTO>();



            CreateMap<PostChoiceQuestionDTO, ChoiceQuestion>();     
            CreateMap<PostOpenQuestionDTO, OpenQuestion>();

            CreateMap<PutChoiceQuestionDTO, ChoiceQuestion>()
                .ForMember(opt=>opt.Options,o=>o.Ignore());
            CreateMap<PutOpenQuestionDTO, OpenQuestion>();



            CreateMap<ChoiceQuestion, GetChoiceQuestionInQuizDTO>();
            CreateMap<OpenQuestion, GetOpenQuestionInQuizDTO>();
        }
    }
}
