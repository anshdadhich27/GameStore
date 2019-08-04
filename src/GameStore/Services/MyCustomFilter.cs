using GameStore.Models.Entity;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace GameStore.Services
{
    public class MyCustomFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode = 500;
            var ex = context.Exception;
            context.ExceptionHandled = true;
            string info = context.HttpContext.Request.Path.Value;
            Task.Run(async () => { await ExceptionGateway.AddExceptionAsync(new ExceptionLog(ex, info)); });
            context.HttpContext.Response.StatusCode = statusCode;
            context.HttpContext.Response.Redirect(string.Format("/error/{0}", statusCode));
        }
    }
}
