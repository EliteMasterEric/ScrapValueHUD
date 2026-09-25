using System;
using GameNetcodeStuff;
using TMPro;
using UnityEngine;

namespace ScrapValueHUD
{
    /// <summary>
    /// Creates and maintains a value label on top of each inventory slot,
    /// displaying the scrap value of any scrap item stored in that slot.
    /// Labels are refreshed whenever an inventory slot changes rather than every frame,
    /// since an item's scrap value is fixed once it has spawned.
    /// </summary>
    internal class ScrapValueHUDUI : MonoBehaviour
    {
        const int NoValue = int.MinValue;

        // Singleton instance.
        private static ScrapValueHUDUI? instance;

        // The labels attached to each slot.
        private TextMeshProUGUI[] slotLabels = null!;

        private void Awake()
        {
            HUDManager hud = HUDManager.Instance;
            if (hud == null || hud.itemSlotIconFrames == null)
            {
                enabled = false;
                return;
            }

            ResetItemSlotIconFrames(hud);

            slotLabels = new TextMeshProUGUI[hud.itemSlotIconFrames.Length];
            for (int i = 0; i < slotLabels.Length; i++)
            {
                slotLabels[i] = CreateLabel(hud.itemSlotIconFrames[i].transform, hud, "ScrapValueSlot" + i);
            }

            instance = this;
            Refresh();
        }

        private void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        /// <summary>
        /// Slot icon frames have an unwanted baked-in rotation; cancel it out.
        /// </summary>
        private static void ResetItemSlotIconFrames(HUDManager hud)
        {
            if (!PluginConfig.FixRotation.Value) return;

            for (int i = 0; i < hud.itemSlotIconFrames.Length; i++)
            {
                hud.itemSlotIconFrames[i].transform.rotation = Quaternion.identity;
            }
            for (int i = 0; i < hud.itemSlotIcons.Length; i++)
            {
                hud.itemSlotIcons[i].transform.rotation = Quaternion.identity;
            }
        }

        /// <summary>
        /// Refreshes the slot labels belonging to the local player, if the overlay exists.
        /// </summary>
        internal static void RefreshLabels()
        {
            if (instance != null) instance.Refresh();
        }

        private static TextMeshProUGUI CreateLabel(Transform parent, HUDManager hud, string name)
        {
            GameObject labelObject = new GameObject(name, typeof(RectTransform));
            labelObject.transform.SetParent(parent, false);

            RectTransform rect = labelObject.GetComponent<RectTransform>();

            // The slot is not rotated.
            // The padding is applied to the minimum and maximum corners of the frame.
            rect.anchorMin = new Vector2(0f, 0f); // lower left
            rect.anchorMax = new Vector2(1f, 1f); // upper right
            rect.offsetMin = new Vector2(0f, PluginConfig.BottomPadding.Value);
            rect.offsetMax = new Vector2(-PluginConfig.RightPadding.Value, 0f);

            TextMeshProUGUI label = labelObject.AddComponent<TextMeshProUGUI>();
            if (hud.weightCounter != null)
            {
                label.font = hud.weightCounter.font;
                label.fontSharedMaterial = hud.weightCounter.fontSharedMaterial;
            }
            else
            {
                label.font = TMP_Settings.defaultFontAsset;
            }
            label.fontSize = PluginConfig.ValueFontSize.Value;
            label.alignment = TextAlignmentOptions.BottomRight;
            label.color = Color.white;
            label.enableWordWrapping = false;
            label.raycastTarget = false;
            label.text = string.Empty;

            return label;
        }

        private void Refresh()
        {
            if (slotLabels == null) return;

            PlayerControllerB? player = GameNetworkManager.Instance?.localPlayerController;

            for (int i = 0; i < slotLabels.Length; i++)
            {
                GrabbableObject? item = (player != null && i < player.ItemSlots.Length) ? player.ItemSlots[i] : null;
                UpdateLabel(slotLabels[i], item);
            }
        }

        private static void UpdateLabel(TextMeshProUGUI? label, GrabbableObject? item)
        {
            if (label == null) return;

            int value = (item != null && item.itemProperties != null && item.itemProperties.isScrap)
                ? item.scrapValue
                : NoValue;

            label.text = (value == NoValue) ? string.Empty : "$" + value;
        }
    }
}
