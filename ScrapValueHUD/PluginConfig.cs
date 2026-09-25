using BepInEx.Configuration;

namespace ScrapValueHUD
{
    /// <summary>
    /// Holds the user-configurable display settings for the scrap value labels.
    /// </summary>
    internal static class PluginConfig
    {
        internal static ConfigEntry<int> ValueFontSize = null!;
        internal static ConfigEntry<float> BottomPadding = null!;
        internal static ConfigEntry<float> RightPadding = null!;

        /// <summary>
        /// Binds the configuration entries to the plugin's config file.
        /// </summary>
        /// <param name="config">The plugin's configuration file.</param>
        internal static void Initialize(ConfigFile config)
        {
            ValueFontSize = config.Bind(
                "Scrap Value",
                "FontSize",
                8,
                new ConfigDescription(
                    "Font size of the scrap value label.",
                    new AcceptableValueRange<int>(1, 72)));

            BottomPadding = config.Bind(
                "Scrap Value",
                "BottomPadding",
                10f,
                new ConfigDescription(
                    "Distance from the bottom of the slot to the label.",
                    new AcceptableValueRange<float>(-64f, 64f)));

            RightPadding = config.Bind(
                "Scrap Value",
                "RightPadding",
                10f,
                new ConfigDescription(
                    "Distance from the right side of the slot to the label.",
                    new AcceptableValueRange<float>(-64f, 64f)));
        }
    }
}
