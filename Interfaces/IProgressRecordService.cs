using RunClub.DTOs;
using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
    public interface IProgressRecordService
    {
        Task<IEnumerable<ProgressRecordDTO>> GetAllProgressRecordsAsync();
        Task<ProgressRecordDTO> GetProgressRecordByIdAsync(int id);
        Task AddProgressRecordAsync(ProgressRecordDTO progressRecord);
    }
}