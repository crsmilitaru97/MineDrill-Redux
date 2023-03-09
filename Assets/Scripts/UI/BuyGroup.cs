using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuyGroup : MonoBehaviour
{
    [Header("Data")]
    public int price;
    public bool canBeBuyed;
    public bool acceptNegativeTotal;

    [Header("UI")]
    public Text priceText;
    public Button buyButton;

    [Header("Calls")]
    public UnityEvent[] functionsToCall;

    private void OnEnable()
    {
        SetButton(price, canBeBuyed);
    }

    public void PressBuy()
    {
        ItemsManager.Manager.ModifyCoins(-price);
        Refresh();

        foreach (var function in functionsToCall)
        {
            function.Invoke();
        }
    }

    public void SetButton(int value, bool canBeBuyed, bool acceptNegativeTotal = false)
    {
        this.canBeBuyed = canBeBuyed;
        this.price = value;
        priceText.text = value.ToString();
        this.acceptNegativeTotal = acceptNegativeTotal;

        Refresh();
    }

    void Refresh()
    {
        if (!acceptNegativeTotal)
            priceText.color = ItemsManager.totalCoins - price < 0 ? Color.red : UIManager.Manager.goldColor;

        buyButton.interactable = canBeBuyed && (acceptNegativeTotal || ItemsManager.totalCoins - price >= 0);

        if (!canBeBuyed)
            buyButton.GetComponent<UIElement>().flyoutText = "Can't be buyed right now";
        else if (ItemsManager.totalCoins - price < 0)
            buyButton.GetComponent<UIElement>().flyoutText = "Not enough coins";
        else
            buyButton.GetComponent<UIElement>().flyoutText = string.Empty;
    }
}
