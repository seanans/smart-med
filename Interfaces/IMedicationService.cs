using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IMedicationService
{
    void AddMedication(Medication medication);
    List<Medication> GetMedication();
    void SaveMedications(List<Medication> medications);
    List<Medication> LoadMedication();
}