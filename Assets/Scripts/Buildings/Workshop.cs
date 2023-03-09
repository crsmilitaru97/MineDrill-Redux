using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class Workshop : MonoBehaviour
{
    [Header("Crafting")]
    public Slot[] craftSlots;
    public Slot[] recipeSlots;
    public Button craftButton;
    public Image craftFill;
    public Slot craftFinalSlot;
    public GameObject craftMask;

    [Header("Duplicate")]
    public Slot[] duplicateSlots;
    public Slot duplicateFinalSlot;
    public BuyGroup duplicateButton;
    public Image duplicateFill;
    public GameObject duplicateMask;
    public RectTransform duplicateScroll;

    public static Workshop Instance;
    private void Awake() => Instance = this;

    void SlotSelected(object source, EventArgs eventArgs)
    {
        if (SlotsManager.Manager.selectedSlot.isCraft && SlotsManager.Manager.selectedSlot.isItem)
        { SelectItemSlotToCraft(SlotsManager.Manager.selectedSlot); }
        if (SlotsManager.Manager.selectedSlot.slotType == SlotsManager.SlotType.isDuplicate)
        { SelectItemSlotToDuplicate(SlotsManager.Manager.selectedSlot); }
    }

    private void Start()
    {
        SlotsManager.Manager.SlotSelected += SlotSelected;

        int si = 0;
        for (int i = 0; i < ItemsManager.Manager.Items.Count; i++)
        {
            var item = ItemsManager.Manager.Items[i].Cloned();
            if (!string.IsNullOrEmpty(item.craft1.CD))
            {
                craftSlots[si].item = item;
                craftSlots[si].item.price = ItemsManager.GetItemFromCD(item.craft1.CD)?.price ?? 0 + ItemsManager.GetItemFromCD(item.craft2.CD)?.price ?? 0 + 1;
                si++;
            }
        }
    }

    #region Duplicate
    public void ShowDuplicableItems()
    {
        foreach (var slot in duplicateSlots)
        {
            slot.ClearAndDisable();
        }

        int dd = 0;
        for (int i = 0; i < ItemsManager.Inventory.Count; i++)
        {
            var item = ItemsManager.Inventory[i].Cloned();
            if (item.canDuplicate)
            {
                duplicateSlots[dd].item = item;
                duplicateSlots[dd].RefreshSlotUI();
                dd++;
            }
        }
    }

    void SelectItemSlotToDuplicate(Slot selectedSlot)
    {
        var selectedItem = ItemsManager.GetItemFromCD(selectedSlot.item.CD);
        if (selectedItem == null)
            return;

        duplicateMask.SetActive(false);

        duplicateFinalSlot.item = selectedItem;
        duplicateFinalSlot.item.quantity = 1;
        duplicateFinalSlot.RefreshSlotUI();
        duplicateFill.gameObject.SetActive(false);
        duplicateFill.sprite = selectedItem.sprite;
        duplicateScroll.anchoredPosition = new Vector2(-selectedSlot.GetComponent<RectTransform>().anchoredPosition.x + 340, 0);

        duplicateButton.SetButton(Mathf.RoundToInt(selectedItem.price * 7.5f), true);
    }

    public void Duplicate()
    {
        duplicateButton.GetComponent<Button>().interactable = false;
        StartCoroutine(DuplicateTimer(SlotsManager.Manager.selectedSlot.item));
        FZAudio.Manager?.PlaySound(Sounds.Instance.duplicateItem);
    }

    IEnumerator DuplicateTimer(Item item)
    {
        duplicateFill.fillAmount = 0;
        duplicateFill.gameObject.SetActive(true);

        while (duplicateFill.fillAmount < 1)
        {
            yield return new WaitForSecondsRealtime(0.05f); //5 sec
            duplicateFill.fillAmount += 0.01f;
        }

        //Finish
        var it = item.Cloned();
        it.quantity = 1;
        ItemsManager.Manager.Add_ItemToInventory(it);
        duplicateButton.SetButton(Mathf.RoundToInt(item.price * 7.5f), true);
        ShowDuplicableItems();
    }
    #endregion

    #region Crafting
    void SelectItemSlotToCraft(Slot selectedSlot)
    {
        var selectedItem = ItemsManager.GetItemFromCD(selectedSlot.item.CD);
        if (selectedItem == null)
            return;

        craftFill.fillAmount = 0;
        craftMask.SetActive(false);

        if (!string.IsNullOrEmpty(selectedItem.craft1.CD))
        {
            recipeSlots[0].item = ItemsManager.GetItemFromCD(selectedItem.craft1.CD).Cloned();
            recipeSlots[0].item.quantity = selectedItem.craft1.quantity;
            recipeSlots[0].RefreshSlotUI();
        }
        else
            recipeSlots[0].ClearAndDisable();
        if (!string.IsNullOrEmpty(selectedItem.craft2.CD))
        {
            if (selectedItem.burned)
            {
                recipeSlots[2].item = ItemsManager.GetItemFromCD(selectedItem.craft2.CD).Cloned();
                recipeSlots[2].item.quantity = selectedItem.craft2.quantity;
                recipeSlots[2].RefreshSlotUI();
                recipeSlots[1].ClearAndDisable();
            }
            else
            {
                recipeSlots[1].item = ItemsManager.GetItemFromCD(selectedItem.craft2.CD).Cloned();
                recipeSlots[1].item.quantity = selectedItem.craft2.quantity;
                recipeSlots[1].RefreshSlotUI();
                recipeSlots[2].ClearAndDisable();
            }
        }
        else
        {
            recipeSlots[1].ClearAndDisable();
            recipeSlots[2].ClearAndDisable();
        }

        craftFinalSlot.item = selectedItem.Cloned();
        //finalSlot.item.quantity = ItemsManager.GetItemFromCD(finalSlot.item.CD).quantity;
        craftFinalSlot.RefreshSlotUI();

        craftButton.interactable = ItemsManager.HasResourcesToCraft(selectedItem.craft1, selectedItem.craft2);
        craftButton.GetComponentInChildren<Text>().text = selectedItem.burned ? "Burn" : "Craft";
    }

    public void Craft()
    {
        craftButton.interactable = false;
        StartCoroutine(CraftTimer(SlotsManager.Manager.selectedSlot.item));
    }

    IEnumerator CraftTimer(Item itemToCraft)
    {
        if (itemToCraft.burned)
            FZAudio.Manager?.PlaySound(Sounds.Instance.burnItem);
        else
            FZAudio.Manager?.PlaySound(Sounds.Instance.craftItem.RandomItem());

        craftFill.fillAmount = 0;
        while (craftFill.fillAmount < 1)
        {
            yield return new WaitForSecondsRealtime(0.03f); //3 sec
            craftFill.fillAmount += 0.01f;
        }
        Finish(itemToCraft);
    }

    void Finish(Item item)
    {
        if (!string.IsNullOrEmpty(item.craft1.CD))
        {
            var craft1 = item.craft1.GetRecipeItem();
            ItemsManager.Manager.Remove_ItemFromInventory(craft1);
        }
        if (!string.IsNullOrEmpty(item.craft2.CD))
        {
            var craft2 = item.craft2.GetRecipeItem();
            ItemsManager.Manager.Remove_ItemFromInventory(craft2);
        }

        ObjectivesManager.Manager.UpdateObjective(2, item.quantity);

        ItemsManager.Manager.Add_ItemToInventory(item);
        SelectItemSlotToCraft(SlotsManager.Manager.selectedSlot);
    }
    #endregion
}
