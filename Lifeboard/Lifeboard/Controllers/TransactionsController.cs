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

        [HttpGet(Name = "GetTransactions")]
        public async Task<IActionResult> Get()
        {
            var result = await _transactionsService.GetTransactions();
            return Ok(result);
        }
    }
}
