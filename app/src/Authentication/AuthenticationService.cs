using ServerSide.Authentication.Interface;
using ServerSide.Entity;
using ServerSide.Interface;
using ServerSide.Models;

namespace ServerSide.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public string AuthenticationResult(UserEntity usr)
        {
            User? user;
            using (var context = new ApplicationDbContext())
            {
                var result = context.Users.Where(u => u.Name == usr.Name && usr.Pwd == u.Pwd);

                user = result.FirstOrDefault();
            }
            
            if (user == null)
                return "";
            
            var token = _jwtTokenGenerator.GenerateJwtToken(user);

            return token;
        }
    }
}
