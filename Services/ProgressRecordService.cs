using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        // ✅ Fetch all progress records
        public async Task<IEnumerable<ProgressRecordDTO>> GetAllProgressRecordsAsync()
        {
            _logger.LogInformation("Fetching all progress records...");

            var progressRecords = await _context.ProgressRecords.AsNoTracking().ToListAsync();

            if (!progressRecords.Any())
            {
                _logger.LogWarning("No progress records found in the system.");
            }

            return progressRecords.Select(pr => new ProgressRecordDTO
            {
                ProgressRecordId = pr.ProgressRecordId,
                UserId = pr.UserId,
                ProgressDateTime = pr.ProgressDateTime,
                DistanceCovered = pr.DistanceCovered,
                TimeTaken = pr.TimeTaken
            }).ToList();
        }

        // ✅ Fetch progress record by ID (Fixed possible null reference return)
        public async Task<ProgressRecordDTO?> GetProgressRecordByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching progress record with ID {id}");

            var progressRecord = await _context.ProgressRecords.AsNoTracking().FirstOrDefaultAsync(pr => pr.ProgressRecordId == id);

            if (progressRecord == null)
            {
                _logger.LogWarning($"Progress record with ID {id} not found.");
                return null;
            }

            return new ProgressRecordDTO
            {
                ProgressRecordId = progressRecord.ProgressRecordId,
                UserId = progressRecord.UserId,
                ProgressDateTime = progressRecord.ProgressDateTime,
                DistanceCovered = progressRecord.DistanceCovered,
                TimeTaken = progressRecord.TimeTaken
            };
        }

        // ✅ Add new progress record
        public async Task<ProgressRecordDTO?> AddProgressRecordAsync(ProgressRecordDTO progressRecordDto)
        {
            _logger.LogInformation("Adding a new progress record...");

            // ✅ Validate User existence before proceeding
            var userExists = await _context.Users.AnyAsync(u => u.UserId == progressRecordDto.UserId);
            if (!userExists)
            {
                _logger.LogWarning($"User with ID {progressRecordDto.UserId} not found.");
                return null;
            }

            var progressRecord = new ProgressRecord
            {
                UserId = progressRecordDto.UserId,
                ProgressDateTime = progressRecordDto.ProgressDateTime,
                DistanceCovered = progressRecordDto.DistanceCovered,
                TimeTaken = progressRecordDto.TimeTaken
            };

            try
            {
                _context.ProgressRecords.Add(progressRecord);
                await _context.SaveChangesAsync();

                return new ProgressRecordDTO
                {
                    ProgressRecordId = progressRecord.ProgressRecordId,
                    UserId = progressRecord.UserId,
                    ProgressDateTime = progressRecord.ProgressDateTime,
                    DistanceCovered = progressRecord.DistanceCovered,
                    TimeTaken = progressRecord.TimeTaken
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding progress record: {ex.Message}");
                throw;
            }
        }

        // ✅ Update progress record
        public async Task<bool> UpdateProgressRecordAsync(int id, ProgressRecordDTO progressRecordDto)
        {
            _logger.LogInformation($"Updating progress record with ID {id}");

            var existingRecord = await _context.ProgressRecords.FindAsync(id);

            if (existingRecord == null)
            {
                _logger.LogWarning($"Progress record with ID {id} not found.");
                return false;
            }

            existingRecord.ProgressDateTime = progressRecordDto.ProgressDateTime;
            existingRecord.DistanceCovered = progressRecordDto.DistanceCovered;
            existingRecord.TimeTaken = progressRecordDto.TimeTaken;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating progress record: {ex.Message}");
                return false;
            }
        }

        // ✅ Delete progress record
        public async Task<bool> DeleteProgressRecordAsync(int id)
        {
            _logger.LogInformation($"Attempting to delete progress record with ID {id}");

            var progressRecord = await _context.ProgressRecords.FindAsync(id);

            if (progressRecord == null)
            {
                _logger.LogWarning($"Progress record with ID {id} not found.");
                return false;
            }

            _context.ProgressRecords.Remove(progressRecord);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting progress record: {ex.Message}");
                return false;
            }
        }
    }
}

