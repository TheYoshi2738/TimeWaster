namespace TimeWaster.Core.Models;

public class User
{
    public Guid Id { get; }
    public string Login { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<Interval>? Intervals { get; }

    public static User Create(string login, string name)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException($"'{nameof(login)}' cannot be null or whitespace.", nameof(login));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }
        
        return new User(login, name);
    }

    public User(Guid id, string login, string name, IEnumerable<Interval>? intervals)
    {
        Id = id;
        Login = login;
        Name = name;
        Intervals = intervals;
    }

    private User(string login, string name)
    {
        Id = Guid.NewGuid();
        Login = login;
        Name = name;
    }

    public void ChangeLogin(string newLogin)
    {
        Login = newLogin;
    }
    public void ChangeName(string newName)
    {
        Name = newName;
    }
}