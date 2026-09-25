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

        const int LeftOffset = -4;
        const int BottomOffset = 0;

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
                slotLabels[i] = CreateLabel(hud.itemSlotIconFrames[i].transform, "ScrapValueSlot" + i);
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
            if (!ScrapValueHUDConfig.FixRotation.Value) return;

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
        internal static void RefreshLabels(bool updateDisplay = false)
        {
            if (instance != null) instance.Refresh(updateDisplay);
        }

        private static TextMeshProUGUI CreateLabel(Transform parent, string name)
        {
            GameObject labelObject = new GameObject(name, typeof(RectTransform));
            labelObject.transform.SetParent(parent, false);

            RectTransform rect = labelObject.GetComponent<RectTransform>();
            ApplyAnchor(rect);

            TextMeshProUGUI label = labelObject.AddComponent<TextMeshProUGUI>();
            ApplyLabelStyle(label);
            
            label.text = string.Empty;

            return label;
        }

        private static void ApplyLabelStyle(TextMeshProUGUI label)
        {
            HUDManager hud = HUDManager.Instance;
            if (hud == null || hud.itemSlotIconFrames == null)
            {
                return;
            }

            if (hud.weightCounter != null)
            {
                label.font = hud.weightCounter.font;
                label.fontSharedMaterial = hud.weightCounter.fontSharedMaterial;
            }
            else
            {
                label.font = TMP_Settings.defaultFontAsset;
            }
            label.fontSize = ScrapValueHUDConfig.ScrapValueFontSize.Value;
            label.alignment = GetAlignment();
            label.color = Color.white;
            label.enableWordWrapping = false;
            label.raycastTarget = false;
        }

        /// <summary>
        /// Offsets the label's rect so the anchored corner is inset by the padding.
        /// </summary>
        private static void ApplyAnchor(RectTransform rect)
        {
            var anchor = ScrapValueHUDConfig.ScrapValueAnchor.Value;

            bool right = anchor == LabelAnchor.TopRight || anchor == LabelAnchor.BottomRight;
            bool top = anchor == LabelAnchor.TopLeft || anchor == LabelAnchor.TopRight;

            float horizontal = ScrapValueHUDConfig.ScrapValueHorizontalPadding.Value;
            float vertical = ScrapValueHUDConfig.ScrapValueVerticalPadding.Value;

            // quick fix
            if (!right) horizontal += LeftOffset;
            if (!top) horizontal += BottomOffset;

            // The label stretches over the whole slot. The anchored corner is pulled
            // inwards by the padding, and the text is aligned to that corner.
            rect.anchorMin = new Vector2(0f, 0f); // lower left
            rect.anchorMax = new Vector2(1f, 1f); // upper right
            rect.offsetMin = new Vector2(right ? 0f : horizontal, top ? 0f : vertical);
            rect.offsetMax = new Vector2(right ? -horizontal : 0f, top ? -vertical : 0f);
        }

        private static TextAlignmentOptions GetAlignment()
        {
            switch (ScrapValueHUDConfig.ScrapValueAnchor.Value)
            {
                case LabelAnchor.TopLeft: return TextAlignmentOptions.TopLeft;
                case LabelAnchor.TopRight: return TextAlignmentOptions.TopRight;
                case LabelAnchor.BottomLeft: return TextAlignmentOptions.BottomLeft;
                default: return TextAlignmentOptions.BottomRight;
            }
        }

        private void Refresh(bool updateDisplay = false)
        {
            if (slotLabels == null) return;

            PlayerControllerB? player = GameNetworkManager.Instance?.localPlayerController;

            for (int i = 0; i < slotLabels.Length; i++)
            {
                if (updateDisplay)
                {
                    // Apply font size and other style changes.
                    ApplyLabelStyle(slotLabels[i]);

                    // Apply anchor and position changes.
                    RectTransform rect = slotLabels[i].GetComponent<RectTransform>();
                    ApplyAnchor(rect);
                }

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
