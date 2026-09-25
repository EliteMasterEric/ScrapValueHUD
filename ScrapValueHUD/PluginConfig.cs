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
        internal static ConfigEntry<bool> FixRotation = null!;

        /// <summary>
        /// Binds the configuration entries to the plugin's config file.
        /// </summary>
        /// <param name="config">The plugin's configuration file.</param>
        internal static void Initialize(ConfigFile config)
        {
            ValueFontSize = config.Bind(
                "Scrap Value",
                "Font Size",
                8,
                new ConfigDescription(
                    "Font size of the scrap value label.",
                    new AcceptableValueRange<int>(1, 72)));

            BottomPadding = config.Bind(
                "Scrap Value",
                "Bottom Padding",
                2f,
                new ConfigDescription(
                    "Distance from the bottom of the slot to the label.",
                    new AcceptableValueRange<float>(-64f, 64f)));

            RightPadding = config.Bind(
                "Scrap Value",
                "Right Padding",
                4f,
                new ConfigDescription(
                    "Distance from the right side of the slot to the label.",
                    new AcceptableValueRange<float>(-64f, 64f)));

            FixRotation = config.Bind(
                "Misc",
                "Fix Rotation",
                true,
                "Correct the rotation of the slot icon frames.");

        }
    }
}
