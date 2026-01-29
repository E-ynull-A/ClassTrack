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
            CreateMap<PutQuizDTO, Quiz>();


        }
    }
}
