using ReactingRecept.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ReactingRecept.Contract;

public static class Contracts
{
    public static void LogAndThrowWhenNull([NotNull] object? value, string errorMessage)
    {
        if (value is null)
        {
            Log.Error(errorMessage);
            throw new NullReferenceException(errorMessage);
        }
    }
}