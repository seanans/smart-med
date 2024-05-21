using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class AppointmentService(JsonDataService jsonDataService) : IAppointmentService
{
    private readonly JsonDataService _jsonDataService = jsonDataService;
    private readonly List<Appointment> _appointments = jsonDataService.LoadAppointments();

    public void ScheduleAppointment(int doctorId, int patientId, DateTime date, string symptoms)
    {
        int newId = _appointments.Any() ? _appointments.Max(a => a.Id) + 1 : 1;
        var appointment = new Appointment
        {
            Id = newId,
            DoctorId = doctorId,
            PatientId = patientId,
            DateTime = date,
            Symptoms = symptoms,
            AppointmentStatus = AppointmentStatus.Scheduled
        };
        
        _appointments.Add(appointment);
        _jsonDataService.SaveAppointments(_appointments);
    }

    public void CancelAppointment(int appointmentId)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appointment != null)
        {
            appointment.AppointmentStatus = AppointmentStatus.Cancelled;
            _jsonDataService.SaveAppointments(_appointments);
        }
    }

    public void CompleteAppointment(int appointmentId)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appointment != null)
        {
            appointment.AppointmentStatus = AppointmentStatus.Completed;
            _jsonDataService.SaveAppointments(_appointments);
        }
    }


    public List<Appointment> GetAppointments() => _appointments;

    public void SaveAppointments(List<Appointment> appointments) => _jsonDataService.SaveAppointments(appointments);

    public List<Appointment> LoadAppointments() => _jsonDataService.LoadAppointments();
    
    public List<Appointment> GetAppointmentsByDoctor(int doctorId) => _appointments.Where(a => a.DoctorId == doctorId).ToList();
    public List<Appointment> GetAppointmentsByPatient(int patientId) => _appointments.Where(a => a.PatientId == patientId).ToList();
}