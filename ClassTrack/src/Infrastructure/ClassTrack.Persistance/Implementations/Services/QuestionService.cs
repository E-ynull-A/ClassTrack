using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ClassTrack.Domain.Utilities;

namespace ClassTrack.Persistance.Implementations.Services

{
    internal class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly IOptionRepository _optionRepository;

        public QuestionService(IQuestionRepository questionRepository
                                , IQuizRepository quizRepository
                                    , IMapper mapper,
                                IOptionRepository optionRepository)
        {
            _questionRepository = questionRepository;
            _quizRepository = quizRepository;
            _mapper = mapper;
            _optionRepository = optionRepository;
        }

        public async Task<ICollection<GetQuestionItemDTO>> GetAllAsync(int page, int take, params string[] includes)
        {

            ICollection<Question> questions = await _questionRepository.GetAll(page: page,
                                            take: take,
                                            sort: x => x.CreatedAt,
                                            includes: ["Options", "Quiz"]).ToListAsync();

            return _mapper.Map<ICollection<GetQuestionItemDTO>>(questions);
        }
        public async Task<GetQuestionDTO> GetByIdAsync(long id)
        {
            Question question = await _questionRepository
                                        .GetByIdAsync(id, includes: ["Quiz.ClassRoom", "Options"]);

            if (question is null)
                throw new Exception("Question Not Found");

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
        public async Task UpdateChoiceQuestionAsync(long id, PutChoiceQuestionDTO putChoice)
        {
            ChoiceQuestion oldQuestion = await _basePutCheckAsync<PutChoiceQuestionDTO, ChoiceQuestion>(id, putChoice);

            if (oldQuestion is ChoiceQuestion)
            {

                _findDifferencesOption(oldQuestion, putChoice);

                oldQuestion = _mapper.Map(putChoice, oldQuestion);

                oldQuestion.Options = oldQuestion.Options.TrimAll();

                _questionRepository.Update(oldQuestion);
            }
            else
            {
                throw new Exception("Bad Type Request!");
            }


            await _questionRepository.SaveChangeAsync();
        }
        public async Task UpdateOpenQuestionAsync(long id,PutOpenQuestionDTO putOpen)
        {
            OpenQuestion oldQuestion = await _basePutCheckAsync<PutOpenQuestionDTO, OpenQuestion>(id,putOpen);

            if(oldQuestion is OpenQuestion)
            {
                oldQuestion = _mapper.Map(putOpen, oldQuestion);

                _questionRepository.Update(oldQuestion);
                await _questionRepository.SaveChangeAsync();
            }
            else
            {
                throw new Exception("Bad Type Request!");
            }
        }
        public async Task DeleteChoiceQuestionAsync(long id)
        {
            try {
                ChoiceQuestion deletedChoice = (ChoiceQuestion)await _questionRepository
                                            .GetByIdAsync(id, includes: [nameof(ChoiceQuestion.Options)]);

                _optionRepository.DeleteRange(deletedChoice.Options);
                _questionRepository.Delete(deletedChoice);

                await _questionRepository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while deleting the Choice question.");
            }   
        }

        public async Task DeleteOpenQuestionAsync(long id)
        {
            try
            {
                OpenQuestion deletedOpen = (OpenQuestion)await _questionRepository.GetByIdAsync(id);

                _questionRepository.Delete(deletedOpen);
                await _questionRepository.SaveChangeAsync();
            }

            catch (Exception)
            {
                throw new Exception("An error occurred while deleting the Open question.");
            }
        }

     
        private void _findDifferencesOption(ChoiceQuestion oldQuest, PutChoiceQuestionDTO newQuestDto)
        {
            ICollection<Option> oldOptions = oldQuest.Options;
            ICollection<PutOptionInChoiceQuestionDTO>? newOptions = newQuestDto.Options;

            oldOptions.Where(o => !newOptions.Select(no => no.Variant)
                                                    .Contains(o.Variant))
                                                        .ToList()
                                                    .ForEach(dlo =>
            {
                _optionRepository.Delete(dlo);
                oldQuest.Options.Remove(dlo);
            });


            for (int i = 0; i < newOptions.Count; i++)
            {
                PutOptionInChoiceQuestionDTO pq = newOptions.ElementAt(i);

                Option? dublictate = oldOptions
                                        .FirstOrDefault(oq => oq.Variant == pq.Variant);

                if (dublictate is not null)
                {
                    dublictate.UpdatedAt = DateTime.UtcNow;
                    dublictate.IsCorrect = pq.IsCorrect;
                }

                else
                {
                    oldQuest.Options.Add(new Option
                    {
                        IsCorrect = pq.IsCorrect,
                        Variant = pq.Variant,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

        }
        private async Task<E> _basePutCheckAsync<T, E>(long id, T questionDTO) where T : IBasePutQuestion where E : Question, new()
        {
            E? oldQuestion = null;

            if (questionDTO is PutChoiceQuestionDTO)
            {
                oldQuestion = await _questionRepository.GetByIdAsync(id, includes: [nameof(ChoiceQuestion.Quiz), nameof(ChoiceQuestion.Options)]) as E;
            }
            else
            {
                oldQuestion = await _questionRepository.GetByIdAsync(id, includes: [nameof(Question.Quiz)]) as E;
            }

            if (oldQuestion == null) throw new Exception("The Question doesn't Found!");

            _quizRepository.GetAllowCreateOrUpdateQuestion(oldQuestion.Quiz);

            if (await _questionRepository.AnyAsync(q => q.Title.Trim() == questionDTO.Title.Trim() && q.Id != id))
                throw new Exception("The Same Question Title couldn't use again in the Same Quiz");

            return oldQuestion;
        }
        private async Task _basePostChecksAsync<T>(T questionDTO) where T : IBasePostQuestion
        {
            Quiz quiz = await _quizRepository.GetByIdAsync(questionDTO.QuizId);

            if (quiz is null)
                throw new Exception("The Quiz doesn't Found");

            _quizRepository.GetAllowCreateOrUpdateQuestion(quiz);

            if (await _questionRepository.AnyAsync(q => q.Title.Trim() == questionDTO.Title.Trim()))
                throw new Exception("The Same Question Title couldn't use again in the Same Quiz");
        }

    }
}
