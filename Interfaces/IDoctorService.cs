using SmartMed.Models;

namespace SmartMed.Interfaces;

public interface IDoctorService
{
    void SaveDoctors(List<Doctor> doctors);
    List<Doctor> LoadDoctors();

    Doctor GetDoctorById(int doctorId);
}