using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class PhysicalExaminationController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public PhysicalExaminationController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<PhysicalExamination> DataSource = _context.PhysicalExaminations.Where(i => i.PatientId == pid).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<PhysicalExamination>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<PhysicalExamination> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            PhysicalExamination _physcialExamination = new PhysicalExamination();
            _physcialExamination.RecordDate = value.Value.RecordDate;
            _physcialExamination.BloodPressure = value.Value.BloodPressure;
            _physcialExamination.Pulse = value.Value.Pulse;
            _physcialExamination.Description = value.Value.Description;
            _physcialExamination.PatientId = pid;
            _context.PhysicalExaminations.Add(_physcialExamination);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<PhysicalExamination> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _physcialExamination = _context.PhysicalExaminations.Where(i => i.PhysicalExaminationId == value.Value.PhysicalExaminationId).FirstOrDefault();
            if (_physcialExamination != null)
            {
                _physcialExamination.RecordDate = value.Value.RecordDate;
                _physcialExamination.BloodPressure = value.Value.BloodPressure;
                _physcialExamination.Pulse = value.Value.Pulse;
                _physcialExamination.Description = value.Value.Description;
                _physcialExamination.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<PhysicalExamination> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _physcialExamination = _context.PhysicalExaminations.Where(i => i.PhysicalExaminationId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_physcialExamination);
            _context.SaveChanges();
            return Json(value);
        }
    }
}