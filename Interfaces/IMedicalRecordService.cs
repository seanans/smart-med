using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IMedicalRecordService
{
    MedicalRecord GetMedicalRecord(int patientId);
    void SaveMedicalRecord(MedicalRecord medicalRecord);
    void AddDisease(int patientId, Disease disease);
    void AddMedication(int patientId, Medication medication);
    void AddAppointment(int patientId, int appointmentId);
    
}