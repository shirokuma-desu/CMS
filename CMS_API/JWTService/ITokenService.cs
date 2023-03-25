using CMS_API.Models;

namespace CMS_API.JWTService
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}
