using ReactingRecept.Application.Commands;
using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Mocking;

public static partial class Mocker
{
    public static DailyIntake MockDailyIntake(string name, AddDailyIntakeEntryCommand[] addDailyIntakeEntryCommands)
    {
        DailyIntake dailyIntake = new(name);

        foreach (AddDailyIntakeEntryCommand addDailyIntakeEntryCommand in addDailyIntakeEntryCommands)
        {
            dailyIntake.Entities.Add(new DailyIntakeEntity(addDailyIntakeEntryCommand.EntityId, addDailyIntakeEntryCommand.Amount, addDailyIntakeEntryCommand.SortOrder));
        }

        return dailyIntake;
    }
}
