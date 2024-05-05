using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace api.Controllers
{
    public static class ApiResponse
    {
        public static IActionResult Success<T>(T data, string message = "Success")
        {
            return CreateResponse<T>(HttpStatusCode.OK, true, data, message);
        }

        public static IActionResult Created<T>(T data, string message = "Resource Created")
        {
            return CreateResponse<T>(HttpStatusCode.Created, true, data, message);
        }

        public static IActionResult NotFound(string message = "Resource Not Found")
        {
            return CreateResponse<object>(HttpStatusCode.NotFound, false, null, message);
        }

        public static IActionResult Conflict(string message = "Conflict Detected")
        {
            return CreateResponse<object>(HttpStatusCode.Conflict, false, null, message);
        }

        public static IActionResult BadRequest(string message = "Bad Request")
        {
            return CreateResponse<object>(HttpStatusCode.BadRequest, false, null, message);
        }

        public static IActionResult UnAuthorized(string message = "Unauthorized Access")
        {
            return CreateResponse<object>(HttpStatusCode.Unauthorized, false, null, message);
        }

        public static IActionResult Forbidden(string message = "Forbidden Access")
        {
            return CreateResponse<object>(HttpStatusCode.Forbidden, false, null, message);
        }

        public static IActionResult ServerError(string message = "Internal Server Error")
        {
            return CreateResponse<object>(HttpStatusCode.InternalServerError, false, null, message);
        }

        private static IActionResult CreateResponse<T>(HttpStatusCode statusCode, bool success, T data, string message)
        {
            return new ObjectResult(new ApiResponseTemplate<T>(success, data, message, (int)statusCode));
        }
    }

    public class ApiResponseTemplate<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public ApiResponseTemplate(bool success, T? data, string message, int statusCode)
        {
            Success = success;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }
    }
}
