namespace MarleneCollectionXmlTool.Domain.Helpers;

public static class EnvironmentVariableHelpers
{
    public static T GetEnvironmentVariableOrDefault<T>(string variableName, T defaultValue = default)
    {
        var envVariableValue = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrWhiteSpace(envVariableValue))
            return defaultValue;

        return (T)Convert.ChangeType(envVariableValue, typeof(T));
    }
}
