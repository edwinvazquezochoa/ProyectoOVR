using Microsoft.AspNetCore.Http;
using Ovr.Domain.Responses;
using System.Net;
using System.Text.Json;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Continúa con el siguiente middleware
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            Console.WriteLine(message);
            // Crear un objeto ResponseBase para la respuesta uniforme
            var response = new ResponseBase<object>
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = ex.Message,//"Ocurrió un error inesperado. Inténtalo más tarde.",
                Data = null
            };

            // Configurar la respuesta HTTP
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Serializar y enviar el objeto ResponseBase
            var responseJson = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseJson);
        }
    }
}
