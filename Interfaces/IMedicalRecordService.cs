using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IMedicalRecordService
{
    MedicalRecord GetMedicalRecord(int patientId);
    void SaveMedicalRecord(MedicalRecord medicalRecord);
    
}