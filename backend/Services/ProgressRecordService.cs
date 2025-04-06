using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RunClubAPI.Services
{
    public class ProgressRecordService : IProgressRecordService
    {
        private readonly RunClubContext _context;
        private readonly ILogger<ProgressRecordService> _logger;

        public ProgressRecordService(RunClubContext context, ILogger<ProgressRecordService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ProgressRecordDTO>> GetAllProgressRecordsAsync()
        {
            var records = await _context.ProgressRecords.AsNoTracking().ToListAsync();

            return records.Select(pr => new ProgressRecordDTO
            {
                ProgressRecordId = pr.ProgressRecordId,
                UserId = pr.UserId,
                ProgressDate = pr.ProgressDate.ToString("yyyy-MM-dd"),
                ProgressTime = pr.ProgressTime.ToString("HH:mm:ss"),
                DistanceCovered = pr.DistanceCovered,
                TimeTaken = pr.TimeTaken.ToString("c")
            });
        }

        public async Task<ProgressRecordDTO?> GetProgressRecordByIdAsync(int id)
        {
            var pr = await _context.ProgressRecords.AsNoTracking().FirstOrDefaultAsync(p => p.ProgressRecordId == id);
            return pr == null ? null : ToDto(pr);
        }

        public async Task<ProgressRecordDTO?> AddProgressRecordAsync(ProgressRecordDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || !await _context.Users.AnyAsync(u => u.Id == dto.UserId))
            {
                _logger.LogWarning("❌ Invalid or missing UserId: {UserId}", dto.UserId);
                return null;
            }

            if (!TryParseDto(dto, out var entity))
            {
                _logger.LogWarning("❌ Failed to parse date/time for progress record.");
                return null;
            }

            _context.ProgressRecords.Add(entity);
            await _context.SaveChangesAsync();

            dto.ProgressRecordId = entity.ProgressRecordId;
            return dto;
        }

        public async Task<bool> UpdateProgressRecordAsync(int id, ProgressRecordDTO dto)
        {
            var pr = await _context.ProgressRecords.FindAsync(id);
            if (pr == null)
            {
                _logger.LogWarning("❌ Progress record with ID {Id} not found", id);
                return false;
            }

            if (!TryParseDto(dto, out var parsed))
            {
                _logger.LogWarning("❌ Failed to parse date/time during update.");
                return false;
            }

            pr.ProgressDate = parsed.ProgressDate;
            pr.ProgressTime = parsed.ProgressTime;
            pr.DistanceCovered = parsed.DistanceCovered;
            pr.TimeTaken = parsed.TimeTaken;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProgressRecordAsync(int id)
        {
            var pr = await _context.ProgressRecords.FindAsync(id);
            if (pr == null) return false;

            _context.ProgressRecords.Remove(pr);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ProgressRecordDTO ToDto(ProgressRecord pr) => new()
        {
            ProgressRecordId = pr.ProgressRecordId,
            UserId = pr.UserId,
            ProgressDate = pr.ProgressDate.ToString("yyyy-MM-dd"),
            ProgressTime = pr.ProgressTime.ToString("HH:mm:ss"),
            DistanceCovered = pr.DistanceCovered,
            TimeTaken = pr.TimeTaken.ToString("c")
        };

        private static bool TryParseDto(ProgressRecordDTO dto, out ProgressRecord entity)
        {
            entity = new ProgressRecord();

            if (!DateOnly.TryParse(dto.ProgressDate, out var date) ||
                !TimeOnly.TryParse(dto.ProgressTime, out var time) ||
                !TimeSpan.TryParse(dto.TimeTaken, out var timeTaken))
            {
                return false;
            }

            entity.UserId = dto.UserId;
            entity.ProgressDate = date;
            entity.ProgressTime = time;
            entity.DistanceCovered = dto.DistanceCovered;
            entity.TimeTaken = timeTaken;

            return true;
        }
    }
}




 /* The ProgressRecordService is a key component of the RunClubAPI, responsible for managing users' progress records. It implements IProgressRecordService and follows asynchronous programming with Entity Framework Core (EF Core) to optimize database interactions. It provides CRUD operations, ensuring that data integrity and validation are maintained before inserting or updating records. The use of logging (ILogger) allows efficient debugging and monitoring, while error handling prevents crashes and improves API stability. Additionally, data transformation between entities and DTOs ensures a clean API response structure. By implementing best practices like AsNoTracking(), validation before operations, and exception handling, this service ensures that RunClubAPI remains scalable, efficient, and maintainable for tracking users' progress effectively. */ 
