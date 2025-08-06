using Lifeboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lifeboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsService _transactionsService;

        public TransactionsController(TransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _transactionsService.GetTransactions();
            return Ok(result);
        }

        [HttpGet]
        [Route("total-courant")]
        public async Task<IActionResult> GetTotalCourant()
        {
            var result = await _transactionsService.GetTotalCourant();
            return Ok(result);
        }
    }
}
