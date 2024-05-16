namespace SmartMed.Models;

public class Patient : User
{
    private MedicalRecord _medicalRecord;

    public MedicalRecord MedicalRecord
    {
        get => _medicalRecord;
        set => _medicalRecord = value;
    }

    public Patient()
    {
        this.Role = Role.Patient;
        MedicalRecord = new MedicalRecord() { PatientId = this.Id };
    }
}