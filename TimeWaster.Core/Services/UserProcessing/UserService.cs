using System.Collections;
using TimeWaster.Core.Models;

namespace TimeWaster.Core.Services.UserProcessing;

public class UserService : IUserService
{
    private readonly IUsersRepository _usersRepository;

    public UserService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public Result<IEnumerable<User>> GetAll()
    {
        return Result<IEnumerable<User>>.Success(_usersRepository.GetAll());
    }
    
    public Result<User?> Get(Guid id)
    {
        return Result<User?>.Success(_usersRepository.Get(id));
    }
    public Result<User?> GetByLogin(string login)
    {
        return Result<User?>.Success(_usersRepository.GetByLogin(login));
    }

    public Result<User?> Create(User user)
    {
        if (!CheckUniqueLogin(user.Login))
        {
            return Result<User?>.Failure("Login is not unique");
        }
        
        return Result<User?>.Success(_usersRepository.Create(user));    
    }

    public bool CheckUniqueLogin(string login)
    {
        return _usersRepository.GetByLogin(login) is null;
    }

    public Result<User?> Update(User user)
    {
        var existingUser = _usersRepository.Get(user.Id);
        
        if (existingUser is null)
        {
            return Result<User?>.Failure("User not found");
        }
        
        return Result<User?>.Success(_usersRepository.Update(user));
    }

    public Result<User?> Delete(Guid userId)
    {
        var existingUser = _usersRepository.Get(userId);

        if (existingUser is null)
        {
            return Result<User?>.Failure("User not found");
        }
        
        _usersRepository.Delete(userId);

        if (_usersRepository.Get(userId) is null)
        {
            return Result<User?>.Success(null);
        }
        
        return Result<User?>.Failure("User delete failed");
    }
}