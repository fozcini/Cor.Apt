using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;

namespace Cor.Apt.Controllers
{
    public class RadiologyRequestTypeController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public RadiologyRequestTypeController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Update()
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            int _radiologyRequestId = Convert.ToInt32(HttpContext.Request.Form["RadiologyRequestId"]);
            int _patientId = Convert.ToInt32(HttpContext.Request.Form["PatientId"]);
            if (_context.RadiologyRequestTypes.Where(i => i.RadiologyRequestId == _radiologyRequestId).Any())
            {
                List<RadiologyRequestType> _radiologyRequestTypes = _context.RadiologyRequestTypes.Where(i => i.RadiologyRequestId == _radiologyRequestId).ToList();
                List<RadiologyRequestType> _newRadiologyRequestTypes = new List<RadiologyRequestType>();
                foreach (var typeNo in HttpContext.Request.Form["RadiologyRequestTypeLists"])
                {
                    Cor.Apt.Entities.RadiologyRequestTypeList _type = _context.RadiologyRequestTypeLists.Where(i => i.RadiologyRequestTypeListId == Convert.ToInt32(typeNo)).FirstOrDefault();
                    if (!_radiologyRequestTypes.Any(i => i.RadiologyRequestTypeListId == _type.RadiologyRequestTypeListId))
                    {
                        RadiologyRequestType _radiologyRequestType = new RadiologyRequestType
                        {
                            Price = _type.Price,
                            RadiologyRequestTypeListId = _type.RadiologyRequestTypeListId,
                            RadiologyRequestId = _radiologyRequestId,
                            IsDone = false,
                            IsEndoklinikTest = "Endoklinik"
                        };
                        _newRadiologyRequestTypes.Add(_radiologyRequestType);
                    }
                }
                _context.AddRange(_newRadiologyRequestTypes);
            }
            else
            {
                if (HttpContext.Request.Form["RadiologyRequestTypeLists"].Count == 0)
                {
                    return RedirectToAction("Remove", "RadiologyRequestType", new { radiologyRequestId = _radiologyRequestId, patientId = _patientId });
                }
                else
                {
                    List<RadiologyRequestType> _radiologyRequestTypes = new List<RadiologyRequestType>();
                    foreach (var typeNo in HttpContext.Request.Form["RadiologyRequestTypeLists"])
                    {
                        Cor.Apt.Entities.RadiologyRequestTypeList _type = _context.RadiologyRequestTypeLists.Where(i => i.RadiologyRequestTypeListId == Convert.ToInt32(typeNo)).FirstOrDefault();
                        RadiologyRequestType _radiologyRequestType = new RadiologyRequestType
                        {
                            Price = _type.Price,
                            RadiologyRequestTypeListId = _type.RadiologyRequestTypeListId,
                            RadiologyRequestId = _radiologyRequestId,
                            IsDone = false,
                            IsEndoklinikTest = "Endoklinik"
                        };
                        _radiologyRequestTypes.Add(_radiologyRequestType);
                    }
                    _context.AddRange(_radiologyRequestTypes);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = _patientId });
        }
        [HttpPost]
        public IActionResult Set()
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            int _radiologyRequestId = Convert.ToInt32(HttpContext.Request.Form["RadiologyRequestId"]);
            int _patientId = Convert.ToInt32(HttpContext.Request.Form["PatientId"]);
            List<RadiologyRequestType> radiologyRequestTypes = _context.RadiologyRequestTypes.Where(i => i.RadiologyRequestId == _radiologyRequestId).ToList();
            RadiologyRequest _radiologyRequest = _context.RadiologyRequests.Where(i => i.RadiologyRequestId == _radiologyRequestId).FirstOrDefault();
            _radiologyRequest.Description = HttpContext.Request.Form["Description"];
            foreach (var radiologyRequestType in radiologyRequestTypes)
            {
                radiologyRequestType.IsEndoklinikTest = HttpContext.Request.Form[radiologyRequestType.RadiologyRequestTypeId.ToString()].ToString();
            }
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = _patientId });
        }

        public IActionResult Remove(int radiologyRequestId, int patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            if (_context.RadiologyRequestTypes.Any(i => i.RadiologyRequestId == radiologyRequestId))
            {
                List<RadiologyRequestType> _radiologyRequestTypes = _context.RadiologyRequestTypes.Where(i => i.RadiologyRequestId == radiologyRequestId).ToList();
                _context.RemoveRange(_radiologyRequestTypes);
                _context.SaveChanges();
            }
            RadiologyRequest _radiologyRequest = _context.RadiologyRequests.Where(i => i.RadiologyRequestId == radiologyRequestId).FirstOrDefault();
            _context.Remove(_radiologyRequest);
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = patientId });
        }
    }
}