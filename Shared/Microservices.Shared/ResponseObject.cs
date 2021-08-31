using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Microservices.Shared
{
    public class ResponseObject<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }



        public static ResponseObject<T> CreateSuccessResponse(T data, int statusCode = 200)
        {
            return new ResponseObject<T>(){Data = data, StatusCode = statusCode, IsSuccess = true};
        }


        public static ResponseObject<T> CreateSuccessResponse(int statusCode = 200)
        {
            return new ResponseObject<T>() { Data = default(T), StatusCode = statusCode, IsSuccess = true };
        }



        public static ResponseObject<T> CreateErrorResponse(int statusCode, params string[] errors)
        {
            return new ResponseObject<T>(){Errors = errors.ToList(), StatusCode = statusCode};

        }

    }
}
