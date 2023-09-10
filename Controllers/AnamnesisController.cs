using System.Linq;
using Microsoft.AspNetCore.Mvc;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;
using System.Threading.Tasks;

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
        public JsonResult Update(Anamnesis value)
        {
            try
            {
                if (_context.Anamnesises.Where(i => i.PatientId == value.PatientId).Any())
                {
                    var _anamnesis = _context.Anamnesises.Where(i => i.PatientId == value.PatientId).FirstOrDefault();
                    _anamnesis.Profession = value.Profession;
                    _anamnesis.Height = value.Height;
                    _anamnesis.Weight = value.Weight;
                    _anamnesis.Complaint = value.Complaint;
                    _anamnesis.MedicalHistory = value.MedicalHistory;
                    _anamnesis.WeightHistoryOnTeen = value.WeightHistoryOnTeen;
                    _anamnesis.WeightHistoryOnEducation = value.WeightHistoryOnEducation;
                    _anamnesis.WeightHistoryOnWork = value.WeightHistoryOnWork;
                    _anamnesis.WeightHistoryOnMarriage = value.WeightHistoryOnMarriage;
                    _anamnesis.WeightHistoryOnPregnancy = value.WeightHistoryOnPregnancy;
                    _anamnesis.WeightHistoryOnDiet = value.WeightHistoryOnDiet;
                    _anamnesis.WeightHistoryOnTop = value.WeightHistoryOnTop;
                    _anamnesis.WeightHistoryOnLow = value.WeightHistoryOnLow;
                    _anamnesis.WeightHistoryOnDream = value.WeightHistoryOnDream;
                    _anamnesis.WeightHistoryOnGoal = value.WeightHistoryOnGoal;
                    _anamnesis.Period = value.Period;
                    _anamnesis.NumberOfBirth = value.NumberOfBirth;
                    _anamnesis.NumberOfMiscarry = value.NumberOfMiscarry;
                    _anamnesis.BabyWeightOnBirth = value.BabyWeightOnBirth;
                    _anamnesis.MedicalOperations = value.MedicalOperations;
                    _anamnesis.MaritalStatus = value.MaritalStatus;
                    _anamnesis.Nutrition = value.Nutrition;
                    _anamnesis.Sleep = value.Sleep;
                    _anamnesis.Sport = value.Sport;
                    _anamnesis.Smoking = value.Smoking;
                    _anamnesis.Alcohol = value.Alcohol;
                    _anamnesis.Caffeine = value.Caffeine;
                    _anamnesis.Drugs = value.Drugs;
                    _anamnesis.Mother = value.Mother;
                    _anamnesis.Father = value.Father;
                    _anamnesis.Siblings = value.Siblings;
                    _anamnesis.Other = value.Other;
                    _anamnesis.PatientId = value.PatientId;
                }
                else
                {
                    Anamnesis _newAnamnesis = new Anamnesis()
                    {
                        Profession = value.Profession,
                        Height = value.Height,
                        Weight = value.Weight,
                        Complaint = value.Complaint,
                        MedicalHistory = value.MedicalHistory,
                        WeightHistoryOnTeen = value.WeightHistoryOnTeen,
                        WeightHistoryOnEducation = value.WeightHistoryOnEducation,
                        WeightHistoryOnWork = value.WeightHistoryOnWork,
                        WeightHistoryOnMarriage = value.WeightHistoryOnMarriage,
                        WeightHistoryOnPregnancy = value.WeightHistoryOnPregnancy,
                        WeightHistoryOnDiet = value.WeightHistoryOnDiet,
                        WeightHistoryOnTop = value.WeightHistoryOnTop,
                        WeightHistoryOnLow = value.WeightHistoryOnLow,
                        WeightHistoryOnDream = value.WeightHistoryOnDream,
                        WeightHistoryOnGoal = value.WeightHistoryOnGoal,
                        Period = value.Period,
                        NumberOfBirth = value.NumberOfBirth,
                        NumberOfMiscarry = value.NumberOfMiscarry,
                        BabyWeightOnBirth = value.BabyWeightOnBirth,
                        MedicalOperations = value.MedicalOperations,
                        MaritalStatus = value.MaritalStatus,
                        Nutrition = value.Nutrition,
                        Sleep = value.Sleep,
                        Sport = value.Sport,
                        Smoking = value.Smoking,
                        Alcohol = value.Alcohol,
                        Caffeine = value.Caffeine,
                        Drugs = value.Drugs,
                        Mother = value.Mother,
                        Father = value.Father,
                        Siblings = value.Siblings,
                        Other = value.Other,
                        PatientId = value.PatientId
                    };
                    _context.Add(_newAnamnesis);
                }
                _context.SaveChanges();
                return Json("OK");

            }
            catch (System.Exception)
            {
                return Json("ERR");   
            }
        }
    }
}