using AspNet_Core6.Fundamentals.Models;

namespace AspNet_Core6.Fundamentals.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
