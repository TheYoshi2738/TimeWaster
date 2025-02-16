using TimeWaster.Core.Models;

namespace TimeWaster.Core.Services;

public class UserService
{
    private readonly IUsersRepository _usersRepository;

    public UserService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public IEnumerable<User> GetAll()
    {
        return _usersRepository.GetAll();
    }
    
    public User? Get(Guid userId)
    {
        return _usersRepository.Get(userId);
    }
    public User? GetByLogin(string login)
    {
        return _usersRepository.GetByLogin(login);
    }

    public User? Create(User user)
    {
        return _usersRepository.Create(user);    
    }

    public bool CheckUniqueLogin(string login)
    {
        return _usersRepository.GetByLogin(login) is null;
    }

    public User? Update(User user)
    {
        return _usersRepository.Update(user);
    }

    public void Delete(Guid userId)
    {
        _usersRepository.Delete(userId);
    }
}