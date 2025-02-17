using TimeWaster.Core.Models;

namespace TimeWaster.Core.Services.UserProcessing;

public interface IUserService
{
    public Result<IEnumerable<User>> GetAll();
    public Result<User?> Get(Guid id);
    public Result<User?> GetByLogin(string login);
    public Result<User?> Create(User user);
    public Result<User?> Update(User user);
    public Result<User?> Delete(Guid id);
}