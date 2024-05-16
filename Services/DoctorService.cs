using SmartMed.Models;

namespace SmartMed.Services;

public class DoctorService
{
    private List<User> _users;
    private JsonDataService _jsonDataService;

    public DoctorService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _users = jsonDataService.LoadUsers();
    }
    
    public void AddAppointment(Appointment appointment)
    {
        var doctor = _users.OfType<Doctor>().FirstOrDefault(d => d.Id == appointment.DoctorId);
        if (doctor != null)
        {
            doctor.Appointments.Add(appointment);
            _jsonDataService.SaveUsers(_users);
        }
    }

    public List<Appointment> GetAppointments(int doctorId)
    {
        var doctor = _users.OfType<Doctor>().FirstOrDefault(d => d.Id == doctorId);
        return doctor?.Appointments;
    }

    public void SaveDoctors(List<Doctor> doctors)
    {
        var userDoctors = doctors.Cast<User>().ToList();
        _jsonDataService.SaveUsers(userDoctors);
    }

    public List<Doctor> LoadDoctors()
    {
        return _users.OfType<Doctor>().ToList();
    }
}