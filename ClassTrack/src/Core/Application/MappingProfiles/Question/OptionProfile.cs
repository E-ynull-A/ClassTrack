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
    internal class OptionProfile:Profile
    {
        public OptionProfile()
        {
            CreateMap<Option, GetOptionItemInQuestionDTO>();

            CreateMap<Option, GetOptionItemInChoiceQuestionDTO>();

            CreateMap<PostOptionInChoiceQuestionDTO, Option>();

            CreateMap<PutOptionInChoiceQuestionDTO, Option>();

            CreateMap<Option, GetOptionItemInQuizDTO>();
        
        }
    }
}
