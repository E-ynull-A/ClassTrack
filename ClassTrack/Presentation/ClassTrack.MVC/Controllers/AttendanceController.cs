using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IMemberClientService _memberClient;
        private readonly IStudentAttendanceClientService _studentClient;

        public AttendanceController(IMemberClientService memberClient,
                                    IStudentAttendanceClientService studentClient)
        {
            _memberClient = memberClient;
            _studentClient = studentClient;
        }

        [HttpGet("Attendance/{classRoomId}")]
        public async Task<IActionResult> Attendance(long classRoomId)
        {
           
            return View(new StudentAttendanceVM(
                             (await _memberClient.GetStudentListAsync(classRoomId)).ToList()));
        }

        [HttpPost("Attendance/{classRoomId}")]
        public async Task<IActionResult> Create(ICollection<PostStudentAttendanceVM> attendanceVMs,long classRoomId)
        {
            if (!ModelState.IsValid)
            {
                return View("Attendance", new StudentAttendanceVM(
                             (await _memberClient.GetStudentListAsync(classRoomId)).ToImmutableList(),attendanceVMs.ToList()));
            }

            ServiceResult result = await _studentClient.CreateAttendanceAsync(attendanceVMs.ToList());

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey,result.ErrorMessage);
                return View("Attendance", new StudentAttendanceVM(
                             (await _memberClient.GetStudentListAsync(classRoomId)).ToImmutableList(), attendanceVMs.ToList()));
            }

            return RedirectToAction("Attendance",new {classRoomId });
        }


    }
}
