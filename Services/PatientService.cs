using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class PatientService : IPatientService
{
    private readonly JsonDataService _jsonDataService;
    private readonly List<Patient> _patients;

    public PatientService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _patients = jsonDataService.LoadPatients();
    }

    public void SavePatients(List<Patient> patients)
    {
        _jsonDataService.SavePatients(patients);
    }

    public List<Patient> LoadPatients()
    {
        return _patients;
    }
}