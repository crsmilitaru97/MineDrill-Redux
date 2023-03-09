using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class Service : MonoBehaviour
{
    [Header("Main")]
    public BuyGroup repairButton;
    public Button fillButton;
    public Text damageLevelText;
    [Header("UI")]
    public Image infoImage;
    public Text infoNameText;
    public Text infoDescrText;
    [Header("Upgrades")]
    public GameObject upgradeRecipeMask;
    public Button upgradeButton;
    public Slot[] recipeSlots;
    public Slot finalSlot;
    public Image upgradeTimerImage;
    [Header("Mods")]
    public Slot[] craftModRecipeSlots;

    public Button craftModButton;
    public GameObject craftModMask;
    public FZProgressBar craftModProgBar;

    public static Service Instance;
    private void Awake() => Instance = this;

    void SlotSelected(object source, EventArgs eventArgs)
    {
        if (SlotsManager.Manager.selectedSlot.isPart)
        { SelectItemSlotToUpgradePart(SlotsManager.Manager.selectedSlot); }
    }

    void Start()
    {
        SlotsManager.Manager.SlotSelected += SlotSelected;
    }

    public void RepairDrill()
    {
        Drill.Instance.RegenerateAllLifes();
    }

    public void FillTank()
    {
        Drill.Instance.ChangeFuelAmount(Drill.Instance.fuelMax);
    }

    public void UpgradeOrCraftMod()
    {
        upgradeButton.interactable = false;
        StartCoroutine(Timer(SlotsManager.Manager.selectedSlot.part));
    }
    void SelectItemSlotToCraftMod(Slot selectedSlot)
    {
        var selectedMod = selectedSlot.part;
        if (selectedMod == null)
            return;

    }

    void SelectItemSlotToUpgradePart(Slot selectedSlot)
    {
        var selectedMod = selectedSlot.part;
        if (selectedMod == null)
            return;

        infoImage.sprite = selectedMod.sprite;
        infoDescrText.text = selectedMod.description;
        infoNameText.text = selectedMod.CD;

        upgradeRecipeMask.SetActive(false);
        RefreshCraftUI();

        if (!string.IsNullOrEmpty(selectedMod.crafts1[selectedMod.level - 1].CD))
        {
            recipeSlots[0].item = ItemsManager.GetItemFromCD(selectedMod.crafts1[selectedMod.level - 1].CD).Cloned();
            recipeSlots[0].item.quantity = selectedMod.crafts1[selectedMod.level - 1].quantity;
            recipeSlots[0].RefreshSlotUI();
        }
        else
            recipeSlots[0].DisableSlotUI();

        if (!string.IsNullOrEmpty(selectedMod.crafts2[selectedMod.level - 1].CD))
        {
            recipeSlots[1].item = ItemsManager.GetItemFromCD(selectedMod.crafts2[selectedMod.level - 1].CD).Cloned();
            recipeSlots[1].item.quantity = selectedMod.crafts2[selectedMod.level - 1].quantity;
            recipeSlots[1].RefreshSlotUI();
        }
        else
            recipeSlots[1].DisableSlotUI();

        finalSlot.part = selectedMod.Cloned();

        finalSlot.part.level += 1;
        finalSlot.part.isMaxedOut = finalSlot.part.level > finalSlot.part.crafts1.Length;

        finalSlot.RefreshSlotUI();

        upgradeButton.interactable = ItemsManager.HasResourcesToCraft(selectedMod.crafts1[selectedMod.level - 1], selectedMod.crafts2[selectedMod.level - 1]);
        upgradeButton.GetComponentInChildren<Text>().text = "Upgrade";
    }

    IEnumerator Timer(Part modToCraft)
    {
        int time = 0;
        while (time < 50) //5 sec
        {
            yield return new WaitForSecondsRealtime(0.1f);
            time++;
            upgradeTimerImage.fillAmount = time / 50f;
        }
        Finish(modToCraft);
    }

    void Finish(Part mod)
    {
        if (!string.IsNullOrEmpty(mod.crafts1[mod.level - 1].CD))
        {
            var craft1 = mod.crafts1[mod.level - 1].GetRecipeItem();
            ItemsManager.Manager.Remove_ItemFromInventory(craft1);
        }
        if (!string.IsNullOrEmpty(mod.crafts2[mod.level - 1].CD))
        {
            var craft2 = mod.crafts2[mod.level - 1].GetRecipeItem();
            ItemsManager.Manager.Remove_ItemFromInventory(craft2);
        }

        if (mod.CD == "Engine")
            ObjectivesManager.Manager.UpdateObjective(3, 1);


        ModsManager.Manager.UpgradePart(mod);
        RefreshCraftUI();
        upgradeRecipeMask.SetActive(true);
    }

    void RefreshCraftUI()
    {
        recipeSlots[0].ClearAndDisable();
        recipeSlots[1].ClearAndDisable();
        finalSlot.ClearAndDisable();
        upgradeTimerImage.fillAmount = 0;
    }
}
