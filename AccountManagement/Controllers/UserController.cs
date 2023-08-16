using AccountManagement.Models.Requests;
using AccountManagement.Models.Responses;
using AccountManagement.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class UserController: ControllerBase
{
    
    private readonly IUserService _userService;
    private readonly Serilog.ILogger _logger;
    private readonly IAuthenticationManager _authenticationManager;

    public UserController(IUserService userService, Serilog.ILogger logger, IAuthenticationManager authenticationManager)
    {
        _userService = userService;
        _logger = logger;
        _authenticationManager = authenticationManager;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {

        try
        {
            await _userService.Login(model.Username, model.Pin);
            var jwt = await _authenticationManager.AuthenticateWithRefreshAsync(model.Username, model.Pin);
            return Ok(jwt);
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.BadRequest(ex.Message);
        }

    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        try
        {
            var jwt = await _authenticationManager.RefreshAccessToken(request.RefreshToken);
            if (jwt == null)
            {
                throw new Exception("Invalid refresh token");
            }
            return Ok(jwt);
        }
        catch (ArgumentException ex)
        {
            return ErrorResponse.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return ErrorResponse.BadRequest(ex.Message);
        }
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(LoginModel model)
    {
        try
        {
            await _userService.CreateUser(model.Username, model.Pin);
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
    
    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateModel model)
    {
        try
        {
            await _userService.UpdateUser(model.Id, model.Username, model.Pin);
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