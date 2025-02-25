using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClub.Services
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
            var result = new List<ProgressRecordDTO>();

            foreach (var pr in progressRecords)
            {
                result.Add(new ProgressRecordDTO
                {
                    ProgressRecordId = pr.ProgressRecordId,
                    UserId = pr.UserId,
                    ProgressDateTime = pr.ProgressDateTime,
                    DistanceCovered = pr.DistanceCovered,
                    TimeTaken = pr.TimeTaken
                });
            }

            if (result.Count == 0)
            {
                _logger.LogWarning("No progress records found in the system.");
            }

            return result;
        }

        // ✅ Fetch progress record by ID
        public async Task<ProgressRecordDTO> GetProgressRecordByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching progress record with ID {id}");

            var progressRecords = await _context.ProgressRecords.AsNoTracking().ToListAsync();
            ProgressRecord progressRecord = null;

            foreach (var pr in progressRecords)
            {
                if (pr.ProgressRecordId == id)
                {
                    progressRecord = pr;
                    break;
                }
            }

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
        public async Task<ProgressRecordDTO> AddProgressRecordAsync(ProgressRecordDTO progressRecordDto)
        {
            _logger.LogInformation("Adding a new progress record...");

            // ✅ Validate User existence before proceeding
            var users = await _context.Users.ToListAsync();
            bool userExists = false;

            foreach (var user in users)
            {
                if (user.UserId == progressRecordDto.UserId)
                {
                    userExists = true;
                    break;
                }
            }

            if (!userExists)
            {
                _logger.LogWarning($"User with ID {progressRecordDto.UserId} not found.");
                return null; // Handle in controller
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

                progressRecordDto.ProgressRecordId = progressRecord.ProgressRecordId;
                return progressRecordDto;
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

            var progressRecords = await _context.ProgressRecords.ToListAsync();
            ProgressRecord existingRecord = null;

            foreach (var pr in progressRecords)
            {
                if (pr.ProgressRecordId == id)
                {
                    existingRecord = pr;
                    break;
                }
            }

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

            var progressRecords = await _context.ProgressRecords.ToListAsync();
            ProgressRecord progressRecord = null;

            foreach (var pr in progressRecords)
            {
                if (pr.ProgressRecordId == id)
                {
                    progressRecord = pr;
                    break;
                }
            }

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
