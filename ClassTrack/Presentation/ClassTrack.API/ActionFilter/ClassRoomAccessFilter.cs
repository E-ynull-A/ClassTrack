using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ClassTrack.API.ActionFilter
{
    public class ClassRoomAccessFilter : IAsyncActionFilter
    {
        private readonly IClassRoomRepository _roomRepository;

        public ClassRoomAccessFilter(IClassRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            string? userRole = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (userRole is null)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (userRole == UserRole.Admin.ToString())
            {
                await next();
                return;
            }

            context.RouteData.Values.TryGetValue("classRoomId", out object roomId);
            long classRoomId = Convert.ToInt64(roomId);

            if (!string.IsNullOrEmpty(userId) && classRoomId > 0)
            {
                bool isExists = await _roomRepository.AnyAsync(r => r.Id == classRoomId);

                if (!isExists)
                {
                    context.Result = new NotFoundObjectResult("The ClassRoom not Found!");
                    return;
                }


                bool hasAccess = await _roomRepository
                    .AnyAsync(r => r.Id == classRoomId 
                    && (r.StudentClasses.Any(sc => sc.Student.AppUserId == userId)
                    || r.TeacherClasses.Any(tc=>tc.Teacher.AppUserId == userId)));

                if (!hasAccess)
                {
                    context.Result = new ForbidResult();
                    return;
                }

            }
            else
            {
                context.Result = new BadRequestObjectResult("The Datas is not Valid or not Completed");
                return;
            }
            await next();
        }

       
    }
}
