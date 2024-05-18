using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IMedicalRecordService
{
    MedicalRecord GetMedicalRecord(int patientId);
    void SaveMedicalRecords(List<MedicalRecord> medicalRecords);
    List<MedicalRecord> LoadMedicalRecords();
}