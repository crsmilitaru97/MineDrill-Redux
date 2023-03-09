using System;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    public Color disabledColor;
    public Color highlightColor;
    public Color selectedColor;
    public Color recipeOKColor;
    public Color recipeNOColor;
    public Slot selectedSlot;

    public static SlotsManager Manager;

    public enum SlotType : short
    {
        isViewOnly = 0,
        isSpecial = 1,
        isBuy = 2,
        isSell = 3,
        isSelected = 4,
        isCraft = 5,
        isInfo = 6,
        isRecipe = 7,
        isCargo = 8,
        isDuplicate = 9,
        isChest = 10
    };

    private void Awake()
    {
        Manager = GetComponent<SlotsManager>();
    }

    public delegate void SlotSelectedEventHandler(object source, EventArgs args);

    public event SlotSelectedEventHandler SlotSelected;

    public void SlotSelect()
    {
        OnSlotSelected();
    }

    protected virtual void OnSlotSelected()
    {
        if (SlotSelected != null)
            SlotSelected(this, null);
    }
}
