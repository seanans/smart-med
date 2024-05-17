using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class DoctorService : IDoctorService
{
    private List<Doctor> doctors;
    private JsonDataService jsonDataService;

    public DoctorService(JsonDataService jsonDataService)
    {
        this.jsonDataService = jsonDataService;
        doctors = jsonDataService.LoadDoctors();
    }

    public void AddAppointment(Appointment appointment)
    {
        var doctor = doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);
        if (doctor != null)
        {
            doctor.Appointments.Add(appointment);
            jsonDataService.SaveDoctors(doctors);
        }
    }

    public List<Appointment> GetAppointments(int doctorId)
    {
        var doctor = doctors.FirstOrDefault(d => d.Id == doctorId);
        return doctor?.Appointments;
    }

    public void SaveDoctors(List<Doctor> doctors)
    {
        jsonDataService.SaveDoctors(doctors);
    }

    public List<Doctor> LoadDoctors()
    {
        return doctors;
    }
}