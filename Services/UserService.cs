using SmartMed.Interfaces;
using SmartMed.Models;

namespace SmartMed.Services;

public class UserService : IUserService
{
    private List<User> _users;
    private JsonDataService _jsonDataService;
    private User _currentUser;

    public UserService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _users = jsonDataService.LoadUsers();
    }

    public User SignIn()
    {
        Console.Write("Ім'я користувача: ");
        string username = Console.ReadLine();
        Console.Write("Пароль: ");
        string password = Console.ReadLine();

        User user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            _currentUser = user;
            Console.WriteLine($"Ласкаво просимо, {user.Fullname}!");
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
                newUser = new Patient { Username = username, Password = password, Fullname = fullName, Email = email, PhoneNumber = phoneNumber };
                break;
            case "2":
                newUser = new Doctor { Username = username, Password = password, Fullname = fullName, Email = email, PhoneNumber = phoneNumber };
                Console.Write("Додайте профілі лікаря (через кому): ");
                string profilesInput = Console.ReadLine();
                ((Doctor)newUser).Profiles = profilesInput.Split(',').Select(p => p.Trim()).ToList();
                break;
            default:
                Console.WriteLine("Неправильний вибір ролі.");
                return;
        }

        _users.Add(newUser);
        _jsonDataService.SaveUsers(_users);
        Console.WriteLine("Реєстрація успішна!");
    }

    public void SignOut()
    {
        Console.WriteLine($"До побачення, {_currentUser.Fullname}!");
        _currentUser = null;
    }

    public User GetCurrentUser()
    {
        return _currentUser;
    }
}