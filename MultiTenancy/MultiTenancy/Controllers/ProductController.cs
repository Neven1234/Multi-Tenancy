using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenancy.Dto;
using MultiTenancy.Models;
using MultiTenancy.Services;

namespace MultiTenancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _iproductServices;

        public ProductController(IProductServices productServices)
        {
            this._iproductServices = productServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var res=await _iproductServices.GetAllProducts();
            return Ok(res);
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetProduct(int Id)
        {
            var res= await _iproductServices.GetBuyId(Id);
            if(res==null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        [HttpPost]
        public async Task <IActionResult>AddProduct(CreatProductDto productDTO)
        {
            Product product = new()
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Rate = productDTO.Rate,
            };
            var res= await _iproductServices.AddProduct(product);
            return Ok(res);

      
        }
    }
}
