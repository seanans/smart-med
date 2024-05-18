namespace SmartMed.Models;

public class MedicalRecord
{
    private List<Appointment> _appointments = [];
    private List<Disease> _diseases = [];
    private List<Medication> _medications = [];

    public int PatientId { get; set; }

    public List<Disease> Diseases
    {
        get => _diseases;
        set => _diseases = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Appointment> Appointments
    {
        get => _appointments;
        set => _appointments = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Medication> Medications
    {
        get => _medications;
        set => _medications = value ?? throw new ArgumentNullException(nameof(value));
    }
}