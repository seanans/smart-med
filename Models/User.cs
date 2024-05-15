namespace SmartMed.Models;

public abstract class User
{
    private int id;
    private string username;
    private string password;
    private Role role;
    private string email;
    private string phoneNumber;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string Username
    {
        get => username;
        set => username = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Password
    {
        get => password;
        set => password = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Role Role
    {
        get => Role;
        set => Role = value;
    }

    public string Email
    {
        get => email;
        set => email = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string PhoneNumber
    {
        get => phoneNumber;
        set => phoneNumber = value ?? throw new ArgumentNullException(nameof(value));
    }
}