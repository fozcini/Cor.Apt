using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;

namespace Cor.Apt.Controllers
{
    public class AnamnesisController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AnamnesisController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Update(Anamnesis value)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            if (_context.Anamnesises.Where(i => i.PatientId == value.PatientId).Any())
            {
                var _anamnesis = _context.Anamnesises.Where(i => i.PatientId == value.PatientId).FirstOrDefault();
                _anamnesis.Age = value.Age;
                _anamnesis.Height = value.Height;
                _anamnesis.Weight = value.Weight;
                _anamnesis.Profession = value.Profession;
                _anamnesis.Alcohol = value.Alcohol;
                _anamnesis.Smoking = value.Smoking;
                _anamnesis.ConstipationAndDiarrhea = value.ConstipationAndDiarrhea;
                _anamnesis.Drugs = value.Drugs;
                _anamnesis.Allergy = value.Allergy;
                _anamnesis.Diet = value.Diet;
                _anamnesis.Disease = value.Disease;
            }
            else
            {
                Anamnesis _newAnamnesis = new Anamnesis() {
                    Age = value.Age,
                    Height = value.Height,
                    Weight = value.Weight,
                    Profession = value.Profession,
                    Alcohol = value.Alcohol,
                    Smoking = value.Smoking,
                    ConstipationAndDiarrhea = value.ConstipationAndDiarrhea,
                    Drugs = value.Drugs,
                    Allergy = value.Allergy,
                    Diet = value.Diet,
                    Disease = value.Disease,
                    PatientId = value.PatientId
                };
                _context.Add(_newAnamnesis);
                Console.WriteLine("EKLENDI");
            }
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = value.PatientId });
        }
    }
}