using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Utilities;





namespace ClassTrack.Persistance.Implementations.Services
{
    internal class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository,
                              IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetTeacherItemDTO>> GetAllAsync(long classRoomId, int page, int take)
        {
            if (!await _teacherRepository.AnyAsync(r => r.TeacherClassRooms.Any(tc=>tc.ClassRoomId==classRoomId)))
                    throw new NotFoundException("Class Room not Found!");

            return _mapper.Map<ICollection<GetTeacherItemDTO>>(_teacherRepository.GetAll(page: page,
                                                                                         take: take,
                                                                                         includes: [nameof(Teacher.AppUser)],
                                                                                         function:x=>x.TeacherClassRooms.Select(tc=>tc.ClassRoomId).Contains(classRoomId),
                                                                                         sort: x => x.AppUser.Name));
        }
    }
}
