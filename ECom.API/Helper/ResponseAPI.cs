﻿namespace ECom.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(StatusCode);
        }
        private string GetMessageFromStatusCode(int statusCode) 
        {
            return statusCode switch
            {
                200 => "Done",
                400 => "Bad Request",
                401 => "Un Authorized",
                404 => "Resource Not Found",
                500 => "Server Error",
                _ => "Done",

            };
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
