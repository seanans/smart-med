using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class UserService : IUserService
{
    private List<Patient> patients;
    private List<Doctor> doctors;
    private JsonDataService jsonDataService;
    private User currentUser;

    public UserService(JsonDataService jsonDataService)
    {
        this.jsonDataService = jsonDataService;
        patients = jsonDataService.LoadPatients();
        doctors = jsonDataService.LoadDoctors();
    }

    public User SignIn()
    {
        Console.Write("Ім'я користувача: ");
        string username = Console.ReadLine();
        Console.Write("Пароль: ");
        string password = Console.ReadLine();

        User user = (User)patients.FirstOrDefault(u => u.Username == username && u.Password == password)
                    ?? doctors.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            currentUser = user;
            Console.WriteLine($"Ласкаво просимо, {user.FullName}!");
            return user;
        }
        else
        {
            Console.WriteLine("Неправильні дані.");
            return null;
        }
    }

    public void SignUp()
    {
        Console.WriteLine("Реєстрація");

        Console.Write("Ім'я користувача: ");
        string username = Console.ReadLine();
        Console.Write("Пароль: ");
        string password = Console.ReadLine();
        Console.Write("Повне ім'я: ");
        string fullName = Console.ReadLine();
        Console.Write("Електронна пошта: ");
        string email = Console.ReadLine();
        Console.Write("Номер телефону: ");
        string phoneNumber = Console.ReadLine();

        Console.WriteLine("Виберіть роль: 1 - Пацієнт, 2 - Лікар");
        string roleChoice = Console.ReadLine();
        
        User newUser;
        switch (roleChoice)
        {
            case "1":
                newUser = new Patient { Username = username, Password = password, FullName = fullName, Email = email, PhoneNumber = phoneNumber };
                patients.Add((Patient)newUser);
                jsonDataService.SavePatients(patients);
                break;
            case "2":
                newUser = new Doctor { Username = username, Password = password, FullName = fullName, Email = email, PhoneNumber = phoneNumber };
                Console.Write("Додайте профілі лікаря (через кому): ");
                string profilesInput = Console.ReadLine();
                ((Doctor)newUser).Profiles = profilesInput.Split(',').Select(p => p.Trim()).ToList();
                doctors.Add((Doctor)newUser);
                jsonDataService.SaveDoctors(doctors);
                break;
            default:
                Console.WriteLine("Неправильний вибір ролі.");
                return;
        }
        Console.WriteLine("Реєстрація успішна!");
    }

    public void SignOut()
    {
        Console.WriteLine($"До побачення, {currentUser?.FullName}!");
        currentUser = null;
    }

    public User GetCurrentUser()
    {
        return currentUser;
    }
}