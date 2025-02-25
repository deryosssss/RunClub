using RunClub.DTOs;
using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
    public interface IProgressRecordService
    {
        Task<IEnumerable<ProgressRecordDTO>> GetAllProgressRecordsAsync();
        Task<ProgressRecordDTO> GetProgressRecordByIdAsync(int id);
        Task<ProgressRecordDTO> AddProgressRecordAsync(ProgressRecordDTO progressRecordDto);
        Task<bool> DeleteProgressRecordAsync(int id);
        Task<bool> UpdateProgressRecordAsync(int id, ProgressRecordDTO progressRecordDto);
    }
}
