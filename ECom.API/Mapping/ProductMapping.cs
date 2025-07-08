using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entites.Product;
namespace ECom.API.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product,ProductDTO>().ForMember( x=>x.CategoryName , op=>op.MapFrom(src=>src.Category.Name)).ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();

            CreateMap<AddProductDTO,Product>()
                .ForMember(x=>x.Photos,op=>op.Ignore())
                .ReverseMap();

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(x => x.Photos, op => op.Ignore())
                .ReverseMap();
        }


    }
}
