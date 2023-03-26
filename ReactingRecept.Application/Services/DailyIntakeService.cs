using ReactingRecept.Application.Interfaces.Services;
using ReactingRecept.Domain;

namespace ReactingRecept.Application.Services
{
    public class DailyIntakeService : IDailyIntakeService
    {
        public Task<IDailyIntakeDto?> AddAsync(IDailyIntakeDto dailyIntake)
        {
            throw new NotImplementedException();
        }

        public Task<IDailyIntakeDto[]?> AddManyAsync(IDailyIntakeDto[] dailyIntakes)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(IDailyIntakeDto dailyIntake)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteManyAsync(IDailyIntakeDto[] dailyIntakes)
        {
            throw new NotImplementedException();
        }

        public Task<IDailyIntakeDto[]?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDailyIntakeDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDailyIntakeDto?> UpdateAsync(IDailyIntakeDto dailyIntake)
        {
            throw new NotImplementedException();
        }

        public Task<IDailyIntakeDto[]?> UpdateManyAsync(IDailyIntakeDto[] dailyIntakes)
        {
            throw new NotImplementedException();
        }
    }
}
