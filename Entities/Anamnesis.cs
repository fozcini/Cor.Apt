namespace Cor.Apt.Entities
{
    public class Anamnesis
    {
        public int AnamnesisId { get; set; }
        public string Profession { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Complaint { get; set; }
        public string MedicalHistory { get; set; }
        public string WeightHistoryOnTeen { get; set; }
        public string WeightHistoryOnEducation { get; set; }
        public string WeightHistoryOnWork { get; set; }
        public string WeightHistoryOnMarriage { get; set; }
        public string WeightHistoryOnPregnancy { get; set; }
        public string WeightHistoryOnDiet { get; set; }
        public string WeightHistoryOnTop { get; set; }
        public string WeightHistoryOnLow { get; set; }
        public string WeightHistoryOnDream { get; set; }
        public string WeightHistoryOnGoal { get; set; }
        public string Period { get; set; }
        public int NumberOfBirth { get; set; }
        public int NumberOfMiscarry { get; set; }
        public string BabyWeightOnBirth { get; set; }
        public string MedicalOperations { get; set; }
        public string Smoking { get; set; }
        public string Alcohol { get; set; }
        public string Caffeine { get; set; }
        public string Drugs { get; set; }
        public string MaritalStatus { get; set; }
        public string Nutrition { get; set; }
        public string Sleep { get; set; }
        public string Sport { get; set; }
        public string Mother { get; set; }
        public string Father { get; set; }
        public string Siblings { get; set; }
        public string Other { get; set; }


        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}