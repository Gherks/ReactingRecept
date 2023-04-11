using ReactingRecept.Application.Commands;
using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IDailyIntakeService
    {
        Task<bool> AnyAsync(Guid id);
        Task<bool> AnyAsync(string name);
        Task<DailyIntakeDTO?> GetAsync(Guid id);
        Task<DailyIntakeDTO?> GetAsync(string name);
        Task<DailyIntakeDTO[]?> GetAllAsync();
        Task<DailyIntakeDTO?> AddAsync(DailyIntake dailyIntake);
        Task<DailyIntakeDTO?> UpdateAsync(DailyIntake dailyIntake);
        Task<bool> DeleteAsync(Guid id);
    }
}
