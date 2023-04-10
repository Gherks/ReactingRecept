using ReactingRecept.Application.Commands;
using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Mocking;

public static partial class Mocker
{
    public static DailyIntake MockDailyIntake(string name, AddDailyIntakeEntityCommand[] addDailyIntakeEntityCommands)
    {
        DailyIntake dailyIntake = new(name);

        foreach (AddDailyIntakeEntityCommand addDailyIntakeEntryCommand in addDailyIntakeEntityCommands)
        {
            dailyIntake.Entities.Add(new DailyIntakeEntity(addDailyIntakeEntryCommand.EntityId, addDailyIntakeEntryCommand.Amount, addDailyIntakeEntryCommand.SortOrder));
        }

        return dailyIntake;
    }
}
