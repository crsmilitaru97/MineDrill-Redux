using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class ItemsManager : MonoBehaviour
{
    public List<Item> Items = new List<Item>();
    public Transform canvas;
    public FZPanel chestPanel;

    [Header("UI")]
    public Button disposeButton;
    public FZText coinsText;
    public FZText cargoText, storageText;
    public Text cargoMaxText;
    public GameObject cargoGroup, storageGroup;
    public GameObject addItemsGroup;

    [Header("Info group")]
    public GameObject infoMask;
    public Text infoNameText;
    public Text infoDescrText;
    public Text infoDropQuantity;
    public Text infoObtainedBy;

    [Header("Slot Lists")]
    public Slot[] cargoSlots;
    public Slot[] marketSlots;
    public Slot[] storageSlots;
    public Slot[] chestSlots;
    public Slot[] cargoChestSlots;

    public static int totalCargoItems = 0;
    public static int totalInventoryItems = 0;
    public static int totalCoins = 0;

    public static List<Item> Inventory = new List<Item>();
    public static List<Item> Cargo = new List<Item>();

    public static ItemsManager Manager;

    private void Awake()
    {
        Manager = this;
    }

    void Start()
    {
        SlotsManager.Manager.SlotSelected += SlotSelected;

        GetSave();
    }

    public void FullTheStorage()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            var item = storageSlots[i].item = Items[i].Cloned();
            item.quantity = 50;
            storageSlots[i].RefreshSlotUI();
        }
    }

    void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            totalCoins = FZSave.Int.GetToText(FZSave.Constants.Coins, coinsText, 0);

            var items = FZSave.Dictionary.GetStringIntPair(FZSave.Constants.Items);
            foreach (var itemSaved in items)
            {
                var item = GetItemFromCD(itemSaved.Key);
                item.quantity = itemSaved.Value;
                AddItemToInventorySilent(item);
            }

            ShowInventoryItemsOnUI(false);
        }
        else
        {
            totalCoins = 0;
            coinsText.text = totalCoins.ToString();
            FZSave.Delete(FZSave.Constants.Coins);
            FZSave.Delete(FZSave.Constants.Items);
        }
    }

    public void AddItemToInventorySilent(Item item)
    {
        Item already = Inventory.Find(e => e.CD == item.CD);
        if (already != null)
        {
            already.quantity += item.quantity;
        }
        else
        {
            already = item.Cloned();
            Inventory.Add(already);
        }
    }

    public void ArriveOnLand()
    {
        cargoGroup.SetActive(false);
        storageGroup.SetActive(true);

        foreach (Item item in Cargo)
        {
            if (!string.IsNullOrEmpty(item.CD))
            {
                AddItemToInventorySilent(item);
            }
        }

        Cargo.Clear();
        totalCargoItems = 0;
        cargoText.text = totalCargoItems.ToString();
        ShowInventoryItemsOnUI();
    }

    public static Item GetItemFromCD(string CD)
    {
        return Manager.Items.FirstOrDefault(e => e.CD == CD).Cloned();
    }

    public static bool IsCDInInventory(string CD, int qty = 1)
    {
        if (string.IsNullOrEmpty(CD))
            return false;

        return Inventory.Any(e => e.CD == CD && e.quantity >= qty);
    }

    public static bool IsNullOrInInventory(Recipe craft)
    {
        if (craft == null || craft.CD == string.Empty)
            return true;
        else
            return IsCDInInventory(craft.CD, craft.quantity);
    }

    public static bool HasResourcesToCraft(Recipe craft1, Recipe craft2 = null, Recipe craft3 = null)
    {
        return IsNullOrInInventory(craft1)
            && IsNullOrInInventory(craft2)
            && IsNullOrInInventory(craft3);
    }

    public void LeaveLand()
    {
        cargoGroup.SetActive(true);
        storageGroup.SetActive(false);
    }

    public void ModifyCoins(int price)
    {
        totalCoins += price;
        Manager.coinsText.SlowlyUpdateNumberText(totalCoins);
        FZSave.Int.Set(FZSave.Constants.Coins, totalCoins);
    }

    public void Add_ToCargo(Item item)
    {
        if (totalCargoItems + item.quantity <= Drill.Instance.cargoMaxQuantity)
        {
            totalCargoItems += item.quantity;
            cargoText.SlowlyUpdateNumberText(totalCargoItems);

            Item already = Cargo.Find(e => e.CD == item.CD);
            if (already != null)
            {
                already.quantity += item.quantity;
            }
            else
            {
                Cargo.Add(item.Cloned());
            }
            AddItemsUIGroup(item);
            ShowCargoItemsOnUI();
        }
    }

    public void LoadChestItems(int chestID)
    {
        var chestItems = FZSave.Dictionary.GetStringIntPair("Chest" + chestID.ToString());

        for (int i = 0; i < chestSlots.Count(); i++)
        {
            chestSlots[i].item.CD = chestItems.ElementAt(chestID).Key;
            chestSlots[i].item.quantity = chestItems.ElementAt(chestID).Value;
        }
    }

    public Item Add_ItemToInventory(Item item)
    {
        Item already = Inventory.Find(e => e.CD == item.CD);
        if (already != null)
        {
            already.quantity += item.quantity;
        }
        else
        {
            already = item.Cloned();
            Inventory.Add(already);
        }
        AddItemsUIGroup(item);
        ShowInventoryItemsOnUI();
        return already;
    }

    int addItemsY = -250;
    bool isZeroYUsed = false;
    public void AddItemsUIGroup(Item itemToAdd)
    {
        if (!isZeroYUsed)
            StartCoroutine(TimerForAddToCargoY());
        var a = Instantiate(addItemsGroup, canvas);
        a.GetComponent<RectTransform>().anchoredPosition = new Vector3(70, addItemsY, 0);
        addItemsY -= 80;
        a.SetActive(true);
        a.GetComponentInChildren<Text>().text = "+" + itemToAdd.quantity.ToString();
        a.GetComponentInChildren<Image>().sprite = itemToAdd.sprite;
    }

    public void Remove_ItemFromInventory(Item item)
    {
        Item already = GetItemFromInventory(item.CD);
        already.quantity -= item.quantity;
        if (already.quantity <= 0)
        {
            Inventory.Remove(already);
        }

        ShowInventoryItemsOnUI();
    }

    Item GetItemFromInventory(string CD)
    {
        return Inventory.Find(e => e.CD == CD);
    }


    public void TryRemoveRecipesFromInventory(Recipe[] recipes)
    {
        foreach (Recipe recipe in recipes)
        {
            if (!string.IsNullOrEmpty(recipe.CD))
            {
                Remove_ItemFromInventory(recipe.GetRecipeItem());
            }
        }
    }

    public bool Remove_FromCargo(Item item, int qty)
    {
        bool slotEmpty = false;

        Item already = Cargo.Find(e => e.CD == item.CD);
        already.quantity -= qty;
        if (already.quantity <= 0)
        {
            Cargo.Remove(already); slotEmpty = true;
        }
        ShowCargoItemsOnUI();
        return slotEmpty;
    }

    public void DisposeItemsFromCargo()
    {
        bool slotEmpty = Remove_FromCargo(SlotsManager.Manager.selectedSlot.item, 1);

        if (slotEmpty)
        {
            SlotsManager.Manager.selectedSlot.isSelected = false;
            SlotsManager.Manager.selectedSlot = null;
            disposeButton.interactable = false;
        }
        else
            SlotsManager.Manager.selectedSlot.GetComponent<Image>().color = SlotsManager.Manager.selectedColor;
    }

    void SlotSelected(object source, EventArgs eventArgs)
    {
        if (SlotsManager.Manager.selectedSlot.isItem &&
            SlotsManager.Manager.selectedSlot.slotType == SlotsManager.SlotType.isCargo)
        { SelectItemSlotToDispose(); }

        if (SlotsManager.Manager.selectedSlot.slotType == SlotsManager.SlotType.isInfo)
        { SelectItemSlotForInfo(SlotsManager.Manager.selectedSlot); }
    }

    private void SelectItemSlotToDispose()
    {
        disposeButton.interactable = true;
    }

    private void SelectItemSlotForInfo(Slot selectedSlot)
    {
        var item = selectedSlot.item;
        infoMask.SetActive(false);
        infoDescrText.text = selectedSlot.item.description;
        infoNameText.text = selectedSlot.item.CD;
        infoDropQuantity.text = selectedSlot.item.quantity.ToString();
        infoObtainedBy.text = item.burned ? "Burning" : !string.IsNullOrEmpty(item.craft1.CD) ? "Crafting" : "Drilling";
    }

    void ShowInventoryItemsOnUI(bool slowly = true)
    {
        Workshop.Instance.ShowDuplicableItems();

        SaveItems();

        //Total
        totalInventoryItems = 0;
        foreach (var item in Inventory)
        {
            totalInventoryItems += item.quantity;
        }
        if (slowly)
            storageText.SlowlyUpdateNumberText(totalInventoryItems);
        else
            storageText.text = totalInventoryItems.ToString();

        //Storage
        for (int i = 0; i < storageSlots.Length; i++)
        {
            if (i < Inventory.Count)
                storageSlots[i].item = Inventory[i].Cloned();
            else
                storageSlots[i].ClearAndDisable();
            storageSlots[i].RefreshSlotUI();
        }

        //Order by price in market        
        List<Item> InventoryOrdered = Inventory.OrderByDescending(e => e.price).ThenByDescending(x => x.quantity).ToList();
        for (int i = 0; i < marketSlots.Length; i++)
        {
            if (i < InventoryOrdered.Count)
                marketSlots[i].item = InventoryOrdered[i].Cloned();
            else
                marketSlots[i].ClearAndDisable();
            marketSlots[i].RefreshSlotUI();
        }
    }

    void SaveItems()
    {
        Dictionary<string, int> items = new Dictionary<string, int>();
        foreach (var item in Inventory)
        {
            items.Add(item.CD, item.quantity);
        }
        FZSave.Dictionary.SetStringIntPair(FZSave.Constants.Items, items);
    }

    void ShowCargoItemsOnUI()
    {
        //Cargo
        for (int i = 0; i < cargoSlots.Length; i++)
        {
            if (i < Cargo.Count)
                cargoSlots[i].item = Cargo[i].Cloned();
            else
                cargoSlots[i].ClearAndDisable();

            cargoSlots[i].RefreshSlotUI();
        }

        //Chest cargo
        for (int i = 0; i < cargoChestSlots.Length; i++)
        {
            if (i < Cargo.Count)
                cargoChestSlots[i].item = Cargo[i].Cloned();

            cargoSlots[i].RefreshSlotUI();
        }
    }

    public IEnumerator TimerForAddToCargoY()
    {
        isZeroYUsed = true;
        yield return new WaitForSecondsRealtime(2);
        addItemsY = -250;
        isZeroYUsed = false;
    }
}
