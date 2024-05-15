namespace SmartMed.Models;

public class Doctor : User
{
    private List<Appointment> _appointments = new List<Appointment>();

    public List<Appointment> Appointments
    {
        get => _appointments;
        set => _appointments = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Doctor()
    {
        Role = Role.Doctor;
    }
}