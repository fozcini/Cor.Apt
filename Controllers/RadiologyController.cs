using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class RadiologyController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public RadiologyController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Radiology> DataSource = _context.Radiologies.Include(i => i.RadiologyType).Where(i => i.PatientId == pid).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Radiology>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Radiology> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Radiology _radiology = new Radiology();
            _radiology.RecordDate = value.Value.RecordDate;
            _radiology.Description = value.Value.Description;
            _radiology.RadiologyTypeId = value.Value.RadiologyTypeId;
            _radiology.PatientId = pid;
            _context.Radiologies.Add(_radiology);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Radiology> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _radiology = _context.Radiologies.Include(i => i.RadiologyType).Where(i => i.RadiologyId == value.Value.RadiologyId).FirstOrDefault();
            if (_radiology != null)
            {
                _radiology.RecordDate = value.Value.RecordDate;
                _radiology.Description = value.Value.Description;
                _radiology.RadiologyTypeId = value.Value.RadiologyTypeId;
                _radiology.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Radiology> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _radiology = _context.Radiologies.Where(i => i.RadiologyId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_radiology);
            _context.SaveChanges();
            return Json(value);
        }
    }
}