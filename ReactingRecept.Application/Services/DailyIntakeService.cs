using ReactingRecept.Application.DTOs;
using ReactingRecept.Application.Interfaces.Services;

namespace ReactingRecept.Application.Services
{
    public class DailyIntakeService : IDailyIntakeService
    {
        public Task<DailyIntakeDTO?> AddAsync(DailyIntakeDTO dailyIntakeDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(DailyIntakeDTO dailyIntakeDTO)
        {
            throw new NotImplementedException();
        }

        public Task<DailyIntakeDTO[]?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DailyIntakeDTO?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DailyIntakeDTO?> GetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<DailyIntakeDTO?> UpdateAsync(DailyIntakeDTO dailyIntakeDTO)
        {
            throw new NotImplementedException();
        }
    }
}
