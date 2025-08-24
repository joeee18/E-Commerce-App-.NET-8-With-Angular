using AutoMapper;
using ECom.API.Helper;
using ECom.Core.DTO;
using ECom.Core.Entites.Product;
using ECom.Core.Interfaces;
using ECom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.API.Controllers
{

    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all-products")]
        public async Task<IActionResult> Get([FromQuery] ProductParams productParams)
        {
            try
            {
                var products = await work.ProductRepository.GetAllAsync(productParams);
                // var TotalCount = await work.ProductRepository.CountAsync();

                return Ok(new Pagination<ProductDTO>(productParams.PageNumber, productParams.PageSize, products.TotalCount, products.Products));
            }
            catch (Exception ex)
            {

                return BadRequest(new { ex.Message });
            }
        }
        [HttpGet("get-by-Id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var products = await work.ProductRepository.GetByIdAsync(id ,x => x.Photos, x => x.Category);
                var Result = mapper.Map<ProductDTO>(products);
            try
            {
                
                if (products is null)
                    return Ok(new ResponseAPI(400, "No Products"));
                return Ok(Result);
            }
            catch (Exception ex)
            {

                return BadRequest(new { ex.Message });
            }
        }
        [HttpPost("Add-Product")]
        public async Task<IActionResult> Add(AddProductDTO productDTO) 
        {
            try
            {
                await work.ProductRepository.AddAsync(productDTO);
                return Ok(new ResponseAPI(200));

            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI (400, ex.Message ));
            }
        }
        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDTO productDTO)
        {
            try
            {
                await work.ProductRepository.UpdateAsync(productDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await work.ProductRepository
                    .GetByIdAsync(id, x => x.Photos, x => x.Category);
                await work.ProductRepository.DeleteAsync(product);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI( 400, ex.Message ));
            }
        }
    }
}
