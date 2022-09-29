using ServerSide.Entity;

namespace ServerSide.Authentication.Interface
{
    public interface IAuthenticationService
    {
        string AuthenticationResult(UserEntity usr);
    }
}