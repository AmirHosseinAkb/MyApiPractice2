using Entities.User;

namespace Data.Contracts
{
    public interface IUserRepository:IRepository<User>
    {
        Task AddUser(User user,string password, CancellationToken cancellationToken);
        Task<User> GetUserByUserNameAndPass(string userName, string password, CancellationToken cancellationToken);
    }
}
