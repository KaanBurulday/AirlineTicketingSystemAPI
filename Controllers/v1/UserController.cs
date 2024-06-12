using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AirlineTicketingSystemAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IJwtTokenService _jwtTokenService;

        public UserController(IUserService service, ILogger<UserController> logger, IJwtTokenService jwtTokenService)
        {
            _userService = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                if (userLoginDto == null)
                {
                    return BadRequest("Invalid client request");
                }

                var user = await _userService.AuthenticateUser(userLoginDto.Email, userLoginDto.Password);
                if (user == null)
                {
                    return Unauthorized();
                }

                var token = _jwtTokenService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during login process");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GET")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var userDto = await _userService.GetUserAsync(id);

                if (userDto == null)
                {
                    return NotFound();
                }

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("POST")]
        public async Task<ActionResult<UserDto>> InsertUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                User userNew = await _userService.InsertUserAsync(userDto);
                return CreatedAtAction(nameof(InsertUser), new { id = userNew.Id }, userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("PUT/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingUser = await _userService.GetUserAsync(id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                await _userService.UpdateUserAsync(id, userDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var existingUser = await _userService.GetUserAsync(id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE")]
        public async Task<IActionResult> DeleteUsers()
        {
            try
            {
                await _userService.DeleteUsersAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting users");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
