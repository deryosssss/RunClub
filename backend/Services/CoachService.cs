using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using RunClubAPI.Data;

namespace RunClubAPI.Services
{
    public class CoachService : ICoachService
    {
        private readonly RunClubContext _context;

        public CoachService(RunClubContext context)
        {
            _context = context;
        }

        public IEnumerable<CoachDto> GetAllCoaches()
        {
            return _context.Coaches.Select(c => new CoachDto
            {
                Id = c.Id,
                Name = c.Name,
                PhotoUrl = c.PhotoUrl,
                Bio = c.Bio,
                Rating = c.Rating
            }).ToList();
        }

        public CoachDto? GetCoachById(int id)
        {
            var coach = _context.Coaches.Find(id);
            if (coach == null) return null;

            return new CoachDto
            {
                Id = coach.Id,
                Name = coach.Name,
                PhotoUrl = coach.PhotoUrl,
                Bio = coach.Bio,
                Rating = coach.Rating
            };
        }

        public CoachDto CreateCoach(CoachDto dto)
        {
            var coach = new Coach
            {
                Name = dto.Name,
                PhotoUrl = dto.PhotoUrl,
                Bio = dto.Bio,
                Rating = dto.Rating
            };

            _context.Coaches.Add(coach);
            _context.SaveChanges();

            dto.Id = coach.Id;
            return dto;
        }

        public bool UpdateCoach(int id, CoachDto dto)
        {
            var coach = _context.Coaches.Find(id);
            if (coach == null) return false;

            coach.Name = dto.Name;
            coach.PhotoUrl = dto.PhotoUrl;
            coach.Bio = dto.Bio;
            coach.Rating = dto.Rating;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteCoach(int id)
        {
            var coach = _context.Coaches.Find(id);
            if (coach == null) return false;

            _context.Coaches.Remove(coach);
            _context.SaveChanges();
            return true;
        }
    }
}
