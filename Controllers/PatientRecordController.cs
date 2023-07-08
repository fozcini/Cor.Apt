using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class PatientRecordController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _accesor;

        public PatientRecordController(IAuthService authService, AppointmentContext context, IHttpContextAccessor accesor)
        {
            _context = context;
            _authService = authService;
            _accesor = accesor;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<PatientRecord> DataSource =  _context.PatientRecords.Where(i => i.PatientId == pid).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<PatientRecord>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<PatientRecord> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            PatientRecord _patientRecord = new PatientRecord();
            int uid = (int)_accesor.HttpContext.Session.GetInt32("userid");
            _patientRecord.Description = value.Value.Description;
            _patientRecord.RecordDate = value.Value.RecordDate;
            _patientRecord.UserId = uid;
            _patientRecord.PatientId = pid;
            _context.PatientRecords.Add(_patientRecord);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<PatientRecord> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _patientRecord = _context.PatientRecords.Where(i => i.PatientRecordId == value.Value.PatientRecordId).FirstOrDefault();
            if (_patientRecord != null)
            {
                int uid = (int)_accesor.HttpContext.Session.GetInt32("userid");
                _patientRecord.RecordDate = value.Value.RecordDate;
                _patientRecord.Description = value.Value.Description;
                _patientRecord.UserId = uid;
                _patientRecord.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<PatientRecord> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _patientRecord = _context.PatientRecords.Where(i => i.PatientRecordId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_patientRecord);
            _context.SaveChanges();
            return Json(value);
        }
    }
}