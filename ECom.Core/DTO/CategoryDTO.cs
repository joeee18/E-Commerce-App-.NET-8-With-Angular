using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.DTO
{
    public record CategoryDTO
    (string Name,string Description);
    public record UpdatedCategoryDTO
    (string Name, string Description,int Id);
}
