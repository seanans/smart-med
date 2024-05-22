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
            existingRecord.DiseasesRecords = medicalRecord.DiseasesRecords;
            existingRecord.AppointmentIds = medicalRecord.AppointmentIds;
        }
        else
        {
            _medicalRecords.Add(medicalRecord);
        }
        jsonDataService.SaveMedicalRecords(_medicalRecords);
    }

    public void AddDisease(int patientId, DiseaseRecord diseaseRecord)
    {
        var record = GetMedicalRecord(patientId);
        record.DiseasesRecords.Add(diseaseRecord);
        SaveMedicalRecord(record);
    }

    public void AddAppointment(int patientId, int appointmentId)
    {
        var record = GetMedicalRecord(patientId);
        record.AppointmentIds.Add(appointmentId);
        SaveMedicalRecord(record);
    }
}