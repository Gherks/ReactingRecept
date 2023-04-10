namespace ReactingRecept.Application.DTOs;

public class DailyIntakeDTO
{
    public string Name { get; private set; } = string.Empty;
    public DailyIntakeEntityDTO[] DailyIntakeEntityDTOs { get; private set; } = Array.Empty<DailyIntakeEntityDTO>();

    public DailyIntakeDTO(string name, DailyIntakeEntityDTO[] dailyIntakeEntryDTOs)
    {
        Name = name;
        DailyIntakeEntityDTOs = dailyIntakeEntryDTOs;
    }
}
