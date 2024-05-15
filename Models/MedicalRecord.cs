namespace SmartMed.Models;

public class MedicalRecord
{
    private int _patientId;
    private List<string> records = new List<string>();

    public int PatientId
    {
        get => _patientId;
        set => _patientId = value;
    }

    public List<string> Records
    {
        get => records;
        set => records = value ?? throw new ArgumentNullException(nameof(value));
    }
}