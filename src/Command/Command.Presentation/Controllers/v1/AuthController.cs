using Asp.Versioning;
using Command.Application.Command;
using Command.Application.Commands.Auth;
using Command.Application.DTOs.Auth.InputDTOs;
using Command.Presentation.Abstractions;
using Contract.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/auth")]
    public class AuthController : ApiController
    {
        private readonly IMediator mediator;
        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("login")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("refresh-token")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("logout")]
        [MapToApiVersion(1)]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO? logoutRequest)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Substring(7);
            var request = new LogoutCommand
            {
                AccessToken = accessToken,
                RefreshToken = logoutRequest?.RefreshToken ?? string.Empty
            };
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("register")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand? request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [HttpPost("verify-email")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand? request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Error);
        }

        [HttpPost("fogotpw")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> FogotPassword([FromBody] FogotPasswordCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Error);
        }

        [HttpPost("fogotpw-send-verify-email")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> FogotPasswordSendVerifyEmail([FromBody] FogotPasswordSendVerifyEmailCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Error);
        }

        [HttpPost("google")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginGoogleCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Error);
        }

        [HttpPost("change-password")]
        [MapToApiVersion(1)]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        
        [MapToApiVersion(1)]
        [HttpPost("force-change-password")]
        [Authorize(Roles =nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> ForceChangePassword([FromBody] ForceChangePasswordCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
