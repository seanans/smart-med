using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IMedicalRecordService
{
    MedicalRecord GetMedicalRecord(int patientId);
    void AddDisease(int patientId, DiseaseRecord diseaseRecord);
    void AddAppointment(int patientId, int appointmentId);
    void SaveMedicalRecord(MedicalRecord medicalRecord);
}