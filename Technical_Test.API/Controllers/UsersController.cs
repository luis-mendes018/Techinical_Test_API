using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technical_Test.Application.DTOs.UsersDTOs;
using Technical_Test.Application.Interfaces;

namespace Technical_Test.API.Controllers;

[Route("api/v1/users")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var pagedResult = await _userService.GetAllUsersAsync(page, pageSize);

            Response.Headers.Append("X-Pagination-Current-Page", pagedResult.CurrentPage.ToString());
            Response.Headers.Append("X-Pagination-Page-Size", pagedResult.PageSize.ToString());
            Response.Headers.Append("X-Pagination-Total-Items", pagedResult.TotalItems.ToString());
            Response.Headers.Append("X-Pagination-Total-Pages", pagedResult.TotalPages.ToString());

            return Ok(pagedResult.Items);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error {0}", ex.Message);
            return StatusCode(500, "Error processing request");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> EditUser([FromBody] EditUserDto editUserDto)
    {
        var success = await _userService.UpdateUserAsync(editUserDto.UserId, editUserDto.NewUsername);
        if (!success)
        {
            return NotFound("User not found or no changes were made.");
        }
        return Ok("User updated successfully.");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
        {
            return NotFound("User not found.");
        }
        return Ok("User deleted successfully.");
    }
}
