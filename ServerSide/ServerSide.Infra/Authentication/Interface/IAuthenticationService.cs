using ServerSide.Domain.Entity;

namespace ServerSide.Infra.Authentication.Interface
{
    public interface IAuthenticationService
    {
        string AuthenticationResult(UserEntity usr);
    }
}