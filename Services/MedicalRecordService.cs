using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class MedicalRecordService(JsonDataService jsonDataService) : IMedicalRecordService
{
    private readonly List<Patient> _patients = jsonDataService.LoadPatients();

    public MedicalRecord GetMedicalRecord(int patientId)
    {
        var patient = _patients.FirstOrDefault(p => p.Id == patientId);
        return patient?.MedicalRecord;
    }

    public void SaveMedicalRecord(MedicalRecord medicalRecord)
    {
        var patient = _patients.FirstOrDefault(p => p.Id == medicalRecord.PatientId);
        if (patient != null)
        {
            patient.MedicalRecord = medicalRecord;
            jsonDataService.SavePatients(_patients);
        }
    }
}