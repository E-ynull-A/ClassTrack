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
                if (e.Message.Contains("401"))
                    context.Response.Redirect("/Home/Login");
                context.Response.Redirect($"/Error/index?messageError={e.Message}");
            }   
        }
    }
}
