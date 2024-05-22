using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

/// <summary>
///     Клас для управління даними лікарів.
/// </summary>
public class DoctorService(JsonDataService jsonDataService) : IDoctorService
{
    private readonly List<Doctor> _doctors = jsonDataService.LoadDoctors();

    /// <summary>
    ///     Зберігає список лікарів у JSON файл.
    /// </summary>
    /// <param name="doctors">Список лікарів.</param>
    public void SaveDoctors(List<Doctor> doctors)
    {
        jsonDataService.SaveDoctors(doctors);
    }

    /// <summary>
    ///     Завантажує список лікарів із JSON файлу.
    /// </summary>
    /// <returns>Список лікарів.</returns>
    public List<Doctor> LoadDoctors()
    {
        return _doctors;
    }

    /// <summary>
    ///     Повертає лікаря за його ідентифікатором.
    /// </summary>
    /// <param name="doctorId">Ідентифікатор лікаря.</param>
    /// <returns>Лікар або null, якщо лікаря з таким ідентифікатором не знайдено.</returns>
    public Doctor GetDoctorById(int doctorId)
    {
        return _doctors.FirstOrDefault(d => d.Id == doctorId)!;
    }
}