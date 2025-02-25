using RunClubAPI.Interfaces;
using RunClubAPI.Models;

namespace RunClubAPI.Repositories
{
    public class ProgressRecordRepository : IProgressRecordRepository
    {
        private readonly RunClubContext _context;

        public ProgressRecordRepository(RunClubContext context)
        {
            _context = context;
        }

        public async Task<List<ProgressRecord>> GetAllProgressRecordsAsync()
        {
            return await _context.ProgressRecords.ToListAsync();
        }
    }
}
