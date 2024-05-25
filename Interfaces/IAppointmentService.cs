using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IAppointmentService
{
    void ScheduleAppointment(int doctorId, int patientId, DateTime date, string symptoms, int? diseaseId = null);
    List<Appointment> GetAppointments();
    void SaveAppointments(List<Appointment> appointments);
    List<Appointment> LoadAppointments();
    void CancelAppointment(int appointmentId);
    void CompleteAppointment(int appointmentId);
    List<Appointment> GetAppointmentsByDoctor(int doctorId);
    List<Appointment> GetAppointmentsByPatient(int patientId);
}