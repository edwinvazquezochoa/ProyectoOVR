using Microsoft.AspNetCore.Mvc;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.Domain.Responses;
using System.Net;

namespace Ovr.ApiServices.Helpers
{
    public static class ApiResponseHelper
    {
        public static IActionResult Success<T>(T data, string message = "Operación exitosa")
            where T : class, new()
        {
            return new OkObjectResult(new ResponseBase<T>
            {
                Code = (int)HttpStatusCode.OK,
                Message = message,
                Data = data
            });
        }

        public static IActionResult Empty<T>(string message = "No se encontró información")
            where T : class, new()
        {
            return new OkObjectResult(new ResponseBase<T>
            {
                Code = (int)HttpStatusCode.OK,
                Message = message,
                Data = new T()
            });
        }

        public static IActionResult Error(IEventLogger logger, Exception ex, string context)
        {
            logger.LogException(ex, context);

            return new ObjectResult(new ResponseBase<object>
            {
                Code = 500,
                Message = "Ocurrió un error inesperado.",
                Data = null
            })
            {
                StatusCode = 500
            };
        }
    }
}
