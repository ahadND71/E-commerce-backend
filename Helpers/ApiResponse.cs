using api.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace api.Controllers
{
    public static class ApiResponse
    {
        public static IActionResult Success<T>(T data, string message = "Success", PaginationMeta? meta = null, string? token = null)
        {
            return CreateResponse<T>(HttpStatusCode.OK, true, message, data, token, meta);
        }

        public static IActionResult Created<T>(T data, string message = "Resource Created")
        {
            return CreateResponse<T>(HttpStatusCode.Created, true, message, data, null, null);
        }

        public static IActionResult NotFound(string message = "Resource Not Found")
        {
            return CreateResponse<object>(HttpStatusCode.NotFound, false, message, null, null, null);
        }

        public static IActionResult Conflict(string message = "Conflict Detected")
        {
            return CreateResponse<object>(HttpStatusCode.Conflict, false, message, null, null, null);
        }

        public static IActionResult BadRequest(string message = "Bad Request")
        {
            return CreateResponse<object>(HttpStatusCode.BadRequest, false, message, null, null, null);
        }

        public static IActionResult UnAuthorized(string message = "Unauthorized Access")
        {
            return CreateResponse<object>(HttpStatusCode.Unauthorized, false, message, null, null, null);
        }

        public static IActionResult Forbidden(string message = "Forbidden Access")
        {
            return CreateResponse<object>(HttpStatusCode.Forbidden, false, message, null, null, null);
        }

        public static IActionResult ServerError(string message = "Internal Server Error")
        {
            return CreateResponse<object>(HttpStatusCode.InternalServerError, false, message, null, null, null);
        }

        private static IActionResult CreateResponse<T>(HttpStatusCode statusCode, bool success, string message, T data, string token, PaginationMeta meta)
        {
            var result = new ObjectResult(new ApiResponseTemplate<T>((int)statusCode, success, message, data, token, meta));
            //without this line the status code will be shown as 200 then the body will show the error so i make sure the the status code and the body status code is consistent 
            result.StatusCode = (int)statusCode;
            return result;
        }
    }

    public class ApiResponseTemplate<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public string Token { get; set; }
        public PaginationMeta? Meta { get; set; }


        public ApiResponseTemplate(int statusCode, bool success, string message, T? data, string token, PaginationMeta? meta)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Data = data;
            Token = token;
            Meta = meta;

        }
    }
}
