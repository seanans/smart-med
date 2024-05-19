using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class UserService : IUserService
{
    private readonly List<Doctor> _doctors;
    private readonly JsonDataService _jsonDataService;
    private readonly List<Patient> _patients;
    private User _currentUser;

    public UserService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _patients = jsonDataService.LoadPatients();
        _doctors = jsonDataService.LoadDoctors();
    }

    public User SignIn()
    {
        Console.Write("Ім'я користувача: ");
        var username = Console.ReadLine();
        Console.Write("Пароль: ");
        var password = Console.ReadLine();

        var user = (User)_patients.FirstOrDefault(u => u.Username == username && u.Password == password)
                   ?? _doctors.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            _currentUser = user;
            Console.WriteLine($"Ласкаво просимо, {user.FullName}!");
            return user;
        }

        Console.WriteLine("Неправильні дані.");
        return null;
    }

    public void SignUp()
    {
        Console.WriteLine("Реєстрація");

        Console.Write("Ім'я користувача: ");
        var username = Console.ReadLine();
        
        if (_patients.Any(u => u.Username == username) || _doctors.Any(u => u.Username == username))
        {
            Console.WriteLine("Користувач з таким ім'ям вже існує.");
            return;
        }
        
        Console.Write("Пароль: ");
        var password = Console.ReadLine();
        Console.Write("Повне ім'я: ");
        var fullName = Console.ReadLine();
        Console.Write("Електронна пошта: ");
        var email = Console.ReadLine();
        Console.Write("Номер телефону: ");
        var phoneNumber = Console.ReadLine();

        Console.WriteLine("Виберіть роль: 1 - Пацієнт, 2 - Лікар");
        var roleChoice = Console.ReadLine();

        User newUser;
        int newId;
        switch (roleChoice)
        {
            case "1":
                int maxPatientId = _patients.Any() ? _patients.Max(p => p.Id) : 0;
                newId = maxPatientId + 1;
                newUser = new Patient
                {
                    Id = newId, Username = username, Password = password, FullName = fullName, Email = email,
                    PhoneNumber = phoneNumber
                };
                _patients.Add((Patient)newUser);
                _jsonDataService.SavePatients(_patients);
                break;
            case "2":
                int maxDoctorId = _doctors.Any() ? _doctors.Max(p => p.Id) : 0;
                newId = maxDoctorId + 1;
                newUser = new Doctor
                {
                    Id = newId, Username = username, Password = password, FullName = fullName, Email = email,
                    PhoneNumber = phoneNumber
                };
                Console.Write("Додайте профілі лікаря (через кому): ");
                var profilesInput = Console.ReadLine();
                ((Doctor)newUser).Profiles = profilesInput.Split(',').Select(p => p.Trim()).ToList();
                _doctors.Add((Doctor)newUser);
                _jsonDataService.SaveDoctors(_doctors);
                break;
            default:
                Console.WriteLine("Неправильний вибір ролі.");
                return;
        }
        
        Console.WriteLine("Реєстрація успішна!");
    }

    public void SignOut()
    {
        Console.WriteLine($"До побачення, {_currentUser?.FullName}!");
        _currentUser = null;
    }

    public User GetCurrentUser()
    {
        return _currentUser;
    }
}