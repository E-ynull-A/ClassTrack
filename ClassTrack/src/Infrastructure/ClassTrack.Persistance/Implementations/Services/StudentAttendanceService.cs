using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain;
using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.DAL;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class StudentAttendanceService:IStudentAttendanceService
    {
        private readonly IStudentAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public StudentAttendanceService(IStudentAttendanceRepository attendanceRepository,
                                        IMapper mapper,
                                        AppDbContext context)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
            _context = context;
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
            if(await _attendanceRepository.AnyAsync(a => attendanceDTOs.Select(aDto => aDto.LessonDate).Contains(a.LessonDate)))
            {
                throw new Exception("Invalid Operation Error");
            }

            if(attendanceDTOs.DistinctBy(aDto=>aDto.LessonDate).Count() != 1)
            {
                throw new Exception("There is an issue about Attendance Date");
            }

            if(attendanceDTOs.DistinctBy(aDto=>aDto.ClassRoomId).Count() != 1)
            {
                throw new Exception("All Student in this Attendance must are in the Same ClassRoom");
            }

            if(_context.StudentClasses.Select(sc=>sc.ClassRoomId == attendanceDTOs.Select(aDto=>aDto.ClassRoomId).First()).Count()
                                                                 != attendanceDTOs.DistinctBy(aDto => aDto.StudentId).Count())
            {
                throw new Exception("The attendance of All Students must be written And Don't any dublicate Student");
            }


           

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
