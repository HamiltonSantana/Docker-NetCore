using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerSide.Domain.Models;
using ServerSide.Infra.Authentication;
using ServerSide.Infra.Authentication.Interface;
using ServerSide.Infra.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerSide.Infra.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly AuthSettings _authSettings;

        public JwtTokenGenerator(IOptions<AuthSettings> authSettingsOptions)
        {
            _authSettings = authSettingsOptions.Value;
        }

        public string GenerateJwtToken(User? user)
        {
            var siginingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.Latin1.GetBytes(_authSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Phone.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var signToken = new JwtSecurityToken(
                    issuer: _authSettings.Issuer,
                    expires: DateTime.Now.AddMinutes(_authSettings.Expire),
                    claims: claims,
                    signingCredentials: siginingCredentials
                ) ;

            return new JwtSecurityTokenHandler().WriteToken(signToken);
        }
    }
}
