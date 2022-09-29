using ServerSide.Models;

namespace ServerSide.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(User? user);
    }
}
