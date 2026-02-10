using System.Net;
using System.Threading.Tasks;

namespace ClassTrack.MVC.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)                                              
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch(Exception e)
            {
                var encodeMessage = WebUtility.UrlEncode(e.Message);
                context.Response.Redirect($"/Error/index?messageError={encodeMessage}");
            }   
        }
    }
}
