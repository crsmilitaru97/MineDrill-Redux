using System;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    [Header("UI")]
    public Button sellMinus, sellPlus;
    public Slot specialSlot;
    public Button sellButton;
    public BuyGroup buyGroup;
    public GameObject priceGroupSell;
    public Image infoImage;
    public Text infoNameText;
    public Text infoDescrText;
    public GameObject hintDescrGroup;

    int quantityToSell;
    int price;

    public static Market Instance;
    private void Awake() => Instance = this;

    private void Start()
    {
        SlotsManager.Manager.SlotSelected += SlotSelected;
    }

    public void SlotSelected(object source, EventArgs eventArgs)
    {
        if (SlotsManager.Manager.selectedSlot.isBuy)
        { SelectItemSlotToBuy(SlotsManager.Manager.selectedSlot); }
        else if (SlotsManager.Manager.selectedSlot.isSell)
        { SelectItemSlotToSell(SlotsManager.Manager.selectedSlot); }
    }

    public void SellItem()
    {
        ItemsManager.Manager.ModifyCoins(price);
        ItemsManager.Manager.Remove_ItemFromInventory(specialSlot.item);

        priceGroupSell.SetActive(false);
        sellPlus.interactable = false;
        sellMinus.interactable = false;
        sellButton.interactable = false;
        specialSlot.ClearAndDisable();

        if (specialSlot.item.CD == "Stone")
        {
            ObjectivesManager.Manager.UpdateObjective(1, 1);
        }

        SlotsManager.Manager.selectedSlot.isSelected = false;
        SlotsManager.Manager.selectedSlot = null;

    }

    public void BuyPowerup()
    {
        var selectedSlot = SlotsManager.Manager.selectedSlot;
        PowerupsManager.Manager.AddToBuyedPowerupsList(selectedSlot?.powerup);
    }

    public void SelectItemSlotToBuy(Slot selectedSlot)
    {
        hintDescrGroup.SetActive(false);

        infoImage.sprite = selectedSlot.powerup.sprite;
        infoNameText.text = selectedSlot.powerup.CD;
        infoDescrText.text = selectedSlot.powerup.description;

        buyGroup.SetButton(selectedSlot.powerup.price, true);
    }

    public void SelectItemSlotToSell(Slot selectedSlot)
    {
        quantityToSell = 1;
        sellMinus.interactable = false;
        sellPlus.interactable = !(quantityToSell == selectedSlot.item.quantity);
        sellButton.interactable = true;

        specialSlot.item = selectedSlot.item.Cloned();
        specialSlot.item.quantity = quantityToSell;
        specialSlot.RefreshSlotUI();

        priceGroupSell.SetActive(true);
        price = selectedSlot.item.price * 5;
        priceGroupSell.GetComponentInChildren<Text>().text = price.ToString();
    }

    public void ChangeQuantity(int modif)
    {
        quantityToSell += modif;
        sellMinus.interactable = !(quantityToSell == 1);
        sellPlus.interactable = !(quantityToSell == SlotsManager.Manager.selectedSlot.item.quantity);
        specialSlot.item.quantity = quantityToSell;
        price = SlotsManager.Manager.selectedSlot.item.price * quantityToSell * 5;
        priceGroupSell.GetComponentInChildren<Text>().text = price.ToString();

        specialSlot.RefreshSlotUI();
    }
}
