namespace Cor.Apt.Entities
{
    public class Anamnesis
    {
        public int AnamnesisId { get; set; }
        public string Profession { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Complaint { get; set; }
        public string MedicalHistory { get; set; }
        public double WeightHistoryOnTeen { get; set; }
        public double WeightHistoryOnEducation { get; set; }
        public double WeightHistoryOnWork { get; set; }
        public double WeightHistoryOnMarriage { get; set; }
        public double WeightHistoryOnPregnancy { get; set; }
        public double WeightHistoryOnDiet { get; set; }
        public double WeightHistoryOnTop { get; set; }
        public double WeightHistoryOnLow { get; set; }
        public double WeightHistoryOnDream { get; set; }
        public double WeightHistoryOnGoal { get; set; }
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