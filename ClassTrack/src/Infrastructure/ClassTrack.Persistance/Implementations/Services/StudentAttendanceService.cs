using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain;
using ClassTrack.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class StudentAttendanceService:IStudentAttendanceService
    {
        private readonly IStudentAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;

        public StudentAttendanceService(IStudentAttendanceRepository attendanceRepository,
                                        IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
        }


        public async Task<ICollection<GetStudentAttendanceItemDTO>> GetAllAsync(int page,int take)
        {
           return _mapper.Map<ICollection<GetStudentAttendanceItemDTO>>
                                        (await _attendanceRepository
                                                .GetAll(page: page, take: take,
                                                        includes: ["Student.AppUser"]).ToListAsync());
        }

       
        public async Task CreateAttendanceAsync(ICollection<PostStudentAttendanceDTO> attendanceDTOs)
        {

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

            
            if(attendances.Count != attendanceDTOs.Where(a=>a.Id != 0)
                                    .DistinctBy(a => a.Id).Count())
            {
                throw new Exception("One or more students isn't Found!");
            }

            foreach(PutStudentAttendanceDTO attendanceDTO in attendanceDTOs)
            {
                if (attendanceDTO.Id > 0)
                {
                    attendances.FirstOrDefault(ad=>ad.Id == attendanceDTO.Id).Attendance = (Attendance)attendanceDTO.Attendance;
                }

                else
                {
                    _attendanceRepository.Add( new StudentAttendance
                    {
                        StudentId = attendanceDTO.StudentId,
                        LessonDate = attendanceDTO.LessonDate,
                        ClassRoomId = attendanceDTO.ClassRoomId,
                        Attendance = (Attendance)attendanceDTO.Attendance,       
                    });                    
                }

                await _attendanceRepository.SaveChangeAsync();
            }
        }

        public async Task DeleteAttendanceAsync(long id)
        {
          StudentAttendance attendance = await _attendanceRepository.GetByIdAsync(id);

            if (attendance is null)
                throw new Exception("The Attendance isn't Found");

            _attendanceRepository.Delete(attendance);
            await _attendanceRepository.SaveChangeAsync();
        }
    }
}
