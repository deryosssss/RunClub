using RunClubAPI.Models;  // For the ProgressRecord model.
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Interfaces
{
    public interface IProgressRecordRepository
    {
        Task<List<ProgressRecord>> GetAllProgressRecordsAsync();
    }
}
