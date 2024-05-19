using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class PatientService : IPatientService
{
    private readonly JsonDataService _jsonDataService;
    private readonly List<Patient> _patients;

    public PatientService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _patients = jsonDataService.LoadPatients();
    }

    public void AddDisease(int patientId, Disease disease)
    {
        var patient = _patients.FirstOrDefault(p => p.Id == patientId);
        if (patient != null)
        {
            patient.MedicalRecord.Diseases.Add(disease);
            _jsonDataService.SavePatients(_patients);
        }
    }

    public void AddAppointment(int patientId, int appointmentId)
    {
        var patient = _patients.FirstOrDefault(p => p.Id == patientId);
        if (patient != null)
        {
            patient.MedicalRecord.AppointmentIds.Add(appointmentId);
            _jsonDataService.SavePatients(_patients);
        }
    }

    public void AddMedication(int patientId, Medication medication)
    {
        var patient = _patients.FirstOrDefault(p => p.Id == patientId);
        if (patient != null)
        {
            patient.MedicalRecord.Medications.Add(medication);
            _jsonDataService.SavePatients(_patients);
        }
    }

    public List<Disease> GetDiseases(int patientId)
    {
        var patient = _patients.FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord.Diseases;
    }

    public List<Appointment> GetAppointments(int patientId)
    {
        var appointmentIds = _patients.FirstOrDefault(p => p.Id == patientId)?.MedicalRecord.AppointmentIds;
        return _jsonDataService.LoadAppointments().Where(a => appointmentIds.Contains(a.Id)).ToList();
    }

    public List<Medication> GetMedications(int patientId)
    {
        var patient = _patients.FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord.Medications;
    }

    public void SavePatients(List<Patient> patients)
    {
        _jsonDataService.SavePatients(patients);
    }

    public List<Patient> LoadPatients()
    {
        return _patients;
    }
}