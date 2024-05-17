using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class PatientService : IPatientService
{
    private List<Patient> patients;
    private JsonDataService jsonDataService;

    public PatientService(JsonDataService jsonDataService)
    {
        this.jsonDataService = jsonDataService;
        patients = jsonDataService.LoadPatients();
    }

    public void AddDisease(int patientId, Disease disease)
    {
        var patient = patients.FirstOrDefault(p => p.Id == patientId);
        if (patient != null)
        {
            patient.MedicalRecord.Diseases.Add(disease);
            jsonDataService.SavePatients(patients);
        }
    }

    public void AddAppointment(int patientId, Appointment appointment)
    {
        var patient = patients.FirstOrDefault(p => p.Id == patientId);
        if (patient != null)
        {
            patient.MedicalRecord.Appointments.Add(appointment);
            jsonDataService.SavePatients(patients);
        }
    }

    public void AddMedication(int patientId, Medication medication)
    {
        var patient = patients.FirstOrDefault(p => p.Id == patientId);
        if (patient != null)
        {
            patient.MedicalRecord.Medications.Add(medication);
            jsonDataService.SavePatients(patients);
        }
    }

    public List<Disease> GetDiseases(int patientId)
    {
        var patient = patients.FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord.Diseases;
    }

    public List<Appointment> GetAppointments(int patientId)
    {
        var patient = patients.FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord.Appointments;
    }

    public List<Medication> GetMedications(int patientId)
    {
        var patient = patients.FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord.Medications;
    }

    public void SavePatients(List<Patient> patients)
    {
        jsonDataService.SavePatients(patients);
    }

    public List<Patient> LoadPatients()
    {
        return patients;
    }
}