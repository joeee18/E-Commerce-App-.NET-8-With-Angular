using ECom.Core.DTO;
using ECom.Core.Entites;
using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Core.Sharing;
using ECom.Infrasructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrasructure.Repositories
{
    public class AuthRepository:IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailServices emailServices;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenrationToken genrateToken;
        private readonly AppDbContext context;

        public AuthRepository(UserManager<AppUser> userManager, IEmailServices emailServices, SignInManager<AppUser> signInManager, IGenrationToken genrateToken, AppDbContext context)
        {
            this.userManager = userManager;
            this.emailServices = emailServices;
            this.signInManager = signInManager;
            this.genrateToken = genrateToken;
            this.context = context;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO) 
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "this UserName is already registered";
            }

            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "this email is already registered";
            }

            AppUser user = new()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                DisplayName = registerDTO.DisplayName
            };

            var  result = await userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded is not true) 
            {
                return result.Errors.ToList()[0].Description;
            }
            // send email
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await SendEmail(user.Email, token, "active", "ActiveEmail", "Please Active Your Email , Click on Button to Active");

            return "done";
        }
        public async Task SendEmail(string email, string Token, string component, string subject, string message)
        {
            var result = new EmailDTO(email,
                                     "youssefmagdy0904@gmail.com",
                                     subject,
                                     EmailStringBody.send(email, Token, component, message));
            await emailServices.SendEmail(result);
        }
        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return null;
            } 

            var findUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if (findUser == null)
            {
                return "User not found. Please check your email or password.";
            }
            if (!findUser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please Active Your Email , Click on Button to Active");
                return "Please Confirem your E-mail First , we have send activate to your E-mail";
            }
            var result = await signInManager.CheckPasswordSignInAsync(findUser, loginDTO.Password, true);
            if (result.Succeeded)
            {
                return genrateToken.GetAndCreateToken(findUser);
            }
            


            return "Please check your email or password , something went wrong";
        }


        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var finduser = await userManager.FindByEmailAsync(email);
            if (finduser is null)
                return false;

            var token = await userManager.GeneratePasswordResetTokenAsync(finduser);
            await SendEmail(finduser.Email, token, "Reset-Password", "Reset Password", " Click on Button to Reset Your Password ");
            return true;
        }
        public async Task<string> ResetPassword(resetPasswordDTO resetPassword)
        {
            var finduser = await userManager.FindByEmailAsync(resetPassword.Email);
            if (finduser is null)
            {
                return null;
            }
            var result = await userManager.ResetPasswordAsync(finduser, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded) 
            {
                return "done";
            }
            return result.Errors.ToList()[0].Description;
        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO activeAccount)
        {
            var finduser = await userManager.FindByEmailAsync(activeAccount.Email);
            if (finduser is null)
            {
                return false;
            }                
            var result = await userManager.ConfirmEmailAsync(finduser, activeAccount.Token);
            if (result.Succeeded)
            {
                return true;
            }
            var token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);
            await SendEmail(finduser.Email, token, "active", "ActiveEmail", "Please Active Your Email , Click on Button to Active");
            return false;

        }

        public async Task<bool> UpdateAddress(string email, Address address)
        {
            var finduser = await userManager.FindByEmailAsync(email);
            if (finduser == null)
            {
                return false;
            }
            var myAddress = await context.Addresses.FirstOrDefaultAsync(a => a.AppUserId == finduser.Id);
            if (myAddress is null)
            {
                address.AppUserId = finduser.Id;
                await context.Addresses.AddAsync(address);
            }
            else
            {
                // Update properties on the tracked entity
                myAddress.state = address.state;
                myAddress.Street = address.Street;
                myAddress.City = address.City;
                myAddress.FirstName = address.FirstName;
                myAddress.LastName = address.LastName;
                myAddress.ZipCode = address.ZipCode;
                // No need to call Update, EF is already tracking myAddress
            }
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Address> getUserAddress(string email)
        {
            var User = await userManager.FindByEmailAsync(email);
            var address = await context.Addresses.FirstOrDefaultAsync(m=>m.AppUserId==User.Id);
            return address;
        }
    }
}
