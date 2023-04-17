using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public PatientController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Patient> DataSource = _context.Patients.ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Patient>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Patient> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Patient _patient = new Patient();
            _patient.IdentificationNumber = value.Value.IdentificationNumber;
            _patient.FullName = value.Value.FullName;
            _patient.BirthDate = value.Value.BirthDate;
            _patient.Gender = value.Value.Gender;
            _patient.Phone = value.Value.Phone;
            _patient.SecondPhone = value.Value.SecondPhone;
            _patient.ThirdPhone = value.Value.ThirdPhone;
            _patient.Email = value.Value.Email;
            _patient.Address = value.Value.Address;
            _patient.Reference = value.Value.Reference;
            _patient.DiscountTypeId = value.Value.DiscountTypeId;
            _patient.SocialSecurityId = value.Value.SocialSecurityId;
            _context.Patients.Add(_patient);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Patient> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _patient = _context.Patients.Where(i => i.PatientId == value.Value.PatientId).FirstOrDefault();
            if (_patient != null)
            {
                _patient.IdentificationNumber = value.Value.IdentificationNumber;
                _patient.FullName = value.Value.FullName;
                _patient.BirthDate = value.Value.BirthDate;
                _patient.Gender = value.Value.Gender;
                _patient.Phone = value.Value.Phone;
                _patient.SecondPhone = value.Value.SecondPhone;
                _patient.ThirdPhone = value.Value.ThirdPhone;
                _patient.Email = value.Value.Email;
                _patient.Address = value.Value.Address;
                _patient.Reference = value.Value.Reference;
                _patient.DiscountTypeId = value.Value.DiscountTypeId;
                _patient.SocialSecurityId = value.Value.SocialSecurityId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Patient> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _patient = _context.Patients.Where(i => i.PatientId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_patient);
            _context.SaveChanges();
            return Json(value);
        }

        [HttpPost]
        public IActionResult UpdatePatientByUser(Patient value)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _patient = _context.Patients.Where(i => i.PatientId == value.PatientId).FirstOrDefault();
            if (_patient != null)
            {
                _patient.FullName = value.FullName;
                _patient.IdentificationNumber = value.IdentificationNumber;
                _patient.BirthDate = value.BirthDate;
                _patient.Gender = value.Gender;
                _patient.Phone = value.Phone;
                _patient.SecondPhone = value.SecondPhone;
                _patient.ThirdPhone = value.ThirdPhone;
                _patient.Email = value.Email;
                _patient.Address = value.Address;
                _patient.Reference = value.Reference;
                _patient.DiscountTypeId = value.DiscountTypeId;
                _patient.SocialSecurityId = value.SocialSecurityId;
            }
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = value.PatientId });
        }
    }
}