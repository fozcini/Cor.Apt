using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;

namespace Cor.Apt.Controllers
{
    public class AnalysisTypeController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AnalysisTypeController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Update()
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            int _analysisId = Convert.ToInt32(HttpContext.Request.Form["AnalysisId"]);
            int _patientId = Convert.ToInt32(HttpContext.Request.Form["PatientId"]);
            if (_context.AnalysisTypes.Where(i => i.AnalysisId == _analysisId).Any())
            {
                List<AnalysisType> _analysisTypes = _context.AnalysisTypes.Where(i => i.AnalysisId == _analysisId).ToList();
                List<AnalysisType> _newAnalysisTypes = new List<AnalysisType>();
                foreach (var typeNo in HttpContext.Request.Form["Type"])
                {
                    Cor.Apt.Entities.Type _type = _context.Types.Where(i => i.TypeId == Convert.ToInt32(typeNo)).FirstOrDefault();
                    if (!_analysisTypes.Any(i => i.TypeId == _type.TypeId))
                    {
                        AnalysisType _analysisType = new AnalysisType
                        {
                            Price = _type.Price,
                            TypeId = _type.TypeId,
                            AnalysisId = _analysisId,
                            IsDone = false,
                            IsEndoklinikTest = "Endoklinik"
                        };
                        _newAnalysisTypes.Add(_analysisType);
                    }
                }
                _context.AddRange(_newAnalysisTypes);
            }
            else
            {
                if (HttpContext.Request.Form["Type"].Count == 0)
                {
                    return RedirectToAction("Remove", "AnalysisType", new { analysisId = _analysisId, patientId = _patientId });
                }
                else
                {
                    List<AnalysisType> _analysisTypes = new List<AnalysisType>();
                    foreach (var typeNo in HttpContext.Request.Form["Type"])
                    {
                        Cor.Apt.Entities.Type _type = _context.Types.Where(i => i.TypeId == Convert.ToInt32(typeNo)).FirstOrDefault();
                        AnalysisType _analysisType = new AnalysisType
                        {
                            Price = _type.Price,
                            TypeId = _type.TypeId,
                            AnalysisId = _analysisId,
                            IsDone = false,
                            IsEndoklinikTest = "Endoklinik"
                        };
                        _analysisTypes.Add(_analysisType);
                    }
                    _context.AddRange(_analysisTypes);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = _patientId });
        }
        [HttpPost]
        public IActionResult Set()
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            int _analysisId = Convert.ToInt32(HttpContext.Request.Form["AnalysisId"]);
            int _patientId = Convert.ToInt32(HttpContext.Request.Form["PatientId"]);
            List<AnalysisType> analysisTypes = _context.AnalysisTypes.Where(i => i.AnalysisId == _analysisId).ToList();
            Analysis _analysis = _context.Analyses.Where(i => i.AnalysisId == _analysisId).FirstOrDefault();
            _analysis.Description = HttpContext.Request.Form["Description"];
            foreach (var analysisType in analysisTypes)
            {
                analysisType.IsEndoklinikTest = HttpContext.Request.Form[analysisType.AnalysisTypeId.ToString()].ToString();
            }
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = _patientId });
        }

        public IActionResult Remove(int analysisId, int patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            if (_context.AnalysisTypes.Any(i => i.AnalysisId == analysisId))
            {
                List<AnalysisType> _analysisTypes = _context.AnalysisTypes.Where(i => i.AnalysisId == analysisId).ToList();
                _context.RemoveRange(_analysisTypes);
                _context.SaveChanges();
            }
            Analysis _analysis = _context.Analyses.Where(i => i.AnalysisId == analysisId).FirstOrDefault();
            _context.Remove(_analysis);
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = patientId });
        }
    }
}