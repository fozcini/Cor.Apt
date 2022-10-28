namespace Cor.Apt.Entities
{
    public class Anamnesis
    {
        public int AnamnesisId { get; set; }
        public int Age { get; set; }
        public string Profession {get; set;}
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Diet { get; set; }
        public string Disease { get; set; }
        public string Smoking { get; set; }
        public string Alcohol { get; set; }
        public string Allergy { get; set; }
        public string ConstipationAndDiarrhea { get; set; }
        public string Drugs { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}