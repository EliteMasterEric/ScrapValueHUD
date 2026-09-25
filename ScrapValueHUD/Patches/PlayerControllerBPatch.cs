using GameNetcodeStuff;
using HarmonyLib;

namespace ScrapValueHUD.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class PlayerControllerBPatch
    {
        [HarmonyPatch("SwitchToItemSlot")]
        [HarmonyPostfix]
        private static void SwitchToItemSlotPostfix()
        {
            // Refresh labels on pickup/hotbar switch.
            ScrapValueHUDUI.RefreshLabels();
        }

        [HarmonyPatch("DespawnHeldObjectOnClient")]
        [HarmonyPostfix]
        private static void DespawnHeldObjectOnClientPostfix()
        {
            // Refresh labels on throw/despawn.
            ScrapValueHUDUI.RefreshLabels();
        }

        [HarmonyPatch("SetObjectAsNoLongerHeld")]
        [HarmonyPostfix]
        private static void SetObjectAsNoLongerHeldPostfix()
        {
            // Refresh labels on object dropped.
            ScrapValueHUDUI.RefreshLabels();
        }

        [HarmonyPatch("PlaceObjectServerRpc")]
        [HarmonyPostfix]
        private static void PlaceObjectServerRpcPostfix()
        {
            // Refresh labels on object placed.
            ScrapValueHUDUI.RefreshLabels();
        }

        [HarmonyPatch("DropAllHeldItems")]
        [HarmonyPostfix]
        private static void DropAllHeldItemsPostfix()
        {
            // Refresh labels on player killed/disconnected.
            ScrapValueHUDUI.RefreshLabels();
        }
    }
}
