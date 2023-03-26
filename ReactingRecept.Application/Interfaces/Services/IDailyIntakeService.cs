using ReactingRecept.Domain;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Application.Interfaces.Services
{
    public interface IDailyIntakeService
    {
        Task<bool> AnyAsync(Guid id);
        Task<IDailyIntakeDto?> GetByIdAsync(Guid id);
        Task<IDailyIntakeDto[]?> GetAllAsync();
        Task<IDailyIntakeDto?> AddAsync(IDailyIntakeDto dailyIntake);
        Task<IDailyIntakeDto[]?> AddManyAsync(IDailyIntakeDto[] dailyIntakes);
        Task<IDailyIntakeDto?> UpdateAsync(IDailyIntakeDto dailyIntake);
        Task<IDailyIntakeDto[]?> UpdateManyAsync(IDailyIntakeDto[] dailyIntakes);
        Task<bool> DeleteAsync(IDailyIntakeDto dailyIntake);
        Task<bool> DeleteManyAsync(IDailyIntakeDto[] dailyIntakes);
    }
}
