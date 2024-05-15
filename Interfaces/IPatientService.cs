using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IPatientService
{
    void AddDisease(int patientId, Disease disease);
    void AddAppointment(int patientId, Appointment appointment);
    void AddMedication(int patientId, Medication medication);
    List<Disease> GetDiseases(int patientId);
    List<Appointment> GetAppointments(int patientId);
    List<Medication> GetMedications(int patientId);
    void SavePatients(List<Patient> patients);
    List<Patient> LoadPatients();
}