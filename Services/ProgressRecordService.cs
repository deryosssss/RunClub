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
        private readonly RunClubContext _context; // Dependency for database access
        private readonly ILogger<ProgressRecordService> _logger; // Logger for debugging and monitoring

        /// <summary>
        /// Constructor for ProgressRecordService.
        /// Injects RunClubContext for database interactions and ILogger for logging.
        /// </summary>
        public ProgressRecordService(RunClubContext context, ILogger<ProgressRecordService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all progress records from the database.
        /// Uses AsNoTracking() to improve performance by not tracking entities in EF Core.
        /// </summary>
        public async Task<IEnumerable<ProgressRecordDTO>> GetAllProgressRecordsAsync()
        {
            _logger.LogInformation("Fetching all progress records...");

            // Retrieve progress records from the database without tracking changes
            var progressRecords = await _context.ProgressRecords.AsNoTracking().ToListAsync();

            // Log a warning if no records are found
            if (!progressRecords.Any())
            {
                _logger.LogWarning("No progress records found in the system.");
            }

            // Convert ProgressRecord entities into DTOs before returning to ensure API encapsulation
            return progressRecords.Select(pr => new ProgressRecordDTO
            {
                ProgressRecordId = pr.ProgressRecordId,
                UserId = pr.UserId,
                ProgressDate = pr.ProgressDate.ToString("yyyy-MM-dd"),  // Store date in ISO format
                ProgressTime = pr.ProgressTime.ToString("HH:mm:ss"),    // Store time in 24-hour format
                DistanceCovered = pr.DistanceCovered,
                TimeTaken = pr.TimeTaken
            }).ToList();
        }

        /// <summary>
        /// Retrieves a specific progress record by its unique ID.
        /// </summary>
        public async Task<ProgressRecordDTO?> GetProgressRecordByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching progress record with ID {id}");

            // Fetch the record from the database, but do not track it for performance optimization
            var progressRecord = await _context.ProgressRecords.AsNoTracking()
                .FirstOrDefaultAsync(pr => pr.ProgressRecordId == id);

            // Log a warning if the record does not exist
            if (progressRecord == null)
            {
                _logger.LogWarning($"Progress record with ID {id} not found.");
                return null;
            }

            // Convert entity to DTO before returning
            return new ProgressRecordDTO
            {
                ProgressRecordId = progressRecord.ProgressRecordId,
                UserId = progressRecord.UserId,
                ProgressDate = progressRecord.ProgressDate.ToString("yyyy-MM-dd"),
                ProgressTime = progressRecord.ProgressTime.ToString("HH:mm:ss"),
                DistanceCovered = progressRecord.DistanceCovered,
                TimeTaken = progressRecord.TimeTaken
            };
        }

        /// <summary>
        /// Adds a new progress record to the database.
        /// Ensures that the associated User exists before adding the record.
        /// </summary>
        public async Task<ProgressRecordDTO?> AddProgressRecordAsync(ProgressRecordDTO progressRecordDto)
        {
            _logger.LogInformation("Adding a new progress record...");

            // Validate if the user exists before adding a progress record
            var userExists = await _context.Users.AnyAsync(u => u.UserId == progressRecordDto.UserId);
            if (!userExists)
            {
                _logger.LogWarning($"User with ID {progressRecordDto.UserId} not found.");
                return null;
            }

            // Convert string-based date/time into DateOnly and TimeOnly types
            if (!DateOnly.TryParse(progressRecordDto.ProgressDate, out var parsedDate) ||
                !TimeOnly.TryParse(progressRecordDto.ProgressTime, out var parsedTime))
            {
                _logger.LogWarning("Invalid date or time format provided.");
                return null;
            }

            // Create a new ProgressRecord entity with the converted values
            var progressRecord = new ProgressRecord
            {
                UserId = progressRecordDto.UserId,
                ProgressDate = parsedDate,
                ProgressTime = parsedTime,
                DistanceCovered = progressRecordDto.DistanceCovered,
                TimeTaken = progressRecordDto.TimeTaken
            };

            try
            {
                // Add the new record to the database
                _context.ProgressRecords.Add(progressRecord);
                await _context.SaveChangesAsync();

                // Convert the saved entity back to DTO and return it
                return new ProgressRecordDTO
                {
                    ProgressRecordId = progressRecord.ProgressRecordId,
                    UserId = progressRecord.UserId,
                    ProgressDate = progressRecord.ProgressDate.ToString("yyyy-MM-dd"),
                    ProgressTime = progressRecord.ProgressTime.ToString("HH:mm:ss"),
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

        /// <summary>
        /// Updates an existing progress record identified by ID.
        /// Converts string-based date and time to DateOnly and TimeOnly.
        /// </summary>
        public async Task<bool> UpdateProgressRecordAsync(int id, ProgressRecordDTO progressRecordDto)
        {
            _logger.LogInformation($"Updating progress record with ID {id}");

            // Retrieve the existing record
            var existingRecord = await _context.ProgressRecords.FindAsync(id);
            if (existingRecord == null)
            {
                _logger.LogWarning($"Progress record with ID {id} not found.");
                return false;
            }

            // Convert string date/time to DateOnly and TimeOnly
            if (!DateOnly.TryParse(progressRecordDto.ProgressDate, out var parsedDate) ||
                !TimeOnly.TryParse(progressRecordDto.ProgressTime, out var parsedTime))
            {
                _logger.LogWarning("Invalid date or time format provided.");
                return false;
            }

            // Update the existing record with new values
            existingRecord.ProgressDate = parsedDate;
            existingRecord.ProgressTime = parsedTime;
            existingRecord.DistanceCovered = progressRecordDto.DistanceCovered;
            existingRecord.TimeTaken = progressRecordDto.TimeTaken;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating progress record: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a progress record by its unique ID.
        /// Ensures that the record exists before attempting deletion.
        /// </summary>
        public async Task<bool> DeleteProgressRecordAsync(int id)
        {
            _logger.LogInformation($"Attempting to delete progress record with ID {id}");

            // Retrieve the record from the database
            var progressRecord = await _context.ProgressRecords.FindAsync(id);
            if (progressRecord == null)
            {
                _logger.LogWarning($"Progress record with ID {id} not found.");
                return false;
            }

            // Remove the record from the database
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
 /* The ProgressRecordService is a key component of the RunClubAPI, responsible for managing users' progress records. It implements IProgressRecordService and follows asynchronous programming with Entity Framework Core (EF Core) to optimize database interactions. It provides CRUD operations, ensuring that data integrity and validation are maintained before inserting or updating records. The use of logging (ILogger) allows efficient debugging and monitoring, while error handling prevents crashes and improves API stability. Additionally, data transformation between entities and DTOs ensures a clean API response structure. By implementing best practices like AsNoTracking(), validation before operations, and exception handling, this service ensures that RunClubAPI remains scalable, efficient, and maintainable for tracking users' progress effectively. */ 
