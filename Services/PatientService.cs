using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;
/// <summary>
///     Клас для управління даними пацієнтів.
/// </summary>
public class PatientService(JsonDataService jsonDataService) : IPatientService
{
    private readonly List<Patient> _patients = jsonDataService.LoadPatients();

    /// <summary>
    ///     Зберігає список пацієнтів у JSON файл.
    /// </summary>
    /// <param name="patients">Список пацієнтів.</param>
    public void SavePatients(List<Patient> patients)
    {
        jsonDataService.SavePatients(patients);
    }
    /// <summary>
    ///     Завантажує список пацієнтів із JSON файлу.
    /// </summary>
    /// <returns>Список пацієнтів.</returns>
    public List<Patient> LoadPatients()
    {
        return _patients;
    }
}