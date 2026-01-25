using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Domain.Entities;

namespace ClassTrack.Application.MappingProfiles
{
    public class QuizProfile:Profile
    {
        public QuizProfile()
        {
            CreateMap<Quiz, GetQuizItemDTO>();
            CreateMap<Quiz,GetQuizDTO>();

            CreateMap<PostQuizDTO, Quiz>();
            CreateMap<PutQuizDTO, Quiz>()
                .ForMember(q=>q.ChoiceQuestions,opt=>opt.Ignore())
                .ForMember(q => q.OpenQuestions, opt => opt.Ignore());
        }
    }
}
