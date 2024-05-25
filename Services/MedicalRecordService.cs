using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

/// <summary>
///     Клас, який надає методи для роботи з медичними картками пацієнтів.
/// </summary>
public class MedicalRecordService(JsonDataService jsonDataService) : IMedicalRecordService
{
    private readonly List<MedicalRecord> _medicalRecords = jsonDataService.LoadMedicalRecords();

    /// <summary>
    ///     Отримує медичну картку пацієнта за його ідентифікатором.
    /// </summary>
    /// <param name="patientId">Ідентифікатор пацієнта.</param>
    /// <returns>Медична картка пацієнта або нова картка, якщо її не знайдено.</returns>
    public MedicalRecord GetMedicalRecord(int patientId)
    {
        return _medicalRecords.FirstOrDefault(r => r.PatientId == patientId) ??
               new MedicalRecord { PatientId = patientId };
    }

    /// <summary>
    ///     Додає запис про хворобу до медичної картки пацієнта.
    /// </summary>
    /// <param name="patientId">Ідентифікатор пацієнта.</param>
    /// <param name="diseaseRecord">Запис про хворобу.</param>
    public void AddDisease(int patientId, DiseaseRecord diseaseRecord)
    {
        var record = GetMedicalRecord(patientId);
        record.DiseasesRecords.Add(diseaseRecord);
        SaveMedicalRecord(record);
    }

    /// <summary>
    ///     Додає ідентифікатор запису про зустріч до медичної картки пацієнта.
    /// </summary>
    /// <param name="patientId">Ідентифікатор пацієнта.</param>
    /// <param name="appointmentId">Ідентифікатор запису про зустріч.</param>
    public void AddAppointment(int patientId, int appointmentId)
    {
        var record = GetMedicalRecord(patientId);
        record.AppointmentIds.Add(appointmentId);
        SaveMedicalRecord(record);
    }

    /// <summary>
    ///     Зберігає медичну картку пацієнта у JSON файл.
    /// </summary>
    /// <param name="medicalRecord">Медична картка.</param>
    public void SaveMedicalRecord(MedicalRecord medicalRecord)
    {
        var existingRecord = _medicalRecords.FirstOrDefault(r => r.PatientId == medicalRecord.PatientId);
        if (existingRecord != null)
        {
            existingRecord.DiseasesRecords = medicalRecord.DiseasesRecords;
            existingRecord.AppointmentIds = medicalRecord.AppointmentIds;
        }
        else
        {
            _medicalRecords.Add(medicalRecord);
        }

        jsonDataService.SaveMedicalRecords(_medicalRecords);
    }
}