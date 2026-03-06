namespace Didww.Api3.Converter;

public static class DirtySerializationContext
{
    private static readonly AsyncLocal<bool> DirtyOnly = new();

    public static void EnableDirtyOnlyMode() => DirtyOnly.Value = true;

    public static void DisableDirtyOnlyMode() => DirtyOnly.Value = false;

    public static bool IsDirtyOnlyModeEnabled => DirtyOnly.Value;
}
