using BepInEx.Configuration;

namespace ScrapValueHUD
{
    /// <summary>
    /// The corner of an inventory slot at which the scrap value label is anchored.
    /// </summary>
    internal enum LabelAnchor
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    /// <summary>
    /// Holds the user-configurable display settings for the scrap value labels.
    /// </summary>
    internal static class ScrapValueHUDConfig
    {
        internal static ConfigEntry<int> ScrapValueFontSize = null!;
        internal static ConfigEntry<LabelAnchor> ScrapValueAnchor = null!;
        internal static ConfigEntry<float> ScrapValueVerticalPadding = null!;
        internal static ConfigEntry<float> ScrapValueHorizontalPadding = null!;
        internal static ConfigEntry<bool> FixRotation = null!;

        /// <summary>
        /// Binds the configuration entries to the plugin's config file.
        /// </summary>
        /// <param name="config">The plugin's configuration file.</param>
        internal static void Initialize(ConfigFile config)
        {
            // NOTE: When adding new ConfigEntry fields, make sure to add them to Compatibility.LethalConfig as well.

            ScrapValueFontSize = config.Bind(
                "Scrap Value",
                "Font Size",
                8,
                new ConfigDescription(
                    "Font size of the scrap value label.",
                    new AcceptableValueRange<int>(1, 72)));
            ScrapValueFontSize.SettingChanged += (_, _) => ScrapValueHUDUI.RefreshLabels(true);

            ScrapValueAnchor = config.Bind(
                "Scrap Value",
                "Anchor",
                LabelAnchor.BottomRight,
                "The corner of the slot at which the label is anchored.");
            ScrapValueAnchor.SettingChanged += (_, _) => ScrapValueHUDUI.RefreshLabels(true);

            ScrapValueVerticalPadding = config.Bind(
                "Scrap Value",
                "Vertical Padding",
                2f,
                new ConfigDescription(
                    "Distance from the top or bottom of the slot to the label, depending on the anchor.",
                    new AcceptableValueRange<float>(-64f, 64f)));
            ScrapValueVerticalPadding.SettingChanged += (_, _) => ScrapValueHUDUI.RefreshLabels(true);

            ScrapValueHorizontalPadding = config.Bind(
                "Scrap Value",
                "Horizontal Padding",
                4f,
                new ConfigDescription(
                    "Distance from the left or right side of the slot to the label, depending on the anchor.",
                    new AcceptableValueRange<float>(-64f, 64f)));
            ScrapValueHorizontalPadding.SettingChanged += (_, _) => ScrapValueHUDUI.RefreshLabels(true);

            FixRotation = config.Bind(
                "Misc",
                "Fix Rotation",
                true,
                "Correct the rotation of the slot icon frames.");
            // FixRotation CANNOT be changed without a restart.

        }
    }
}
