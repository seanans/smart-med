namespace SmartMed.Models;

public class MedicalRecord
{
    private List<DiseaseRecord> _diseaseRecords = [];
    private List<int> _appointmentIds = [];

    public int PatientId { get; set; }

    public List<DiseaseRecord> DiseasesRecords
    {
        get => _diseaseRecords;
        set => _diseaseRecords = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public List<int> AppointmentIds
    {
        get => _appointmentIds;
        set => _appointmentIds = value ?? throw new ArgumentNullException(nameof(value));
    }
}