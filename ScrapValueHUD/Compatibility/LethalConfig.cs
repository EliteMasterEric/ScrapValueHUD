using LethalConfig;
using LethalConfig.ConfigItems;

namespace ScrapValueHUD.Compatibility
{
    internal class LethalConfigCompat
    {
        internal static void Setup()
        {
            // Options that require a restart.
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ScrapValueHUDConfig.FixRotation, true));

            // Options that do NOT require a restart.
            LethalConfigManager.AddConfigItem(new IntSliderConfigItem(ScrapValueHUDConfig.ScrapValueFontSize, false));
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<LabelAnchor>(ScrapValueHUDConfig.ScrapValueAnchor, false));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(ScrapValueHUDConfig.ScrapValueVerticalPadding, false));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(ScrapValueHUDConfig.ScrapValueHorizontalPadding, false));
        }
    }
}