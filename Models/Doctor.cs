namespace SmartMed.Models;

public class Doctor : User
{
    public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    public List<string> Profiles { get; set; } = new List<string>();
}