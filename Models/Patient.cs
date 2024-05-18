namespace SmartMed.Models;

public class Patient : User
{
    public MedicalRecord MedicalRecord { get; set; } = new();
}