using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class MedicalRecordService(JsonDataService jsonDataService) : IMedicalRecordService
{
    private readonly List<MedicalRecord> _medicalRecords = jsonDataService.LoadMedicalRecords();

    public MedicalRecord GetMedicalRecord(int patientId)
    {
        return _medicalRecords.FirstOrDefault(r => r.PatientId == patientId) ??
               new MedicalRecord { PatientId = patientId };
    }

    public void SaveMedicalRecord(MedicalRecord medicalRecord)
    {
        var existingRecord = _medicalRecords.FirstOrDefault(r => r.PatientId == medicalRecord.PatientId);
        if (existingRecord != null)
        {
            existingRecord.Diseases = medicalRecord.Diseases;
            existingRecord.Medications = medicalRecord.Medications;
            existingRecord.AppointmentIds = medicalRecord.AppointmentIds;
        }
        else
        {
            _medicalRecords.Add(medicalRecord);
        }
        jsonDataService.SaveMedicalRecords(_medicalRecords);
    }

    public void AddDisease(int patientId, Disease disease)
    {
        var record = GetMedicalRecord(patientId);
        record.Diseases.Add(disease);
        SaveMedicalRecord(record);
    }

    public void AddMedication(int patientId, Medication medication)
    {
        var record = GetMedicalRecord(patientId);
        record.Medications.Add(medication);
        SaveMedicalRecord(record);
    }

    public void AddAppointment(int patientId, int appointmentId)
    {
        var record = GetMedicalRecord(patientId);
        record.AppointmentIds.Add(appointmentId);
        SaveMedicalRecord(record);
    }
}