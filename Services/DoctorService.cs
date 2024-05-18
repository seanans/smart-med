using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class DoctorService : IDoctorService
{
    private readonly List<Doctor> _doctors;
    private readonly JsonDataService _jsonDataService;

    public DoctorService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _doctors = jsonDataService.LoadDoctors();
    }

    public void AddAppointment(Appointment appointment)
    {
        var doctor = _doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);
        if (doctor != null)
        {
            doctor.Appointments.Add(appointment);
            _jsonDataService.SaveDoctors(_doctors);
        }
    }

    public List<Appointment> GetAppointments(int doctorId)
    {
        var doctor = _doctors.FirstOrDefault(d => d.Id == doctorId);
        return doctor?.Appointments;
    }

    public void SaveDoctors(List<Doctor> doctors)
    {
        _jsonDataService.SaveDoctors(doctors);
    }

    public List<Doctor> LoadDoctors()
    {
        return _doctors;
    }
}