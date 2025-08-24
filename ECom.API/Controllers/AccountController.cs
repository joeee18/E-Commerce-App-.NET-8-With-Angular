using AutoMapper;
using ECom.API.Helper;
using ECom.Core.DTO;
using ECom.Core.Entites;
using ECom.Core.Interfaces;
using ECom.Infrasructure.Data.Migrations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECom.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class AccountController : BaseController
    {
        //private readonly IUnitOfWork _unit;
        //private readonly IMapper _mapper;
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
            //_unit = work;
            //_mapper = mapper;
        }

        [Authorize]
        [HttpPut("update-address")]
        public async Task<IActionResult> updateAddress( ShipAddressDTO shipAddressDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var address = mapper.Map<Address>(shipAddressDTO);
            var result = await work.Auth.UpdateAddress(email, address);
            return result ? Ok() : BadRequest();
        }


        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterDTO registerDTO)
        {
            var result = await work.Auth.RegisterAsync(registerDTO);
            if (result != "done")
            {
                return BadRequest(new ResponseAPI(400, result));
            }
            else
            {
                return Ok(new ResponseAPI(200, result));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDTO loginDTO)
        {
            var result = await work.Auth.LoginAsync(loginDTO);
            if (result.StartsWith("please"))
            {
                return BadRequest(new ResponseAPI(400, result));
            }
            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
            });
            return Ok(new ResponseAPI(200, "Login successfully"));

        }
        [HttpPost("active-account")]
        public async Task<IActionResult> active(ActiveAccountDTO accountDTO)
        {
            var result = await work.Auth.ActiveAccount(accountDTO);
            return result ? Ok(new ResponseAPI(200, "Active Successfully")) : BadRequest(new ResponseAPI(400));
        }
        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await work.Auth.SendEmailForForgetPassword(email);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> reset(resetPasswordDTO passwordDTO)
        {
           
            var result = await work.Auth.ResetPassword(passwordDTO);
            if (result == "done")
            {
                return Ok(new ResponseAPI(200));
            }
            else
            {
                return BadRequest(new ResponseAPI(400));
            }
        }
        [HttpGet("isUserAuth")]
        public async Task<IActionResult> isUserAuth()
        {

            return User.Identity.IsAuthenticated ? Ok():BadRequest();
        }

        [HttpGet("get-address-for-user")]
        [Authorize]
        public async Task<IActionResult> getAddress()
        {
            var address = await work.Auth.getUserAddress(User.FindFirst(ClaimTypes.Email).Value);   
            var result = mapper.Map<ShipAddressDTO>(address);
            return Ok(result);       

        }

    }
}
