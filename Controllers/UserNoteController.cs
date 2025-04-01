using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class UserNoteController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public UserNoteController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            IEnumerable<UserNote> DataSource = _context.UserNotes.Where(i => i.UserId == pid).OrderByDescending(i => i.RecordDate).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<UserNote>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<UserNote> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            UserNote _userNote = new UserNote();
            _userNote.RecordDate = value.Value.RecordDate;
            _userNote.Description = value.Value.Description;
            _userNote.UserId = pid;
            _context.UserNotes.Add(_userNote);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<UserNote> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            var _userNote = _context.UserNotes.Where(i => i.UserNoteId == value.Value.UserNoteId).FirstOrDefault();
            if (_userNote != null)
            {
                _userNote.RecordDate = value.Value.RecordDate;
                _userNote.Description = value.Value.Description;
                _userNote.UserId = value.Value.UserId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<UserNote> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            var _userNote = _context.UserNotes.Where(i => i.UserNoteId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_userNote);
            _context.SaveChanges();
            return Json(value);
        }
    }
}