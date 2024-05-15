using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IDoctorService
{
    void AddAppointment(Appointment appointment);
    List<Appointment> GetAppointments(int doctorId);
    void SaveDoctors(List<Doctor> doctors);
    List<Doctor> LoadDoctors();
}