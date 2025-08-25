using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Technical_Test.Application.DTOs.RolesDTOs;
using Technical_Test.Application.Interfaces;
using Technical_Test.Application.Services;

namespace Technical_Test.API.Controllers;

[Route("api/v1/roles")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IValidator<UpdateCreateRoleDto> _validator;

    public RolesController(IRoleService roleService, IValidator<UpdateCreateRoleDto> validator)
    {
        _roleService = roleService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(int id)
    {
        try
        {
            var role = await _roleService.GetRoleByIdAsync(id);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            return Ok(role);
        }
        catch (Exception ex)
        {

            Console.WriteLine("Error {0}", ex.Message);
            return StatusCode(500, "Error processing request");
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] UpdateCreateRoleDto updateCreateDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(updateCreateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var roleName = updateCreateDto.NewName;
            var success = await _roleService.CreateRoleAsync(roleName);
            if (!success)
            {
                return BadRequest("Failed to create role. It may already exist.");
            }
            return Ok("Role created successfully.");

        }
        catch (Exception ex)
        {

            Console.WriteLine("Error log: {0} ", ex);
            return StatusCode(500, "Error processing request");
        }

    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateCreateRoleDto updateCreateDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(updateCreateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var success = await _roleService.UpdateRoleAsync(id, updateCreateDto.NewName);
            if (!success)
            {
                return NotFound("Role not found.");
            }
            return Ok("Role updated successfully.");

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error log: {0} ", ex);
            return StatusCode(500, "Error processing request");
        }

    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var success = await _roleService.DeleteRoleAsync(id);
            if (!success)
            {
                return NotFound("Role not found.");
            }
            return Ok("Role deleted successfully.");

        }
        catch (Exception ex)
        {

            Console.WriteLine("Error log: {0} ", ex);
            return StatusCode(500, "Error processing request");
        }

    }
}
