using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Constants;
using static SlotsManager;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Part part;
    public Powerup powerup;
    public RocketPart rocketPart;

    [Header("UI")]
    public Image itemImage;
    public Image quantityImage;
    public Text quantityText;

    [Header("Types")]
    public SlotType slotType;
    public bool isViewOnly;
    public bool isSpecial;
    public bool isBuy;
    public bool isSell;
    public bool isSelected;
    public bool isCraft;
    public bool isRecipe;

    public bool isPart;
    public bool isItem;
    public bool isPowerup;
    public bool isRocketPart;

    bool isEnabled;

    int quantity;

    private void Start()
    {
        RefreshSlotUI();
    }

    public void RefreshSlotUI()
    {
        if (!isSpecial)
        {
            isPowerup = !string.IsNullOrEmpty(powerup.CD);
            isItem = !string.IsNullOrEmpty(item.CD);
            isPart = !string.IsNullOrEmpty(part.CD);
            isRocketPart = !string.IsNullOrEmpty(rocketPart.CD);
        }

        isEnabled = (isItem && item.quantity > 0) ||
                    (isPowerup && isBuy) || (isPowerup && powerup.quantity > 0) ||
                    isPart ||
                    isRocketPart;

        quantity = isItem ? item.quantity : isPowerup ? powerup.quantity : part.level;

        if (isEnabled)
            EnableSlotUI();
        else
            DisableSlotUI();

    }

    void EnableSlotUI()
    {
        gameObject.SetActive(true);
        isEnabled = true;

        itemImage.type = Image.Type.Simple;
        itemImage.preserveAspect = true;
        GetComponent<Image>().color = Color.white;
        itemImage.gameObject.SetActive(true);


        itemImage.sprite = isItem ? item.sprite : isPart ? part.sprite : isRocketPart ? rocketPart.sprite : powerup.sprite;
        if (GetComponent<UIElement>() != null)
            GetComponent<UIElement>().flyoutText = isItem ? item.CD : isPart ? part.CD : isRocketPart ? rocketPart.CD : powerup.CD;

        if (quantityImage != null || quantityText != null)
        {
            quantityText.text = quantity.ToString();
            quantityImage.color = Color.white;
            if (isPart)
            {
                quantityText.gameObject.SetActive(true);
                quantityImage.gameObject.SetActive(true);
                if (part.isMaxedOut)
                    quantityText.text = "MAX";

            }
        }

        if (isRecipe)
        {
            if (ItemsManager.IsCDInInventory(item.CD, item.quantity) || string.IsNullOrEmpty(item.CD))
                quantityImage.color = Manager.recipeOKColor;
            else
                quantityImage.color = Manager.recipeNOColor;
        }

        if (part.isMaxedOut && !isViewOnly)
        {
            GetComponent<Image>().color = Manager.disabledColor;
            return;
        }

        if (isRocketPart && rocketPart.isCrafted)
        {
            GetComponent<Image>().color = Manager.disabledColor;
            return;
        }
    }

    public void ClearAndDisable()
    {
        item.CD = null;
        powerup.CD = null;
        part.CD = null;
        rocketPart.CD = null;

        item.quantity = 0;
        powerup.quantity = 0;
        part.level = 0;

        DisableSlotUI();
    }

    public void DisableSlotUI()
    {
        if (isCraft || isPowerup || slotType == SlotType.isDuplicate)
        {
            gameObject.SetActive(false);
            return;
        }
        isEnabled = false;

        item.CD = null;
        GetComponent<Image>().color = Manager.disabledColor;
        if (GetComponent<UIElement>() != null)
            GetComponent<UIElement>().flyoutText = string.Empty;

        itemImage.gameObject.SetActive(false);
        if (quantityImage != null || quantityText != null)
        {
            quantityImage.color = Manager.disabledColor;
            quantityText.text = string.Empty;
        }
    }

    public void UnselectUI()
    {
        Manager.selectedSlot.GetComponent<Image>().color = Color.white;
    }

    private void OnDisable()
    {
        if (isSelected)
        {
            if (Manager.selectedSlot == null)
            {
                return;
            }
            Manager.selectedSlot.UnselectUI();
            Manager.selectedSlot.isSelected = false;
            Manager.selectedSlot = null;

            if (isBuy)
            {
                Market.Instance.buyGroup.SetButton(0, false);
                Market.Instance.hintDescrGroup.gameObject.SetActive(true);
            }
            if (isSell)
            {
                Market.Instance.sellButton.interactable = false;
                Market.Instance.priceGroupSell.gameObject.SetActive(false);
                Market.Instance.sellMinus.interactable = false;
                Market.Instance.sellPlus.interactable = false;
            }
            if (isPart)
            {
                Service.Instance.upgradeRecipeMask.SetActive(true);
            }
            if (isCraft)
            {
                Workshop.Instance.craftMask.SetActive(true);
            }
            if (slotType == SlotType.isInfo)
            {
                ItemsManager.Manager.infoMask.SetActive(true);
            }
            if (isRocketPart)
            {
                Rocket.Instance.craftMask.SetActive(true);
            }
            if (slotType == SlotType.isCargo)
            {
                ItemsManager.Manager.disposeButton.interactable = false;
            }
            if (slotType == SlotType.isDuplicate)
            {
                Workshop.Instance.duplicateMask.SetActive(true);
            }
        }

        if (isSpecial)
        {
            DisableSlotUI();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSlot();
    }

    public void SelectSlot()
    {
        if (isRocketPart && rocketPart.isCrafted)
            return;

        if (isViewOnly || !isEnabled || isPart || part.isMaxedOut)
            return;

        isSelected = true;

        if (Manager.selectedSlot != null)
        {
            Manager.selectedSlot.isSelected = false;
            Manager.selectedSlot.GetComponent<Image>().color = Color.white;
        }

        Manager.selectedSlot = GetComponent<Slot>();
        GetComponent<Image>().color = Manager.selectedColor;
        Manager.SlotSelect();

        if (isPowerup && isSpecial)
        {
            PowerupsManager.Manager.UsePowerup(Manager.selectedSlot);
        }
    }
}
