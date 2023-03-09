using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class Rocket : MonoBehaviour
{
    public RocketPart[] rocketParts;

    [Header("UI")]
    public Slot[] partsSlots;
    public GameObject[] realParts;
    public BuyGroup launchButton;
    public Button craftButton;
    public Slot[] recipeSlots;
    public Image[] finalFills;
    public GameObject craftMask;
    public FZCanvas canvas;
    [Header("Buildings")]
    public GameObject crashSite;
    public GameObject launchPad;

    int craftedParts;

    public static Rocket Instance;
    private void Awake() => Instance = this;

    void SlotSelected(object source, EventArgs eventArgs)
    {
        if (SlotsManager.Manager.selectedSlot.isRocketPart)
        { SelectRocketPartToCraft(SlotsManager.Manager.selectedSlot); }
    }

    private void SelectRocketPartToCraft(Slot selectedSlot)
    {
        var selectedPart = selectedSlot.rocketPart;
        if (selectedPart == null)
            return;

        craftMask.SetActive(false);
        RefreshCraftUI();

        for (int i = 0; i < selectedPart.recipes.Length; i++)
        {
            if (!string.IsNullOrEmpty(selectedPart.recipes[i].CD))
            {
                recipeSlots[i].item = ItemsManager.GetItemFromCD(selectedPart.recipes[i].CD).Cloned();
                recipeSlots[i].item.quantity = selectedPart.recipes[i].quantity;
                recipeSlots[i].RefreshSlotUI();
            }
            else
                recipeSlots[i].DisableSlotUI();
        }

        craftButton.interactable = ItemsManager.HasResourcesToCraft(selectedPart.recipes[0], selectedPart.recipes[1], selectedPart.recipes[2]);
    }

    void RefreshCraftUI()
    {
        recipeSlots[0].ClearAndDisable();
        recipeSlots[1].ClearAndDisable();
        recipeSlots[2].ClearAndDisable();
        foreach (var fills in finalFills)
        {
            fills.fillAmount = 0;
        }
    }

    void Start()
    {
        SlotsManager.Manager.SlotSelected += SlotSelected;

        for (int i = 0; i < partsSlots.Length; i++)
        {
            partsSlots[i].rocketPart = rocketParts[i];
        }

        RefreshCraftUI();
        GetSave();

        CalculateLaunchProgress();
    }

    void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            crashSite.SetActive(!FZSave.Bool.Get("spaceStation", false));
            launchPad.SetActive(FZSave.Bool.Get("spaceStation", false));
            for (int i = 0; i < rocketParts.Length; i++)
            {
                rocketParts[i].isCrafted = FZSave.Bool.Get("rocketPartCrafted" + i.ToString(), false);
                realParts[i].SetActive(rocketParts[i].isCrafted);
            }
        }
        else
        {
            FZSave.Bool.Set("spaceStation", false);
            crashSite.SetActive(true);
            launchPad.SetActive(false);

            for (int i = 0; i < rocketParts.Length; i++)
            {
                FZSave.Bool.Set("rocketPartCrafted" + i.ToString(), false);
                realParts[i].SetActive(false);
            }
        }
    }

    public void BuildLaunchPad()
    {
        crashSite.SetActive(false);
        launchPad.SetActive(true);
        FZSave.Bool.Set("spaceStation", true);
        FZPanelsManager.Manager.CloseAllPanels();
    }

    public void CraftRocketPart()
    {
        craftButton.interactable = false;
        StartCoroutine(Timer(SlotsManager.Manager.selectedSlot.rocketPart));
    }

    public void LaunchRocket()
    {
        FZPanelsManager.Manager.CloseAllPanels();
        CameraLogic.zoomToRocket = true;
        StartCoroutine(Story.Instance.LaunchTimer());
    }

    void FinishCrafting(RocketPart rocketPart)
    {
        ItemsManager.Manager.TryRemoveRecipesFromInventory(rocketPart.recipes);
        rocketPart.isCrafted = true;
        FZSave.Bool.Set("rocketPartCrafted" + rocketPart.ID.ToString(), true);
        realParts[rocketPart.ID].SetActive(true);
        RefreshCraftUI();
        craftMask.SetActive(true);

        SlotsManager.Manager.selectedSlot.GetComponent<Image>().color = SlotsManager.Manager.disabledColor;
        SlotsManager.Manager.selectedSlot.isSelected = false;
        SlotsManager.Manager.selectedSlot = null;

        CalculateLaunchProgress();

        if (craftedParts == 1)
            FZStory.TryShowMessagesWithDelay(100, 0.25f, Story.Messages.rocket1part);
        if (craftedParts == 2)
            FZStory.TryShowMessagesWithDelay(100, 0.25f, Story.Messages.rocket2part);
        if (craftedParts == 3)
            FZStory.TryShowMessagesWithDelay(100, 0.25f, Story.Messages.rocket3part);
    }

    void CalculateLaunchProgress()
    {
        craftedParts = 0;
        foreach (var part in rocketParts)
        {
            if (part.isCrafted)
            {
                craftedParts++;
            }
        }

        launchButton.SetButton(1000, craftedParts == rocketParts.Length);
    }

    IEnumerator Timer(RocketPart rocketPart)
    {
        int time = 0;
        while (time < 50) //5 sec
        {
            yield return new WaitForSecondsRealtime(0.1f);
            time++;
            finalFills[rocketPart.ID].fillAmount = time / 50f;
        }
        FinishCrafting(rocketPart);
    }
}
