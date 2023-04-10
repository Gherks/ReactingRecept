using ReactingRecept.Application.Commands;
using ReactingRecept.Application.DTOs;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IDailyIntakeService
    {
        Task<bool> AnyAsync(Guid id);
        Task<bool> AnyAsync(string name);
        Task<DailyIntakeDTO?> GetAsync(Guid id);
        Task<DailyIntakeDTO?> GetAsync(string name);
        Task<DailyIntakeDTO[]?> GetAllAsync();
        Task<DailyIntakeDTO?> AddAsync(string name, AddDailyIntakeEntityCommand[] addDailyIntakeEntityCommands);
        Task<DailyIntakeDTO?> UpdateAsync(UpdateDailyIntakeEntryCommand[] updateDailyIntakeEntryCommands);
        Task<bool> DeleteAsync(DailyIntakeDTO dailyIntakeDTO);
    }
}
