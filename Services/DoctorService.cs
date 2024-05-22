using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class DoctorService(JsonDataService jsonDataService) : IDoctorService
{
    private readonly List<Doctor> _doctors = jsonDataService.LoadDoctors();

    public void SaveDoctors(List<Doctor> doctors)
    {
        jsonDataService.SaveDoctors(doctors);
    }

    public List<Doctor> LoadDoctors()
    {
        return _doctors;
    }

    public Doctor getDoctorById(int doctorId)
    {
        return _doctors.FirstOrDefault(d => d.Id == doctorId);
    }

    public List<Appointment> GetAppointments(int doctorId)
    {
        return jsonDataService.LoadAppointments().Where(a => a.DoctorId == doctorId).ToList();
    }
}