using RunClubAPI.DTOs;

namespace RunClubAPI.Interfaces
{
    public interface ICoachService
    {
        IEnumerable<CoachDto> GetAllCoaches();
        CoachDto GetCoachById(int id);
        CoachDto CreateCoach(CoachDto coachDto);
        bool UpdateCoach(int id, CoachDto coachDto);
        bool DeleteCoach(int id);
    }
}
