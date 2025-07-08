using AutoMapper;
using ECom.Core.Interfaces;
using ECom.Core.Services;
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
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public UintOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context, _mapper, _imageManagementService);
            PhotoRepository = new PhotoRepository(context);
            
        }
    }
}
