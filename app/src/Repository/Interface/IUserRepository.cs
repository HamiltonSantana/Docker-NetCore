using ServerSide.Models;

namespace ServerSide.Repository.Interface
{
    public interface IUserRepository
    {
        User GetUsersByName(string name, string pwd);
        IEnumerable<User> GetUsers();
    }
}
