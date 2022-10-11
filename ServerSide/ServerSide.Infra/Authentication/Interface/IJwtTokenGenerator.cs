using ServerSide.Domain.Models;


namespace ServerSide.Infra.Authentication.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(User? user);
    }
}
