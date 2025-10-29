using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers.AccountsController.cs
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountDto dto)
        {
            var account = await _accountService.CreateAccountAsync(dto);
            return Ok(account);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> AccountNumberExist(string accountNumber)
        {
            var success = await _accountService.AccountNumberExistAsync(accountNumber);
            return success ? NoContent() : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountDto dto)
        {
            var success = await _accountService.UpdateAccountAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _accountService.DeleteAccountAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance(Guid id)
        {
            var balance = await _accountService.GetAccountBalanceAsync(id);
            return Ok(new { Balance = balance });
        }
    }
}

