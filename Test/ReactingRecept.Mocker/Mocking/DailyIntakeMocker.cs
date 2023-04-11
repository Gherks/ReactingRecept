using ReactingRecept.Application.Commands;
using ReactingRecept.Application.DTOs;
using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Mocking;

public static partial class Mocker
{
    public static DailyIntake MockDailyIntake(DailyIntake dailyIntake)
    {
        MockId(dailyIntake);

        foreach(DailyIntakeEntity dailyIntakeEntity in dailyIntake.Entities)
        {
            MockId(dailyIntakeEntity);
        }

        return dailyIntake;
    }
}
