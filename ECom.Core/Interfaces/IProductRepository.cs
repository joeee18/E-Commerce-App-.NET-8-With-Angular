using ECom.Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECom.Core.DTO;

namespace ECom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> AddAsync(AddProductDTO productDTO);
        Task<bool> UpdateAsync(UpdateProductDTO productDTO);
        Task DeleteAsync(Product product);

    }
}
