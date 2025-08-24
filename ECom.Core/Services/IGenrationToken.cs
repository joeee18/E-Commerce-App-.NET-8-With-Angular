using ECom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.Services
{
    public interface IGenrationToken
    {
        string GetAndCreateToken(AppUser user);
    }
}
