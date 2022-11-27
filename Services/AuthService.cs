using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models.AuthModels;
using WebApplication1.Models.DatabaseModels;

namespace WebApplication1.Services
{
    public class AuthService
    {
        IConfiguration _configuration;
        DbDataContext _context;
        public AuthService(IConfiguration configuration, DbDataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        internal string GenerateToken(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_configuration.GetSection("Jwt:Issuer").Value,
                _configuration.GetSection("Jwt:Issuer").Value,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string Login(UserLoginModel currUser)
        {
            User currentUser = _context.Users.Where(x => x.Email == currUser.Email).SingleOrDefault();
            if (currentUser != null)
            {
                string hashedPass = PasswordHasher(currUser.Password);
                if (hashedPass == currentUser.Password)
                {
                    return GenerateToken(currentUser);
                }

            }
            throw new Exception("Wrong username or password!");
        }

        public string Register(UserModel currUser)
        {
            User currentUser = _context.Users.Where(x => x.Email == currUser.Email).SingleOrDefault();
            if (currentUser != null)
            {
                throw new Exception("Email is taken!");
            }
            User userForSave = new User()
            {
                Email = currUser.Email,
                Password = PasswordHasher(currUser.Password),
                FirstName = currUser.FirstName,
                LastName = currUser.LastName,
                Role = currUser.Role
            };

            _context.Users.Add(userForSave);
            _context.SaveChanges();

            int id = userForSave.UserId;
            AddConnectionsBetweenUsers(id);
            return GenerateToken(userForSave);
        }

        internal string PasswordHasher(string password)
        {
            string salt = _configuration.GetSection("PasswordSalt").Value;
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        internal void AddConnectionsBetweenUsers(int userId)
        {
            List<User> userIds = _context.Users.Where(x => x.UserId != userId).Select(x => new User
            {
                UserId = x.UserId,
            }).ToList();

            foreach(var user in userIds)
            {
                Room currentRoom = new Room() { Name = "test", Type = true};
                _context.Rooms.Add(currentRoom);
                _context.SaveChanges();
                _context.Participants.AddRange(new Participant() { RoomId = currentRoom.RoomId, UserId = user.UserId});
                _context.Participants.AddRange(new Participant() { RoomId = currentRoom.RoomId, UserId = userId });
            }
            _context.SaveChanges();
        }

        public User GetUserLocalInfo(int UserId)
        {
            User currentUser = _context.Users.Where(x => x.UserId == UserId).SingleOrDefault();

            return currentUser;
        }
    }

}
