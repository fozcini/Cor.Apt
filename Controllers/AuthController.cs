using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using Cor.Apt.Models;
using Cor.Apt.Helpers;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, IOptions<AppSettings> appSettings)
        {
            _authService = authService;
            _appSettings = appSettings.Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AuthenticateModel authenticateModel)
        {
            var _hashed = ComputeSha256Hash(authenticateModel.Password + _appSettings.Salt);
            authenticateModel.Password = _hashed;
            if (ModelState.IsValid)
            {
                if (_authService.Authenticate(authenticateModel))
                {
                    if(HttpContext.Session.GetInt32("rl") == 1) return RedirectToAction("Appointment", "User");
                    if(HttpContext.Session.GetInt32("rl") == 2) return RedirectToAction("Index", "Admin");
                    if(HttpContext.Session.GetInt32("rl") == 3) return RedirectToAction("Index", "Master");
                    if(HttpContext.Session.GetInt32("rl") == 4) return RedirectToAction("Index", "Accountant");
                }
                else ModelState.AddModelError(string.Empty, "Kullanıcı adı veya parola hatalı.");
            }
            return View();
        }
        
        public IActionResult LogOut()
        {
            if (_authService.LogOut()) return RedirectToAction("Index", "Auth");
            else return RedirectToAction("Error", "Auth");
        }

        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}