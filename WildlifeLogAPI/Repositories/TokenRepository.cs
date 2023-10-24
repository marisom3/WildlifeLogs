using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace WildlifeLogAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //Create claims (new Claim(claim type, the object we are making the claim for) 

            //create a list for the claims
            var claims = new List<Claim>();

            //create a claim for the emails 
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            //create a claim for the list of roles 
            foreach (var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }

            //Create a key 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            //Create credentials 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //create token 
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            //return the token as a string that we can use inside the controller 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
