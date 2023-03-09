using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour
{
    public Cart[] minecarts;
    public Button sendButton;
    public BuyGroup buyGroup;
    public Sprite cartSprite;
    public Sprite cartFullSprite;
    public GameObject cartArrived;

    public static Mine Instance;
    private void Awake() => Instance = this;

    private void Start()
    {
        GetSave();
    }

    private void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            bool val = !FZSave.Bool.Get("isCartEnabled" + (minecarts.Length - 1).ToString(), false);
            buyGroup.gameObject.SetActive(val);

            for (int i = 0; i < minecarts.Length; i++)
            {
                if (FZSave.Bool.Get("isCartEnabled" + i, false))
                    minecarts[i].EnableCart();
                else
                    minecarts[i].cartButton.gameObject.SetActive(false);
            }

            buyGroup.SetButton(100 * minecarts.Count(e => e.enabled == true), true);
        }
        else
        {
            for (int i = 0; i < minecarts.Length; i++)
            {
                FZSave.Bool.Set("isCartEnabled" + i.ToString(), false);
                minecarts[i].cartButton.gameObject.SetActive(false);
            }
            buyGroup.SetButton(100, true);
        }
    }

    public void BuyNewCart()
    {
        for (int i = 0; i < minecarts.Length; i++)
        {
            if (!minecarts[i].isCartEnabled)
            {
                minecarts[i].EnableCart();
                break;
            }
        }
        bool isLastCartEnabled = !minecarts[minecarts.Length - 1].isCartEnabled;
        buyGroup.gameObject.SetActive(isLastCartEnabled);
        buyGroup.SetButton(100 * minecarts.Count(e => e.enabled == true), true);
    }

    public void SendCarts()
    {
        Instance.cartArrived.SetActive(false);
        sendButton.interactable = false;

        foreach (Cart cart in minecarts)
        {
            if (cart.isCartSent == false && cart.isCartEnabled == true)
            {
                cart.collectGroup.SetActive(false);
                cart.StartCoroutine(cart.Timer());
                cart.isCartSent = true;
            }
            if (!cart.isCartSent)
            {
                Instance.cartArrived.SetActive(true);
            }
        }
    }
}
