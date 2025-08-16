using Lifeboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lifeboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _categoriesService;

        public CategoriesController(CategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _categoriesService.GetCategories();
            return Ok(result);
        }
    }
}
