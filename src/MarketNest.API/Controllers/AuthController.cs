using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketNest.Application.Common.DTOs;
using MarketNest.Application.Auth;
using MarketNest.Application.Common.Interfaces;
using MarketNest.Domain.Interfaces.Repositories;

namespace MarketNest.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _uow;

    public AuthController(IMediator mediator, IMapper mapper, ICurrentUserService currentUser, IUnitOfWork uow)
    {
        _mediator = mediator;
        _mapper = mapper;
        _currentUser = currentUser;
        _uow = uow;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Created(string.Empty, res);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Ok(res);
    }

    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody] GoogleLoginCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Ok(res);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand cmd)
    {
        var res = await _mediator.Send(cmd);
        return Ok(res);
    }

    [Authorize]
    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenCommand cmd)
    {
        await _mediator.Send(cmd);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = _currentUser.UserId;
        var user = await _uow.Users.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(_mapper.Map<UserDto>(user));
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserDto dto)
    {
        var id = _currentUser.UserId;
        var user = await _uow.Users.GetByIdAsync(id);
        if (user == null) return NotFound();
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Phone = dto != null ? dto.Phone : user.Phone;
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();
        return Ok(_mapper.Map<UserDto>(user));
    }

    [Authorize]
    [HttpPut("me/password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
    {
        var id = _currentUser.UserId;
        var user = await _uow.Users.GetByIdAsync(id);
        if (user == null) return NotFound();
        if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(req.OldPassword, user.PasswordHash))
            return BadRequest("Invalid current password");
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();
        return NoContent();
    }

    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
