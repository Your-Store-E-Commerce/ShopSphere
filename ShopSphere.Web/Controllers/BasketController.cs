using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Web.Models.Basket;

namespace ShopSphere.Web.Controllers
{
    public class BasketController : Controller
    {
        //private readonly IBasketRepository _basketRepo;
        //private readonly IMapper _mapper;
        //public BasketController(IBasketRepository basketRepo, IMapper mapper)
        //{
        //    _basketRepo = basketRepo;
        //    _mapper = mapper;
        //}
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost("{basketId}")]
        //public async Task<IActionResult> GetBasket(string basketId)
        //{
        //    var basket = await _basketRepo.GetBasketAsync(basketId);

        //    return View (basket ?? new CustomerBasket(basketId));
        //}


        //[HttpPost]
        //public async Task<IActionResult> UpdateorCreateBasket(CustomerBasket customerBasket)
        //{
           
        //    var creatbasket = await _basketRepo.UpdateBasketAsync(customerBasket);

        //    if (creatbasket is null)
        //        return BadRequest();

        //    return View (creatbasket);
        //}

        //[HttpDelete]
        //public async Task DeleteAsync(string basketId)
        //{
        //    await _basketRepo.DeleteBasketAsync(basketId);
        //}


    }
}
