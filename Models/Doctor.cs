namespace SmartMed.Models;

public class Doctor : User
{
    private List<Appointment> _appointments = new List<Appointment>();
    private List<string> profiles = new List<string>();
    public List<Appointment> Appointments
    {
        get => _appointments;
        set => _appointments = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<string> Profiles
    {
        get => profiles;
        set => profiles = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Doctor()
    {
        Role = Role.Doctor;
    }
}