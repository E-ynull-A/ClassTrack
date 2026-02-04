
using System.Net.Http.Headers;

namespace ClassTrack.MVC.Middlewares
{
    public class TokenHandlerMiddleware:DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public TokenHandlerMiddleware(IHttpContextAccessor accessor)
                                       
        {
            _accessor = accessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            string? access = _accessor.HttpContext.Request.Cookies["AccessToken"];

            if (access is not null)
            {
                request.Headers.Authorization = null;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access);
            }
   
            return base.SendAsync(request, cancellationToken);
        }
    }
}
