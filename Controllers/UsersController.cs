using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Collections.Generic;
using Syncfusion.EJ2.Base;

using Cor.Apt.Helpers;
using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;
        private readonly AppSettings _appSettings;

        public UsersController(IAuthService authService, IOptions<AppSettings> appSettings, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
            _appSettings = appSettings.Value;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<User> DataSource = _context.Users.ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<User>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<User> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            User _user = new User();
            _user.IdentificationNumber = value.Value.IdentificationNumber;
            _user.FullName = value.Value.FullName;
            _user.BirthDate = value.Value.BirthDate;
            _user.StartTime = value.Value.StartTime;
            _user.Phone = value.Value.Phone;
            _user.SecondPhone = value.Value.SecondPhone;
            _user.Email = value.Value.Email;
            _user.RegistrationNumber = value.Value.RegistrationNumber;
            _user.Address = value.Value.Address;
            _user.Description = value.Value.Description;
            _user.Password = ComputeSha256Hash(_user.IdentificationNumber.Substring(0,6) + _appSettings.Salt);
            _user.IsActive = true;
            _user.PhotoLink = "";
            _user.RoleId = 1;
            _context.Users.Add(_user);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<User> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _user = _context.Users.Where(i => i.UserId == value.Value.UserId).FirstOrDefault();
            if (_user != null)
            {
                _user.IdentificationNumber = value.Value.IdentificationNumber;
                _user.FullName = value.Value.FullName;
                _user.BirthDate = value.Value.BirthDate;
                _user.StartTime = value.Value.StartTime;
                _user.Phone = value.Value.Phone;
                _user.SecondPhone = value.Value.SecondPhone;
                _user.Email = value.Value.Email;
                _user.RegistrationNumber = value.Value.RegistrationNumber;
                _user.Address = value.Value.Address;
                _user.Description = value.Value.Description;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<User> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _user = _context.Users.Where(i => i.UserId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_user);
            _context.SaveChanges();
            return Json(value);
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