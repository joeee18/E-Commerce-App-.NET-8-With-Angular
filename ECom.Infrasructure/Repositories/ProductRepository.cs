using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entites.Product;
using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Infrasructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrasructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public readonly IImageManagementService imageManagementService ;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return false;
            }
            else
            {
                var product = mapper.Map<Product>(productDTO);
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                var ImagePath = await imageManagementService.AddImageAsync(productDTO.photo, productDTO.Name);
                var photo = ImagePath.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = product.Id,
                }).ToList();
                await context.Photos.AddRangeAsync(photo);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO productDTO)
        {
            if (productDTO is null)
            {
                return false;
            }
            var FindProduct = await context.Products.Include(m => m.Category).Include(x => x.Photos).FirstOrDefaultAsync(m=>m.Id == productDTO.Id);
            if (FindProduct is null)
            {
                return false;
            }
            mapper.Map(productDTO,FindProduct);

            var FindPhoto = await context.Photos.Where(Photo => Photo.ProductId == productDTO.Id).ToListAsync();

            foreach (var item in FindPhoto)
            { 
                imageManagementService.DeleteImageAsync(item.ImageName);
            }

            context.Photos.RemoveRange(FindPhoto);

            var ImagePath = await imageManagementService.AddImageAsync( productDTO.photo, productDTO.Name);

            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = productDTO.Id
            }).ToList();

            await context.Photos.AddRangeAsync(photo);

            await context.SaveChangesAsync();
            return true;
        }
    }
}
