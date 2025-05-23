using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;
using System;

namespace Cor.Apt.Controllers
{
    public class DiagnosisRecordController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;


        public DiagnosisRecordController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse", "Secretary" })) return RedirectToAction("Index", "Auth");
            IEnumerable<DiagnosisRecord> DataSource = _context.DiagnosisRecords.Where(i => i.PatientId == pid).OrderByDescending(i => i.RecordDate).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<DiagnosisRecord>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<DiagnosisRecord> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            DiagnosisRecord _diagnosisRecord = new DiagnosisRecord
            {
                Description = value.Value.Description,
                RecordDate = value.Value.RecordDate,
                DiagnosisId = value.Value.DiagnosisId,
                PatientId = pid
            };
            _context.DiagnosisRecords.Add(_diagnosisRecord);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<DiagnosisRecord> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            var _diagnosisRecord = _context.DiagnosisRecords.Where(i => i.DiagnosisRecordId == value.Value.DiagnosisRecordId).FirstOrDefault();
            if (_diagnosisRecord != null)
            {
                _diagnosisRecord.RecordDate = value.Value.RecordDate;
                _diagnosisRecord.Description = value.Value.Description;
                _diagnosisRecord.DiagnosisId = value.Value.DiagnosisId;
                _diagnosisRecord.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<DiagnosisRecord> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            var _diagnosisRecord = _context.DiagnosisRecords.Where(i => i.DiagnosisRecordId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_diagnosisRecord);
            _context.SaveChanges();
            return Json(value);
        }
    }
}