using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class LeaveController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public LeaveController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Leave> DataSource = _context.Leaves.Include(i => i.LeaveType).Where(i => i.UserId == pid).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Leave>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Leave> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Leave _leave = new Leave();
            _leave.StartTime = value.Value.StartTime;
            _leave.EndTime = value.Value.EndTime;
            _leave.Days = value.Value.Days;
            _leave.LeaveTypeId = value.Value.LeaveTypeId;
            _leave.UserId = pid;
            _context.Leaves.Add(_leave);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Leave> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _leave = _context.Leaves.Include(i => i.LeaveType).Where(i => i.Id == value.Value.Id).FirstOrDefault();
            if (_leave != null)
            {
                _leave.StartTime = value.Value.StartTime;
                _leave.EndTime = value.Value.EndTime;
                _leave.Days = value.Value.Days;
                _leave.LeaveTypeId = value.Value.LeaveTypeId;
                _leave.UserId = value.Value.UserId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Leave> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _leave = _context.Leaves.Where(i => i.Id == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_leave);
            _context.SaveChanges();
            return Json(value);
        }
    }
}