namespace SmartMed.Models;

public class MedicalRecord
{
    private List<DiseaseRecord> _diseases = [];
    private List<Medication> _medications = [];
    private List<int> _appointmentIds = [];

    public int PatientId { get; set; }

    public List<DiseaseRecord> Diseases
    {
        get => _diseases;
        set => _diseases = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Medication> Medications
    {
        get => _medications;
        set => _medications = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public List<int> AppointmentIds
    {
        get => _appointmentIds;
        set => _appointmentIds = value ?? throw new ArgumentNullException(nameof(value));
    }
}