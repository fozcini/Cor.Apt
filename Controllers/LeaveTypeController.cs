using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class LeaveTypeController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public LeaveTypeController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<LeaveType> DataSource = _context.LeaveTypes.ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<LeaveType>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<LeaveType> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            LeaveType _leaveType = new LeaveType();
            _leaveType.LeaveTypeName = value.Value.LeaveTypeName;
            _context.LeaveTypes.Add(_leaveType);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<LeaveType> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _leaveType = _context.LeaveTypes.Where(i => i.LeaveTypeId == value.Value.LeaveTypeId).FirstOrDefault();
            if (_leaveType != null)
            {
                _leaveType.LeaveTypeName = value.Value.LeaveTypeName;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<LeaveType> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _leaveType = _context.LeaveTypes.Where(i => i.LeaveTypeId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_leaveType);
            _context.SaveChanges();
            return Json(value);
        }
    }
}