
using ServerSide.Domain.Entity;
using ServerSide.Infra.Authentication.Interface;
using ServerSide.Infra.Interface;
using ServerSide.Infra.Repository.Interface;

namespace ServerSide.Infra.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public string AuthenticationResult(UserEntity usr)
        {
            var user = _userRepository.GetUsersByName(usr.Name, usr.Pwd);
            
            if (user == null)
                return "";
            
            var token = _jwtTokenGenerator.GenerateJwtToken(user);

            return token;
        }
    }
}
