using Microsoft.EntityFrameworkCore;
using TimeWaster.Core;
using TimeWaster.Core.Models;

namespace TimeWaster.Data.Users.Repositories;

public class UsersInMemoryRepository : IUsersRepository
{
    private readonly TimeWasterDbContext _context;

    public UsersInMemoryRepository(TimeWasterDbContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users
            .AsNoTracking()
            .Select(user => new User(user.Id, user.Login, user.Name, null))
            .AsEnumerable();
    }

    public User? Get(Guid id)
    {
        var user = _context.Users
            .AsNoTracking()
            .FirstOrDefault(user => user.Id == id);

        return CreateUserFromDbModel(user);
    }

    public User? GetByLogin(string login)
    {
        var user = _context.Users
            .AsNoTracking()
            .FirstOrDefault(user => user.Login == login);

        return CreateUserFromDbModel(user);
    }

    public User? GetWithIntervals(Guid id)
    {
        var user = _context.Users
            .AsNoTracking()
            .Include(user => user.Intervals)
            .FirstOrDefault(user => user.Id == id);

        return user is null
            ? null
            : new User(user.Id,
                user.Login,
                user.Name,
                user.Intervals?
                    .Select(interval =>
                        Interval.Create(user.Id, interval.StartTime, interval.Name, interval.EndTime)));
    }

    public User? Create(User user)
    {
        var userDbModel = new UserDbModel()
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
        };

        _context.Users.Add(userDbModel);
        _context.SaveChanges();

        return Get(user.Id);
    }

    public User? Update(User user)
    {
        var userToUpdate = _context.Users
                                   .FirstOrDefault(userDbModel => userDbModel.Id == user.Id)
                               ?? throw new ArgumentException($"User {user.Id} not found");

        userToUpdate.Login = user.Login;
        userToUpdate.Name = user.Name;
        
        _context.SaveChanges();
        return Get(userToUpdate.Id);
    }

    public void Delete(Guid id)
    {
        var userToDelete = _context.Users
                               .AsNoTracking()
                               .FirstOrDefault(user => user.Id == id)
                           ?? throw new ArgumentException($"User {id} does not exist");

        _context.Users.Remove(userToDelete);

        _context.SaveChanges();
    }

    private User? CreateUserFromDbModel(UserDbModel? user)
    {
        return user is null
            ? null
            : new User(user.Id, user.Login, user.Name, null);
    }
}