using WebApiInalambria.DTOs.Authentication;

namespace WebApiInalambria.Ports
{
    public interface IAuthenticationRepositoryPort
    {
        string GenerateJwtToken(string username);
    }
}
