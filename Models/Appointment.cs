namespace SmartMed.Models;

public class Appointment
{
    private int _id;
    private int _doctorId;
    private int _patientId;
    private DateTime _dateTime;
    private string _symptoms;
    private AppointmentStatus _appointmentStatus;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int DoctorId
    {
        get => _doctorId;
        set => _doctorId = value;
    }

    public int PatientId
    {
        get => _patientId;
        set => _patientId = value;
    }

    public DateTime DateTime
    {
        get => _dateTime;
        set => _dateTime = value;
    }

    public string Symptoms
    {
        get => _symptoms;
        set => _symptoms = value ?? throw new ArgumentNullException(nameof(value));
    }

    public AppointmentStatus AppointmentStatus
    {
        get => _appointmentStatus;
        set => _appointmentStatus = value;
    }
}