namespace SmartMed.Models;

public class MedicalRecord
{
    private int _patientId;
    private List<Disease> _diseases = new List<Disease>();
    private List<Appointment> _appointments = new List<Appointment>();
    private List<Medication> _medications = new List<Medication>();

    public int PatientId
    {
        get => _patientId;
        set => _patientId = value;
    }

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