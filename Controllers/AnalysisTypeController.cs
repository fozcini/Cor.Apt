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
        public IActionResult Update(List<Cor.Apt.Entities.Type> types)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            int _analysisId = Convert.ToInt32(HttpContext.Request.Form["AnalysisId"]);
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
                            IsEndoklinikTest = false
                        };
                        _newAnalysisTypes.Add(_analysisType);
                    }
                }
                _context.AddRange(_newAnalysisTypes);
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
                        IsEndoklinikTest = false
                    };
                    _analysisTypes.Add(_analysisType);
                }
                _context.AddRange(_analysisTypes);
            }
            _context.SaveChanges();
            return RedirectToAction("Analysis", "User", new { analysisId = HttpContext.Request.Form["AnalysisId"] });
        }
    }
}