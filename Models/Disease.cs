namespace SmartMed.Models;

public class Disease
{
    private string _description;
    private string _name;
    private string _profile;

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Description
    {
        get => _description;
        set => _description = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Profile
    {
        get => _profile;
        set => _profile = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<string> Symptoms { get; set; } = new();
}