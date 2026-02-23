using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.MVC.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberClientService _memberClient;

        public MemberController(IMemberClientService memberClient)
        {
            _memberClient = memberClient;
        }

        public async Task<IActionResult> GetStudent(long classRoomId, long? quizId, long? taskWorkId)
        {
            if (classRoomId < 1 || quizId.GetValueOrDefault() < 0 || taskWorkId.GetValueOrDefault() < 0)
                return BadRequest();

            return View("StudentList",await _memberClient.GetSimpleStudentAsync(classRoomId));
        }
        public async Task<IActionResult> Members(long classRoomId)
        {
            if (classRoomId < 1)
                return BadRequest();

            return View(await _memberClient.GetMembersAsync(classRoomId));
        }
        public async Task<IActionResult> Delete(long classRoomId,long studentId)
        {
            if (classRoomId < 1 || studentId < 1)
                return BadRequest();

            ServiceResult result = await _memberClient.KickAsync(classRoomId, studentId);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);            
            }

            return RedirectToAction(nameof(Members), new { classRoomId });
        }
        public async Task<IActionResult> Put(long classRoomId, long studentId)
        {
            if (classRoomId < 1 || studentId < 1)
                return BadRequest();

            ServiceResult result = await _memberClient.PromoteAsync(classRoomId, studentId);

            if (!result.Ok)
            {
                ModelState.AddModelError(result.ErrorKey, result.ErrorMessage);
            }

            return RedirectToAction(nameof(Members), new { classRoomId });
        }
    }
}
