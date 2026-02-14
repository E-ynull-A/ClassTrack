
using ClassTrack.MVC.Services.Interfaces;
using ClassTrack.MVC.ViewModels;
using System.Net;
using System.Net.Http.Headers;

namespace ClassTrack.MVC.Middlewares
{
    public class TokenHandlerMiddleware:DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ITokenClientService _tokenService;

        public TokenHandlerMiddleware(IHttpContextAccessor accessor,
                                       ITokenClientService tokenService)
                                       
        {
            _accessor = accessor;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            string? access = _accessor.HttpContext.Request.Cookies["AccessToken"];

            if (access is not null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access);
            }
  
           HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                string? rToken = _accessor.HttpContext.Request.Cookies["RefreshToken"];

                if (rToken is not null)
                {
                    ResponseTokenVM newToken = await _tokenService.GetTokensAsync(rToken);

                    if (newToken != null)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken.AccessToken.AccessToken);
                        return await base.SendAsync(request, cancellationToken);
                    }

                }
            }
            return response;
        }
    }
}


