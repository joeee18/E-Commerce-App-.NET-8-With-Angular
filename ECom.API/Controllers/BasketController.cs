using AutoMapper;
using ECom.API.Helper;
using ECom.Core.Entites;
using ECom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.API.Controllers
{

    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await work.CustomerBasket.GetBasketAsync(id);
            if (result is null) 
            { 
                return Ok(new CustomerBasket());
            }
            return Ok(result);

        }

        [HttpPost("update-basket")]
        public async Task<IActionResult> Add(CustomerBasket basket)
        {
            var _Basket = await work.CustomerBasket.UpdateBasketAsync(basket);
            return Ok(basket);
        }

        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await work.CustomerBasket.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200,"item deleted")): BadRequest(new ResponseAPI(400));
        }
    }
}
