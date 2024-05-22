using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

/// <summary>
///     Клас, який надає методи для роботи з даними про зустрічі пацієнтів з лікарями.
/// </summary>
public class AppointmentService(JsonDataService jsonDataService) : IAppointmentService
{
    private readonly List<Appointment> _appointments = jsonDataService.LoadAppointments();

    /// <summary>
    ///     Створює нову зустріч між пацієнтом і лікарем.
    /// </summary>
    /// <param name="doctorId">Ідентифікатор лікаря.</param>
    /// <param name="patientId">Ідентифікатор пацієнта.</param>
    /// <param name="date">Дата і час зустрічі.</param>
    /// <param name="symptoms">Симптоми пацієнта.</param>
    public void ScheduleAppointment(int doctorId, int patientId, DateTime date, string symptoms)
    {
        var newId = _appointments.Any() ? _appointments.Max(a => a.Id) + 1 : 1;
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
        jsonDataService.SaveAppointments(_appointments);
    }

    /// <summary>
    ///     Скасовує вказану зустріч.
    /// </summary>
    /// <param name="appointmentId">Ідентифікатор зустрічі.</param>
    public void CancelAppointment(int appointmentId)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appointment != null)
        {
            appointment.AppointmentStatus = AppointmentStatus.Cancelled;
            jsonDataService.SaveAppointments(_appointments);
        }
    }

    /// <summary>
    ///     Завершує вказану зустріч.
    /// </summary>
    /// <param name="appointmentId">Ідентифікатор зустрічі.</param>
    public void CompleteAppointment(int appointmentId)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appointment != null)
        {
            appointment.AppointmentStatus = AppointmentStatus.Completed;
            jsonDataService.SaveAppointments(_appointments);
        }
    }

    /// <summary>
    ///     Повертає список усіх зустрічей.
    /// </summary>
    /// <returns>Список зустрічей.</returns>
    public List<Appointment> GetAppointments()
    {
        return _appointments;
    }

    /// <summary>
    ///     Зберігає список зустрічей у JSON файл.
    /// </summary>
    /// <param name="appointments">Список зустрічей.</param>
    public void SaveAppointments(List<Appointment> appointments)
    {
        jsonDataService.SaveAppointments(appointments);
    }

    /// <summary>
    ///     Завантажує список зустрічей із JSON файлу.
    /// </summary>
    /// <returns>Список зустрічей.</returns>
    public List<Appointment> LoadAppointments()
    {
        return jsonDataService.LoadAppointments();
    }

    /// <summary>
    ///     Повертає список зустрічей для вказаного лікаря.
    /// </summary>
    /// <param name="doctorId">Ідентифікатор лікаря.</param>
    /// <returns>Список зустрічей.</returns>
    public List<Appointment> GetAppointmentsByDoctor(int doctorId)
    {
        return _appointments.Where(a => a.DoctorId == doctorId).ToList();
    }

    /// <summary>
    ///     Повертає список зустрічей для вказаного пацієнта.
    /// </summary>
    /// <param name="patientId">Ідентифікатор пацієнта.</param>
    /// <returns>Список зустрічей.</returns>
    public List<Appointment> GetAppointmentsByPatient(int patientId)
    {
        return _appointments.Where(a => a.PatientId == patientId).ToList();
    }
}