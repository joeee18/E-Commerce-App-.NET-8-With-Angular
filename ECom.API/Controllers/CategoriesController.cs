using AutoMapper;
using ECom.API.Helper;
using ECom.Core.DTO;
using ECom.Core.Entites.Product;
using ECom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Get() 
        {
            try
            {
                var categories = await work.CategoryRepository.GetAllAsync();
                if (categories is null)
                    return BadRequest(new ResponseAPI(400));
                return Ok(categories);

            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
         
        }
        [HttpGet("get-by-id{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await work.CategoryRepository.GetByIdAsync(id);
                if (category is null)
                    return BadRequest( new ResponseAPI(400,$"Not Found Category Id={id}"));
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("add-category")]
        public async Task<IActionResult> Add(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                await work.CategoryRepository.AddAsync(category);
                return Ok(new ResponseAPI(200, "Item has Been Added"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("update-category")]
        public async Task<IActionResult> Update(UpdatedCategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                await work.CategoryRepository.UpdateAsync(category);
                return Ok(new ResponseAPI(200, "Item has Been Updated"));

            }
            catch (Exception ex)
            {

                return BadRequest(new { ex.Message });
            }
        }
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await work.CategoryRepository.DeleteAsync(id);
                return Ok(new ResponseAPI(200, "Item has Been Deleted"));

            }
            catch (Exception ex)
            {

                return BadRequest(new { ex.Message });
            }
        }
    }
}
