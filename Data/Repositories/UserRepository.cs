using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Entities.User;

namespace Data.Repositories
{
    public class UserRepository:BaseRepository<User>,IUserRepository
    {
        private readonly MyApiContext _context;
        public UserRepository(MyApiContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddUser(User user, string password, CancellationToken cancellationToken)
        {
            if (await IsExistAsync(u => u.UserName == user.UserName,cancellationToken))
                throw new BadRequestException("نام کاربری تکراری است");
            var passwordHash = SecurityHelper.HashPasswordSHA256(password);
            user.PasswordHash = passwordHash;
            await base.AddAsync(user, cancellationToken);
        }
    }
}
