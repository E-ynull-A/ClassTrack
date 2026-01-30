using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain;
using ClassTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.MappingProfiles
{
    public class QuizAnswerProfile:Profile
    {
        public QuizAnswerProfile()
        {
            CreateMap<QuizAnswer, GetQuizAnswerItemDTO>()
                .ForCtorParam(nameof(GetQuizAnswerItemDTO.QuestionTitle), 
                                      opt => opt.MapFrom(qa => qa.Question.Title));

            CreateMap<QuizAnswer, GetQuizAnswerDTO>();

            CreateMap<PostQuizSubmitDTO, QuizAnswer>();
             
        }
    }
}
