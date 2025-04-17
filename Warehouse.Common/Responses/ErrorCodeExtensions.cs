using Microsoft.AspNetCore.Http;

namespace Warehouse.Common.Responses
{
    public static class ErrorCodeExtensions
    {
        public static int GetHttpStatusCode(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFound => StatusCodes.Status404NotFound,
                ErrorCode.InvalidInput => StatusCodes.Status400BadRequest,
                ErrorCode.ValidationError => StatusCodes.Status400BadRequest,
                ErrorCode.ServiceUnavailable => StatusCodes.Status503ServiceUnavailable,
                ErrorCode.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorCode.Conflict => StatusCodes.Status409Conflict,
                ErrorCode.BadRequest => StatusCodes.Status400BadRequest,
                ErrorCode.InternalError => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };
        }
    }
}