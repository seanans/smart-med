namespace SmartMed.Models;

public class DiseaseRecord
{
    public int DiseaseId { get; set; }
    public string DiseaseName { get; set; }
    public string DiseaseDescription { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string Symptoms { get; set; }
    public DiseaseStatus Status { get; set; }
    public List<Treatment> Treatments { get; set; } = new();
    public DateTime DateDiagnosed { get; set; }

    public DateTime? DateCured { get; set; }
    public DateTime? DateRecovered { get; set; }
}

public class Treatment
{
    public int MedicationId { get; set; }
    public string Dosage { get; set; }

    public TreatmentStatus TreatmentStatus { get; set; }
}

public enum TreatmentStatus
{
    Taking,
    NotTaking
}

public enum DiseaseStatus
{
    Sick,
    Recovered,
    UnderTreatment
}