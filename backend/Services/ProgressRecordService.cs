using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                TimeTaken = pr.TimeTaken.ToString("c") // e.g., 00:15:00
            });
        }

        public async Task<ProgressRecordDTO?> GetProgressRecordByIdAsync(int id)
        {
            var pr = await _context.ProgressRecords.AsNoTracking().FirstOrDefaultAsync(p => p.ProgressRecordId == id);

            if (pr == null) return null;

            return new ProgressRecordDTO
            {
                ProgressRecordId = pr.ProgressRecordId,
                UserId = pr.UserId,
                ProgressDate = pr.ProgressDate.ToString("yyyy-MM-dd"),
                ProgressTime = pr.ProgressTime.ToString("HH:mm:ss"),
                DistanceCovered = pr.DistanceCovered,
                TimeTaken = pr.TimeTaken.ToString("c")
            };
        }

        public async Task<ProgressRecordDTO?> AddProgressRecordAsync(ProgressRecordDTO dto)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == dto.UserId))
            {
                _logger.LogWarning("User not found: {UserId}", dto.UserId);
                return null;
            }

            if (!DateOnly.TryParse(dto.ProgressDate, out var date) ||
                !TimeOnly.TryParse(dto.ProgressTime, out var time) ||
                !TimeSpan.TryParse(dto.TimeTaken, out var timeTaken))
            {
                _logger.LogWarning("Invalid date/time/timespan provided.");
                return null;
            }

            var entity = new ProgressRecord
            {
                UserId = dto.UserId,
                ProgressDate = date,
                ProgressTime = time,
                DistanceCovered = dto.DistanceCovered,
                TimeTaken = timeTaken
            };

            _context.ProgressRecords.Add(entity);
            await _context.SaveChangesAsync();

            dto.ProgressRecordId = entity.ProgressRecordId;
            return dto;
        }

        public async Task<bool> UpdateProgressRecordAsync(int id, ProgressRecordDTO dto)
        {
            var pr = await _context.ProgressRecords.FindAsync(id);
            if (pr == null) return false;

            if (!DateOnly.TryParse(dto.ProgressDate, out var date) ||
                !TimeOnly.TryParse(dto.ProgressTime, out var time) ||
                !TimeSpan.TryParse(dto.TimeTaken, out var timeTaken))
            {
                return false;
            }

            pr.ProgressDate = date;
            pr.ProgressTime = time;
            pr.DistanceCovered = dto.DistanceCovered;
            pr.TimeTaken = timeTaken;

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
    }
}



 /* The ProgressRecordService is a key component of the RunClubAPI, responsible for managing users' progress records. It implements IProgressRecordService and follows asynchronous programming with Entity Framework Core (EF Core) to optimize database interactions. It provides CRUD operations, ensuring that data integrity and validation are maintained before inserting or updating records. The use of logging (ILogger) allows efficient debugging and monitoring, while error handling prevents crashes and improves API stability. Additionally, data transformation between entities and DTOs ensures a clean API response structure. By implementing best practices like AsNoTracking(), validation before operations, and exception handling, this service ensures that RunClubAPI remains scalable, efficient, and maintainable for tracking users' progress effectively. */ 
