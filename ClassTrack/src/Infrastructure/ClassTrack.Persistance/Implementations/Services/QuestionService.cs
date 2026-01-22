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
                                            isIgnore: true,
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
            ChoiceQuestion oldQuestion = await _basePutCheckAsync<PutChoiceQuestionDTO,ChoiceQuestion>(id, putChoice);

            if (oldQuestion is ChoiceQuestion)
            {

                ChoiceQuestion choiceQuestion = _mapper.Map(putChoice,oldQuestion);

                //choiceQuestion.Options = _mapper.Map(putChoice.Options,oldQuestion.Options);

                choiceQuestion.Options = choiceQuestion.Options.TrimAll();

                _questionRepository.Update(choiceQuestion);
            }
            else
            {
                throw new Exception("Bad Type Request!");
            }


            await _questionRepository.SaveChangeAsync();
        }

        private async Task<E> _basePutCheckAsync<T,E>(long id, T questionDTO) where T : IBasePutQuestion where E : Question, new() 
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
