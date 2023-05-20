using System;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using Cor.Apt.Models;
using Cor.Apt.Helpers;
using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly AppointmentContext _context;
        private readonly IHttpContextAccessor _accesor;

        public AuthService(IOptions<AppSettings> appSettings, AppointmentContext context, IHttpContextAccessor accesor)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _accesor = accesor;
        }

        public bool Authenticate(AuthenticateModel authenticateModel)
        {
            var user = _context.Users.Include(i => i.Role).SingleOrDefault(x => x.IdentificationNumber == authenticateModel.IdentificationNumber && x.Password == authenticateModel.Password && x.IsActive);
            if (user == null) return false;
            
            _accesor.HttpContext.Session.SetInt32("userid", user.UserId);
            _accesor.HttpContext.Session.SetString("fullname", user.FullName);
            _accesor.HttpContext.Session.SetInt32("rl", user.Role.RoleId);
            _accesor.HttpContext.Session.SetString("token", CreateToken(user.UserId.ToString(), user.Role.RoleName));
            return true;
        }

        public bool LogOut()
        {
            try
            {
                _accesor.HttpContext.Session.Remove("userid");
                _accesor.HttpContext.Session.Remove("fullname");
                _accesor.HttpContext.Session.Remove("rl");
                _accesor.HttpContext.Session.Remove("token");
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public string CreateToken(string id, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Issuer,
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, id),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1200),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public bool UserIsValid(List<string> rl)
        {
            var token = _accesor.HttpContext.Session.GetString("token");
            if (string.IsNullOrWhiteSpace(token)) return false;

            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = _appSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = _appSettings.Issuer
                }, out SecurityToken validateToken);

                var jwtSecurityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var userRole = jwtSecurityToken.Claims.FirstOrDefault(i => i.Type == "role").Value;
                if (!rl.Contains(userRole)) return false;
                if (validateToken.ValidTo < DateTime.UtcNow) return false;
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}