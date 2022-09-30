using ServerSide.Authentication.Interface;
using ServerSide.Entity;
using ServerSide.Interface;
using ServerSide.Models;
using ServerSide.Repository.Interface;

namespace ServerSide.Authentication
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
