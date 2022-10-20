using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApp.Data.Entities.Classes;
using WebApp.Data.Repositories.Interfaces;
using WebApp.Infrastructure.Interfaces;
using WebApp.Shared.Models;
using WebApp.Shared.Token;
using WebApp.Shared.Dto;
using WebApp.Shared.Messages;
using AutoMapper;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebApp.Shared.Enums;
using System.Text;

namespace WebApp.Infrastructure.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IGenericRepository<User> genericRepository, IOptions<AuthOptions> authOptions,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _genericRepository = genericRepository;
            _authOptions = authOptions;
            _mapper = mapper;
        }

        public async Task<UserModel> AddAsync(CreateUserModel userModel, RoleType role)
        {
            var user = await _userRepository.GetByEmailAsync(userModel.Email);
            if (user != null) throw new NullReferenceException(Exceptions.UserIsFoundByEmail);

            user = _mapper.Map<CreateUserModel, User>(userModel);
            user.PasswordHash = HashPassword(userModel.Password);
            user.RoleId = (int)role;

            await _genericRepository.AddAsync(user);
            return _mapper.Map<User, UserModel>(user);
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(await _genericRepository.GetAllAsync());
        }

        public async Task<UserModel?> GetByIdAsync(int id)
        {
            return _mapper.Map<User, UserModel>(await _genericRepository.GetByIdAsync(id));
        }

        public async Task UpdateAsync(UserModel userModel)
        {
            var user = await _userRepository.GetByIdAsync(userModel.Id);
            if (user == null) throw new NullReferenceException(Exceptions.UserNotFoundByEmail);

            user.Name = userModel.Name;
            user.Surname = userModel.Surname;
            user.Email = userModel.Email;

            await _genericRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NullReferenceException(Exceptions.UserNotFoundByEmail);

            user.IsDeleted = true;
            await _genericRepository.UpdateAsync(user);
        }

        public async Task<TokenDto> GetTokenAsync(LoginModel loginModel)
        {
            var identity = await GetIdentity(loginModel);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _authOptions.Value.Issuer,
                    audience: _authOptions.Value.Audience,
                    notBefore: now,
                    claims: identity.Identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_authOptions.Value.Lifetime)),
                    signingCredentials: new SigningCredentials(_authOptions.Value.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new TokenDto()
            {
                Token = encodedJwt,
                Email = identity.Email
            };
        }

        private async Task<IdentityDto> GetIdentity(LoginModel loginModel)
        {
            User? user = await _userRepository.GetByEmailAsync(loginModel.Email);
            if (user == null) throw new NullReferenceException(Exceptions.UserNotFoundByEmail);

            if (user.PasswordHash != HashPassword(loginModel.Password)) throw new ArgumentException(Exceptions.IncorrectPassword);

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentityExtension.DefaultEmailClaimType, user.Email),
                    new Claim(ClaimsIdentityExtension.DefaultRoleClaimType, user.Role.Name)
                };
            ClaimsIdentityExtension claimsIdentity =
            new ClaimsIdentityExtension(claims, "Token", ClaimsIdentityExtension.DefaultEmailClaimType,
                ClaimsIdentityExtension.DefaultRoleClaimType);
            return new IdentityDto()
            {
                Identity = claimsIdentity,
                Email = user.Email
            };
        }

        private static string HashPassword(string password)
        {
            const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1;
            const int Pbkdf2IterCount = 1000;
            const int Pbkdf2SubkeyLength = 256 / 8;
            const int SaltSize = 128 / 8;

            byte[] salt = Encoding.ASCII.GetBytes("saltsecuritybytes!123");
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);

            var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
            outputBytes[0] = 0x00;
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }
    }
}
