using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class OzoneController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public OzoneController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Ozone> DataSource = _context.Ozones.Include(i => i.Patient).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Ozone>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Ozone> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Ozone _ozone = new Ozone();
            _ozone.SessionDate = value.Value.SessionDate;
            _ozone.Session = value.Value.Session;
            _ozone.PatientId = value.Value.PatientId;
            _context.Ozones.Add(_ozone);
            _context.SaveChanges();
            return Json(_ozone);
        }
        public IActionResult Update([FromBody] CRUDModel<Ozone> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _ozone = _context.Ozones.Where(i => i.OzoneId == value.Value.OzoneId).FirstOrDefault();
            if (_ozone != null)
            {
                _ozone.OzoneId = value.Value.OzoneId;
                _ozone.SessionDate = value.Value.SessionDate;
                _ozone.Session = value.Value.Session;
                _ozone.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Ozone> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _ozone = _context.Ozones.Where(i => i.OzoneId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_ozone);
            _context.SaveChanges();
            return Json(value);
        }
    }
}