using Lifeboard.Models;
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
        public async Task<IActionResult> Get([FromQuery] string? user)
        {
            var result = string.IsNullOrEmpty(user)
                ? await _transactionsService.GetTransactions()
                : await _transactionsService.GetTransactionsForUser(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] string? user, [FromBody] AddTransactionDto transaction)
        {
            if(string.IsNullOrEmpty(user))
            {
                await _transactionsService.AddTransaction(transaction);
            } else
            {
                await _transactionsService.AddTransactionForUser(user, transaction);
            }
            return Created();
        }

        [HttpGet]
        [Route("total-courant")]
        public async Task<IActionResult> GetTotalCourant([FromQuery] string? user)
        {
            var result = string.IsNullOrEmpty(user)
                ? await _transactionsService.GetTotalCourant()
                : await _transactionsService.GetTotalCourantForUser(user);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionsService.DeleteTransaction(id);
            return Ok();
        }
    }
}
