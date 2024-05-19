using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IPatientService
{
    void SavePatients(List<Patient> patients);
    List<Patient> LoadPatients();
}