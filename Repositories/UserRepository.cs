using RunClubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RunClub.Repositories
{
    public class UserRepository
    {
        private readonly RunClubContext _context;

        public UserRepository(RunClubContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();
        public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);
        public async Task AddAsync(User user) { _context.Users.Add(user); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(User user) { _context.Entry(user).State = EntityState.Modified; await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null) { _context.Users.Remove(user); await _context.SaveChangesAsync(); }
        }
    }
}
