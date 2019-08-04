using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GameStore.Services
{
    public class ManageHttpHeadersMiddleware
    {
        private RequestDelegate _next;

        public ManageHttpHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("X-HTML-Minification-Powered-By");
                return Task.FromResult(1);
            });
            await _next(context);
        }
    }
}
