using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class AndulationRecordController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AndulationRecordController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse", "Secretary" })) return RedirectToAction("Index", "Auth");
            IEnumerable<AndulationRecord> DataSource = _context.AndulationRecords.Where(i => i.PatientId == pid).OrderByDescending(i => i.SessionDate).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<AndulationRecord>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<AndulationRecord> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            AndulationRecord _andulationRecord = new AndulationRecord();
            _andulationRecord.SessionDate = value.Value.SessionDate;
            _andulationRecord.Session = value.Value.Session;
            _andulationRecord.Chest = value.Value.Chest;
            _andulationRecord.Waist = value.Value.Waist;
            _andulationRecord.Abdomen = value.Value.Abdomen;
            _andulationRecord.Hip = value.Value.Hip;
            _andulationRecord.RightArm = value.Value.RightArm;
            _andulationRecord.RightLeg = value.Value.RightLeg;
            _andulationRecord.PatientId = pid;
            _context.AndulationRecords.Add(_andulationRecord);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<AndulationRecord> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            var _andulationRecord = _context.AndulationRecords.Where(i => i.AndulationRecordId == value.Value.AndulationRecordId).FirstOrDefault();
            if (_andulationRecord != null)
            {
                _andulationRecord.SessionDate = value.Value.SessionDate;
                _andulationRecord.Session = value.Value.Session;
                _andulationRecord.Chest = value.Value.Chest;
                _andulationRecord.Waist = value.Value.Waist;
                _andulationRecord.Abdomen = value.Value.Abdomen;
                _andulationRecord.Hip = value.Value.Hip;
                _andulationRecord.RightArm = value.Value.RightArm;
                _andulationRecord.RightLeg = value.Value.RightLeg;
                _andulationRecord.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<AndulationRecord> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            var _andulationRecord = _context.AndulationRecords.Where(i => i.AndulationRecordId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_andulationRecord);
            _context.SaveChanges();
            return Json(value);
        }
    }
}