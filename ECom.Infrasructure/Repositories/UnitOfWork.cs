using ECom.Core.Interfaces;
using ECom.Infrasructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrasructure.Repositories
{
    public class UintOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public UintOfWork(AppDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context);
            PhotoRepository = new PhotoRepository(context);
        }
    }
}
