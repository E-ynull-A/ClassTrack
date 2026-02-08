using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ClassTrack.API.ActionFilter
{
    public class TeacherAccessFilter : IAsyncActionFilter
    {
        private readonly IClassRoomRepository _roomRepository;
        private readonly ICacheService _cacheService;

        public TeacherAccessFilter(IClassRoomRepository roomRepository,
                                    ICacheService cacheService)
        {
            _roomRepository = roomRepository;
            _cacheService = cacheService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            context.RouteData.Values.TryGetValue("classRoomId",out object roomId);
            long classRoomId = Convert.ToInt64(roomId);

            if(userId is not null && classRoomId > 0)
            {
                string cacheKey = $"Is_Teacher_{userId}_{classRoomId}";

                bool? resultCache = await _cacheService.GetAsync<bool?>(cacheKey);

                if (resultCache.HasValue)
                {
                    if (resultCache.Value)
                        await next();

                    context.Result = new ForbidResult();
                    return;
                }
                    

                bool result = await _roomRepository.AnyAsync(r => r.Id == classRoomId 
                && r.TeacherClasses.Any(tc => tc.Teacher.AppUserId == userId));

                await _cacheService
                       .SetCasheAsync(cacheKey, result, TimeSpan.FromMinutes(10));

                if (!result)
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
