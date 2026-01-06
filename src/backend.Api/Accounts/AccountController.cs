using backend.Core.Accounts;
using backend.Core.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Accounts
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(IAccountCrud accountCrud) : ControllerBase
    {
        [Authorize(Roles = Role.Admin)]
        [HttpPost("create")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateUserDto createUserDto)
        {
            if (createUserDto == null)
            {
                return BadRequest("Create user data is required.");
            }

            try
            {
                var user = await accountCrud.CreateAsync(createUserDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                await accountCrud.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
