using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAPI.Models;

namespace TestAPI.Services
{
    public class UserMockService : IUserMockService
    {
        private readonly List<UserDTO> _users = new List<UserDTO>();
        private readonly IConfiguration _configuration;

        public UserMockService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? Authenticate(UserDTO userDTO)
        {
            var existingUser = _users.FirstOrDefault(u => u.Name == userDTO.Name && u.Password == userDTO.Password);
            if (existingUser == null)
                return null;

            var key = _configuration.GetRequiredSection("JwtSettings")["SecretKey"] ?? throw new ArgumentNullException();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = new System.Security.Claims.ClaimsIdentity([new Claim(ClaimTypes.Name, existingUser.Name)]),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool Register(UserDTO userDTO)
        {
            if (_users.Any(item => item.Name == userDTO.Name))
                return false;
            _users.Add(userDTO);
            return true;
        }
    }
}
