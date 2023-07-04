using HiringTask.DTO;
using HiringTask.Helpers;
using HiringTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HiringTask.Services
{
    public class AuthService : IAuthService
    {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly SignInManager<User> _signeManager;
            private readonly Jwt _jwt;

            public AuthService(UserManager<User> userManager, SignInManager<User> SigneManager, RoleManager<IdentityRole> roleManager, IOptions<Jwt> jwt)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _signeManager = SigneManager;
                _jwt = jwt.Value;
            }

            public async Task<Auth> RegisterAsync(RegisterDTO model)
            {
                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    return new Auth { Message = "Email is already registered!" };

                if (await _userManager.FindByNameAsync(model.UserName) is not null)
                    return new Auth { Message = "Username is already registered!" };

                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    ArabicName = model.ArabicName,
                    EnglishName = model.EnglishName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    return new Auth { Message = errors };
                }

                var jwtSecurityToken = await CreateJwtToken(user);

                return new Auth
                {
                    Email = user.Email,
                    ExpireOn = jwtSecurityToken.ValidTo,
                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    UserName = user.UserName
                };
            }

            public async Task<Auth> GetTokenAsync(TokenRequestDT model)
            {
                var Auth = new Auth();

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    Auth.Message = "Email or Password is incorrect!";
                    return Auth;
                }

                var jwtSecurityToken = await CreateJwtToken(user);

                Auth.IsAuthenticated = true;
                Auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                Auth.Email = user.Email;
                Auth.UserName = user.UserName;
                Auth.ExpireOn = jwtSecurityToken.ValidTo;

                return Auth;
            }



        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.ExpireTimeInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async  Task<Auth> LoginAsync(LoginDTO model)
        {
          var CurrenUser = await _userManager.FindByEmailAsync(model.Email);
            if(CurrenUser == null)
            {
                return new Auth { Message = "Email is not registered!" , IsAuthenticated=false };
            }
            var LoginResult = await _signeManager.PasswordSignInAsync(CurrenUser, model.Password,false,lockoutOnFailure:false);
            if(!LoginResult.Succeeded )
            {
                return new Auth { Message = "Password is incorrect!", IsAuthenticated = false };
            }
            var jwtSecurityToken = await CreateJwtToken(CurrenUser);
            return new Auth
            {
                Email = CurrenUser.Email,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = CurrenUser.UserName
            };

        }

        public async Task<bool> LogoutAsync()
        {
            await _signeManager.SignOutAsync();
            return true;
        }

    }
}
