using AccountManagement.Models.Requests;
using AccountManagement.Models.Responses;
using AccountManagement.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    
    private readonly IUserService _userService;
    private readonly IAccountService _accountService; 
    private readonly Serilog.ILogger _logger;
    
    public AccountController(IUserService userService, Serilog.ILogger logger, IAccountService accountService)
    {
        _userService = userService;
        _logger = logger;
        _accountService = accountService;
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateAccountRequest request, [FromHeader] string Authorization)
    {
        try
        {
            var accountNumber = await _accountService.CreateAccount(request);
            return SuccessResponse.CommonResponse(accountNumber);
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.InternalServerError(ex.Message);
        }
    }
    
    [HttpPut("Edit")]
    public async Task<IActionResult> Edit(EditAccountRequest request)
    {
        try
        {
            await _accountService.EditAccount(request);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.InternalServerError(ex.Message);
        }
    }
    
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _accountService.DeleteAccount(id);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.InternalServerError(ex.Message);
        }
    }
    
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var account = await _accountService.GetAccountById(id);
            return Ok(account);
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.InternalServerError(ex.Message);
        }
    }
    
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var accounts = await _accountService.GetAccounts();
            return Ok(accounts);
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.InternalServerError(ex.Message);
        }
    }
    
    [HttpPost("AddBalance")]
    public async Task<IActionResult> AddBalance(BalanceRequest request)
    {
        try
        {
            await _accountService.AddBalance(request);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.InternalServerError(ex.Message);
        }
    }
    
    [HttpPost("SubtractBalance")]
    public async Task<IActionResult> SubtractBalance(BalanceRequest request)
    {
        try
        {
            await _accountService.SubtractBalance(request);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.InternalServerError(ex.Message);
        }
    }

    [HttpPost("TransferBalance")]
    public async Task<IActionResult> TransferBalance(TransferBalanceRequest request)
    {
        try
        {
            await _accountService.TransferBalance(request);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.InternalServerError(ex.Message);
        }
    }
}

