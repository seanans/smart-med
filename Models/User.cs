namespace SmartMed.Models;

public abstract class User
{
    private int _id;
    private string _username;
    private string _password;
    private string _fullname;
    private Role _role;
    private string _email;
    private string _phoneNumber;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Username
    {
        get => _username;
        set => _username = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Password
    {
        get => _password;
        set => _password = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Role Role
    {
        get => Role;
        set => Role = value;
    }

    public string Email
    {
        get => _email;
        set => _email = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Fullname
    {
        get => _fullname;
        set => _fullname = value ?? throw new ArgumentNullException(nameof(value));
    }
}