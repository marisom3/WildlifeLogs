using Microsoft.AspNetCore.Identity;

namespace WildlifeLogAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
