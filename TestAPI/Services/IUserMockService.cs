using TestAPI.Models;

namespace TestAPI.Services
{
    public interface IUserMockService
    {
        bool Register(UserDTO userDTO);
        string? Authenticate(UserDTO user);
    }
}