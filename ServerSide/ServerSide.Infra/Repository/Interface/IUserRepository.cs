using ServerSide.Domain.Models;

namespace ServerSide.Infra.Repository.Interface
{
    public interface IUserRepository
    {
        User GetUsersByName(string name, string pwd);
        IEnumerable<User> GetUsers();
    }
}
