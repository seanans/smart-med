using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IAppointmentService
{
    void ScheduleAppointment(int doctorId, int patientId, DateTime date, string symptoms);
    List<Appointment> GetAppointments();
    void SaveAppointments(List<Appointment> appointments);
    List<Appointment> LoadAppointments();
    void CancelAppointment(int appointmentId);
    List<Appointment> GetAppointmentsByDoctor(int doctorId);
    List<Appointment> GetAppointmentsByPatient(int patientId);
}