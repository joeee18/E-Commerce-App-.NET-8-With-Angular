﻿using ECom.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.API.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    public class errorsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult( new ResponseAPI(statusCode));
        }
    }
}
