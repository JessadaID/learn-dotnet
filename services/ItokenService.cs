using learn_dotnet.models;

namespace learn_dotnet.services;

public interface ITokenService
{
    string GenerateToken(User user);
}