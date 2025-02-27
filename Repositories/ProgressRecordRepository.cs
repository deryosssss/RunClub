using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.DTOs;

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
