namespace ReactingRecept.Application.DTOs;

public class DailyIntakeDTO
{
    public string Name { get; private set; } = string.Empty;
    public DailyIntakeEntryDTO[] DailyIntakeEntryDTOs { get; private set; } = Array.Empty<DailyIntakeEntryDTO>();

    public DailyIntakeDTO(string name, DailyIntakeEntryDTO[] dailyIntakeEntryDTOs)
    {
        Name = name;
        DailyIntakeEntryDTOs = dailyIntakeEntryDTOs;
    }
}
