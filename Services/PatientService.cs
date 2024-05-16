using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class PatientService : IPatientService
{
    private List<User> _users;
    private JsonDataService _jsonDataService;

    public PatientService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _users = jsonDataService.LoadUsers();
    }

    public void AddDisease(int patientId, Disease disease)
    {
        var patient = _users.OfType<Patient>().FirstOrDefault(p => p.Id == patientId);
        if (patientId != null)
        {
            patient.MedicalRecord.Diseases.Add(disease);
            _jsonDataService.SaveUsers(_users);
        }
    }

    public void AddAppointment(int patientId, Appointment appointment)
    {
        var patient = _users.OfType<Patient>().FirstOrDefault(p => p.Id == patientId);
        if (patient != null)
        {
            patient.MedicalRecord.Appointments.Add(appointment);
            _jsonDataService.SaveUsers(_users);
        }
    }

    public void AddMedication(int patientId, Medication medication)
    {
        var patient = _users.OfType<Patient>().FirstOrDefault(p => p.Id == patientId);
        if (patient != null)
        {
            patient.MedicalRecord.Medications.Add(medication);
            _jsonDataService.SaveUsers(_users);
        }
    }

    public List<Disease> GetDiseases(int patientId)
    {
        var patient = _users.OfType<Patient>().FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord.Diseases;
    }

    public List<Appointment> GetAppointments(int patientId)
    {
        var patient = _users.OfType<Patient>().FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord.Appointments;
    }

    public List<Medication> GetMedications(int patientId)
    {
        var patient = _users.OfType<Patient>().FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord.Medications;
    }

    public void SavePatients(List<Patient> patients)
    {
        var userPatients = patients.Cast<User>().ToList();
        _jsonDataService.SaveUsers(userPatients);
    }

    public List<Patient> LoadPatients()
    {
        return _users.OfType<Patient>().ToList();
    }
}