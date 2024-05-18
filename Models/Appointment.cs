namespace SmartMed.Models;

public class Appointment
{
    private string _symptoms;

    public int Id { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public DateTime DateTime { get; set; }

    public string Symptoms
    {
        get => _symptoms;
        set => _symptoms = value ?? throw new ArgumentNullException(nameof(value));
    }

    public AppointmentStatus AppointmentStatus { get; set; }
}