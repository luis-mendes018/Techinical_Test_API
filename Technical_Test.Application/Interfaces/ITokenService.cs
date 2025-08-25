using Technical_Test.Domain.Entities;

namespace Technical_Test.Application.Interfaces;

public interface ITokenService
{
     string GenerateJwtToken(User user, IEnumerable<string> roles);
}
