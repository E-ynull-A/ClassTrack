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

            CreateMap<PostQuizDTO, Quiz>()
                .ForMember(nameof(Quiz.Duration), opt => opt.MapFrom(q => TimeSpan.FromMinutes(q.Duration)))
                .ForMember(nameof(Quiz.FullPoint), opt => opt.MapFrom(q =>
                (q.ChoiceQuestions != null ? q.ChoiceQuestions.Sum(c => c.Point) : 0) +
                (q.OpenQuestions != null ? q.OpenQuestions.Sum(c => c.Point) : 0)));


            CreateMap<PutQuizDTO, Quiz>()
                .ForMember(nameof(Quiz.Duration), opt => opt.MapFrom(q => TimeSpan.FromMinutes(q.Duration)));


        }
    }
}
