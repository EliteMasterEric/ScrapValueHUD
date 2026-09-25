using HarmonyLib;

namespace ScrapValueHUD.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal static class HUDManagerPatch
    {
        /// <summary>
        /// Attaches the scrap value overlay to the active HUDManager once it is initialized.
        /// </summary>
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void AwakePostfix(HUDManager __instance)
        {
            if (HUDManager.Instance != __instance) return;

            if (__instance.GetComponent<ScrapValueHUDUI>() == null)
            {
                __instance.gameObject.AddComponent<ScrapValueHUDUI>();
            }
        }
    }
}
