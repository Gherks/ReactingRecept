using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ReactingRecept.Shared;

public static class Contracts
{
    public static void LogAndThrowWhenNotInjected(
        [NotNull] object? culprit,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (culprit is null)
        {
            string message = $"Failed while calling '{memberName}' method in {GetFileName(sourceFilePath)}({sourceLineNumber}) " +
                $"because object has not been injected.";

            Log.Error(message);
            throw new InvalidOperationException(message);
        }
    }

    public static void LogAndThrowWhenNotSet(
        [NotNull] object? culprit,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (culprit is null)
        {
            string message = $"Failed while calling '{memberName}' method in {GetFileName(sourceFilePath)}({sourceLineNumber}) " +
                $"because object has not been set.";

            Log.Error(message);
            throw new InvalidOperationException(message);
        }
    }

    public static void LogAndThrowWhenNothingWasReceived(
        [NotNull] object? culprit,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (culprit is null)
        {
            string message = $"Failed while calling '{memberName}' method in {GetFileName(sourceFilePath)}({sourceLineNumber}) " +
                $"because object did not receive any data.";

            Log.Error(message);
            throw new InvalidOperationException(message);
        }
    }

    private static string GetFileName(string filePath)
    {
        int sourceFileStartIndex = filePath.LastIndexOf('\\') + 1;
        return filePath[sourceFileStartIndex..];
    }
}
