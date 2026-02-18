using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ClassTrack.Domain.Utilities;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ClassTrack.Persistance.Implementations.Services

{
    internal class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly IOptionRepository _optionRepository;

        public QuestionService(IQuestionRepository questionRepository
                                ,IQuizRepository quizRepository
                                    ,IMapper mapper,
                               IOptionRepository optionRepository)
        {
            _questionRepository = questionRepository;
            _quizRepository = quizRepository;
            _mapper = mapper;
            _optionRepository = optionRepository;
        }

        public async Task<ICollection<GetQuestionItemDTO>> GetAllAsync(long quizId,
                                                                       int page,
                                                                       int take,
                                                                       params string[] includes)
        {

            ICollection<Question> questions = await _questionRepository.GetAll(page: page,
                                                                               take: take,
                                                                               sort: x => x.CreatedAt,
                                                                               function:x=>x.QuizId == quizId)
                                                                               .ToListAsync();

            return _mapper.Map<ICollection<GetQuestionItemDTO>>(questions);
        }
        public async Task<GetChoiceQuestionForUpdateDTO> GetChoiceForUpdateAsync(long id)
        {
            ChoiceQuestion? choiceQuestion = (ChoiceQuestion)await _questionRepository
                                .GetByIdAsync(id, includes: [nameof(ChoiceQuestion.Options)]);

            if (choiceQuestion is null)
                throw new NotFoundException("The Question not Found!");

            return _mapper.Map<GetChoiceQuestionForUpdateDTO>(choiceQuestion);                       
        }
        public async Task<GetOpenQuestionForUpdateDTO>  GetOpenForUpdateAsync(long id)
        {
            OpenQuestion? openQuestion = (OpenQuestion)await _questionRepository.GetByIdAsync(id);

            if (openQuestion is null)
                throw new NotFoundException("The Question not Found!");

            return _mapper.Map<GetOpenQuestionForUpdateDTO>(openQuestion);                      
        }
        public async Task<GetQuestionDTO> GetByIdAsync(long id)
        {
            Question question = await _questionRepository
                                        .GetByIdAsync(id, includes: ["Quiz.ClassRoom", "Options"]);

            if (question is null)
                throw new NotFoundException("Question Not Found");

            GetQuestionDTO questionDTO = _mapper.Map<GetQuestionDTO>(question);

            return questionDTO;
        }
        public async Task CreateChoiceQuestionAsync(PostChoiceQuestionDTO postChoice)
        {
            await _basePostChecksAsync(postChoice);

            ChoiceQuestion question = _mapper.Map<ChoiceQuestion>(postChoice);

            question.Options = question.Options.TrimAll();
            _questionRepository.Add(question);

            await _questionRepository.SaveChangeAsync();
        }
        public async Task CreateOpenQuestionAsync(PostOpenQuestionDTO postOpen)
        {
            await _basePostChecksAsync(postOpen);

            _questionRepository.Add(_mapper.Map<OpenQuestion>(postOpen));
            await _questionRepository.SaveChangeAsync();
        }
        public async Task UpdateChoiceQuestionAsync(long id, PutChoiceQuestionDTO putChoice, long quizId)
        {
            ChoiceQuestion oldQuestion = await _basePutCheckAsync<PutChoiceQuestionDTO, ChoiceQuestion>(id, putChoice,quizId);

            if (oldQuestion is ChoiceQuestion)
            {

                await _findDifferencesOption(oldQuestion, putChoice);

                oldQuestion = _mapper.Map(putChoice, oldQuestion);

                oldQuestion.Options = oldQuestion.Options.TrimAll();

                _questionRepository.Update(oldQuestion);
            }
            else
            {
                throw new BadRequestException("Bad Type Request!");
            }


            await _questionRepository.SaveChangeAsync();
        }
        public async Task UpdateOpenQuestionAsync(long id,PutOpenQuestionDTO putOpen,long quizId)
        {
            OpenQuestion oldQuestion = await _basePutCheckAsync<PutOpenQuestionDTO, OpenQuestion>(id,putOpen,quizId);           

            if(oldQuestion is OpenQuestion)
            {
                oldQuestion = _mapper.Map(putOpen, oldQuestion);

                _questionRepository.Update(oldQuestion);
                await _questionRepository.SaveChangeAsync();
            }
            else
            {
                throw new BadRequestException("Bad Type Request!");
            }
        }
        public async Task DeleteChoiceQuestionAsync(long id)
        {
            try {
                ChoiceQuestion deletedChoice = (ChoiceQuestion)await _questionRepository
                                            .GetByIdAsync(id, includes: [nameof(ChoiceQuestion.Options)]);

                _questionRepository.Delete(deletedChoice);

                await _questionRepository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw new BadRequestException("An error occurred while deleting the Choice question.");
            }   
        }
        public async Task DeleteOpenQuestionAsync(long id)
        {
            try
            {
                OpenQuestion deletedOpen = (OpenQuestion)await _questionRepository.GetByIdAsync(id, includes: ["Quiz"], isIgnore: true);

                if (deletedOpen is null)
                    throw new NotFoundException("The Question isn't Found!");

                deletedOpen.IsDeleted = true;

                _questionRepository.Update(deletedOpen);
                await _questionRepository.SaveChangeAsync();
            }

            catch (Exception)
            {
                throw new BadRequestException("An error occurred while deleting the Open question.");
            }
        }

     
        private async Task _findDifferencesOption(ChoiceQuestion oldQuest, PutChoiceQuestionDTO newQuestDto)
        {
            ICollection<Option> oldOptions = oldQuest.Options;
            ICollection<PutOptionInChoiceQuestionDTO>? newOptions = newQuestDto.Options
                                                            .Where(o=>!o.IsDeleted)
                                                            .ToList();

            oldOptions.Where(o => !newOptions.Where(no => no.Id!=null)
                                                    .Select(no=>no.Id!.Value)
                                                    .Contains(o.Id))
                                                        .ToList()
                                                    .ForEach(dlo =>
            {
                _optionRepository.Delete(dlo);
                oldQuest.Options.Remove(dlo);
            });


            for (int i = 0; i < newOptions.Count; i++)
            {
                PutOptionInChoiceQuestionDTO pq = newOptions.ElementAt(i);

                if (pq.Id.HasValue)
                {                  
                        if (!await _optionRepository.AnyAsync(o => o.Id == pq.Id))
                            throw new NotFoundException("The Option not Found!");                                 
                }
                
                Option? dublictate = oldOptions
                                        .FirstOrDefault(oq => oq.Id == pq.Id);

                if (dublictate is not null)
                {
                    dublictate.UpdatedAt = DateTime.UtcNow;
                    dublictate.Variant = pq.Variant;
                    dublictate.IsCorrect = pq.IsCorrect;
                }

                else
                {
                    oldQuest.Options.Add(new Option
                    {
                        IsCorrect = pq.IsCorrect,
                        Variant = pq.Variant,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

        }
        private async Task<E> _basePutCheckAsync<T, E>(long id, T questionDTO,long quizId) where T : IBasePutQuestion where E : Question, new()
        {       
            E? oldQuestion = null;

            if (!await _quizRepository.AnyAsync(q =>q.Id == quizId && q.ChoiceQuestions.Any(cq => cq.Id == id)
                                               || q.OpenQuestions.Any(oq => oq.Id == id)))
            {
                throw new NotFoundException("The Question not found in this Quiz");
            }

            if (questionDTO is PutChoiceQuestionDTO)            
                oldQuestion = await _questionRepository.GetByIdAsync(id, includes: [nameof(ChoiceQuestion.Quiz),
                                                                                   nameof(ChoiceQuestion.Options)]) as E;
            
            else oldQuestion = await _questionRepository.GetByIdAsync(id, includes: [nameof(Question.Quiz)]) as E;
            
            if (oldQuestion == null) throw new NotFoundException("The Question not Found!");

            if (await _questionRepository.AnyAsync(q => q.Title.Trim() == questionDTO.Title.Trim() && q.Id != id))
                throw new ConflictException("The Same Question Title couldn't use again in the Same Quiz");

            return oldQuestion;
        }
        private async Task _basePostChecksAsync<T>(T questionDTO) where T : IBasePostQuestion
        {
            Quiz quiz = await _quizRepository.GetByIdAsync(questionDTO.QuizId.Value);

            if(quiz is null) 
                throw new NotFoundException("The Quiz isn't Found!");

            if (await _questionRepository.AnyAsync(q => q.Title.Trim() == questionDTO.Title.Trim()))
                throw new ConflictException("The Same Question Title couldn't use again in the Same Quiz");
        }

    }
}
