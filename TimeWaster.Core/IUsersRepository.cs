using TimeWaster.Core.Models;

namespace TimeWaster.Core;

public interface IUsersRepository
{
    IEnumerable<User> GetAll();
    User? Get(Guid id);
    User? GetByLogin(string login);
    User? GetWithIntervals(Guid id);
    User? Create(User user);
    User? Update(User user);
    void Delete(Guid id);
}