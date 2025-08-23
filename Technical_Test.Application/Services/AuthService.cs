using Technical_Test.Application.DTOs;
using Technical_Test.Application.Interfaces;
using Technical_Test.Domain.Repositories.Interfaces;

namespace Technical_Test.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

        if (user == null)
        {
            // Retorna null ou lança uma exceção se o usuário não for encontrado
            return null;
        }

        // TODO: Implementar a validação da senha aqui (usando uma biblioteca de hash)

        // Por enquanto, apenas retorna um token se o usuário existir
        return await Task.FromResult("token-placeholder-aqui");
    }
}
