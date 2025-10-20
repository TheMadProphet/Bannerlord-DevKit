namespace DevKit.Configuration;

/// <summary>
/// DevKit configuration variables.
/// Configuration is automatically loaded from DevKitConfig.ini on startup.
/// Use DevKitConfigIO.Save() after changing values to persist changes.
/// </summary>
public static class DevKitConfig
{
    public static bool EnableDevelopmentMode = true;
    public static bool EnableUiAreaPatch = true;
}
