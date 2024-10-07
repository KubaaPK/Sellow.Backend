using Microsoft.EntityFrameworkCore;
using Sellow.Modules.Auth.Core.Domain;

namespace Sellow.Modules.Auth.Core.DAL.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserUnique(User user, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Username == user.Username || x.Email == user.Email, cancellationToken) is null;
    }

    public async Task Save(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(User user, CancellationToken cancellationToken = default)
    {
        _context.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}