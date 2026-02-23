using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain;
using ClassTrack.Domain.Entities;
using ClassTrack.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class StudentAttendanceService:IStudentAttendanceService
    {
        private readonly IStudentAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        private readonly IClassRoomRepository _roomRepository;

        public StudentAttendanceService(IStudentAttendanceRepository attendanceRepository,
                                        IMapper mapper,
                                        IClassRoomRepository roomRepository)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
            _roomRepository = roomRepository;
        }
        public async Task<ICollection<GetStudentAttendanceItemDTO>> GetAllAsync(int page,
                                                                                int take,
                                                                                long classRoomId)
        {
           return _mapper.Map<ICollection<GetStudentAttendanceItemDTO>>
                                        (await _attendanceRepository
                                                .GetAll(page: page, take: take,
                                                        function: x=>x.ClassRoomId == classRoomId,
                                                        includes: ["Student.AppUser"]).ToListAsync());
        }    
        public async Task CreateAttendanceAsync(ICollection<PostStudentAttendanceDTO> attendanceDTOs)
        {

            if(await _attendanceRepository.AnyAsync(a => attendanceDTOs.Select(aDto => aDto.LessonDate).Contains(a.LessonDate)))
            {
                throw new BadRequestException("There already have an Attendance in that Time");
            }

            if(attendanceDTOs.DistinctBy(aDto=>aDto.LessonDate).Count() != 1)
            {
                throw new BadRequestException("There is an issue about Attendance Date");
            }

            if(attendanceDTOs.DistinctBy(aDto=>aDto.ClassRoomId).Count() != 1)
            {
                throw new BadRequestException("All Student in this Attendance must are in the Same ClassRoom");
            }

            long roomId = attendanceDTOs.Select(a => a.ClassRoomId).First();
            int count = attendanceDTOs.DistinctBy(aDto => aDto.StudentId).Count();

            if (!await _roomRepository.AnyAsync(r => r.Id == roomId && r.StudentClasses.Count == count))
                throw new BadRequestException("The attendance of All Students must be written And Don't any dublicate Student!");
                   
            ICollection<StudentAttendance> attendances = _mapper.
                                            Map<ICollection<StudentAttendance>>(attendanceDTOs);        

            _attendanceRepository.AddRange(attendances);
            await _attendanceRepository.SaveChangeAsync();
        }
        public async Task UpdateAttendanceAsync(ICollection<PutStudentAttendanceDTO> attendanceDTOs)
        {

            ICollection<StudentAttendance> attendances =await _attendanceRepository
                .GetAll(function:x=>x.LessonDate == attendanceDTOs.First().LessonDate &&
                                    x.ClassRoomId == attendanceDTOs.First().ClassRoomId)
                                    .ToListAsync();

            if (attendances.Count == 0)
                throw new NotFoundException("The Attendance List isn't Found!");

            
            if(attendances.Count != attendanceDTOs.Where(a=>a.Id != 0)
                                    .DistinctBy(a => a.Id).Count())
            {
                throw new NotFoundException("One or more students isn't Found!");
            }

            foreach(PutStudentAttendanceDTO attendanceDTO in attendanceDTOs)
            {
                if (attendanceDTO.Id > 0)
                {
                    attendances.FirstOrDefault(ad=>ad.Id == attendanceDTO.Id).Attendance = (Attendance)attendanceDTO.Attendance;
                }
            }

            await _attendanceRepository.SaveChangeAsync();   
        }
        public async Task DeleteAttendanceAsync(long id)
        {        
            StudentAttendance attendance = await _attendanceRepository.GetByIdAsync(id);

            if (attendance is null)
                throw new NotFoundException("The Attendance isn't Found");

            _attendanceRepository.Delete(attendance);
            await _attendanceRepository.SaveChangeAsync();
        }
    }
}
