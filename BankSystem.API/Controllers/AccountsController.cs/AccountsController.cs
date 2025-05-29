using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
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
    }
}

