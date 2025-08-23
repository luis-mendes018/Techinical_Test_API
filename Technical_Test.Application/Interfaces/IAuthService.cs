using Technical_Test.Application.DTOs;

namespace Technical_Test.Application.Interfaces;

public interface IAuthService
{   
    Task<string> LoginAsync(LoginDto loginDto);
}
