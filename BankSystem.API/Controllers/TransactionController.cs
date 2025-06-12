using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
{
    public class TransactionController : ControllerBase
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
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



    }
}
