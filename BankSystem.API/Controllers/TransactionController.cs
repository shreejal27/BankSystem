using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
       
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
        {
            await _transactionService.DepositAsync(dto);
            return Ok("Deposit successful.");
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccount(Guid accountId)
        {
            var result = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
            return Ok(result);
        }
    }
}
