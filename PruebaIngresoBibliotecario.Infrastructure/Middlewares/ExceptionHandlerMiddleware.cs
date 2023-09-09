using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PruebaIngresoBibliotecario.Core.Exceptions;
using PruebaIngresoBibliotecario.Domain.Models;
using System.Net;

namespace PruebaIngresoBibliotecario.Infrastructure.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception e)
            {
                await HandlerException(context, e);
            }
        }
        private Task HandlerException(HttpContext context, Exception e)
        {
            string message = "Hubo un error interno, comunicarse con el administrador";
            int code = (int)HttpStatusCode.InternalServerError;
            if (e.GetType() == typeof(BadRequestBussinessException))
            {
                message = e.Message;
                code = (int)HttpStatusCode.BadRequest;
            }
            if (e.GetType() == typeof(NotFoundBusinessException))
            {
                message = e.Message;
                code = (int)HttpStatusCode.NotFound;
            }

            ResponseBase responseBase = new ResponseBase()
            {
                mensaje = message,
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(responseBase));
        }
    }
}