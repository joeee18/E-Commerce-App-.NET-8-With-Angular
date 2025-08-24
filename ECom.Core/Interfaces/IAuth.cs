using ECom.Core.DTO;
using ECom.Core.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO loginDTO);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPassword(resetPasswordDTO resetPassword);
        Task<bool> ActiveAccount(ActiveAccountDTO activeAccount);
        Task<bool> UpdateAddress(string email, Address address);
        Task<Address> getUserAddress(string email);

    }
}
