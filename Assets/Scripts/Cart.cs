using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Constants;
using static Mine;

public class Cart : MonoBehaviour
{
    public List<Item> CollectItems = new List<Item>();

    public int ID;
    public bool isCartEnabled;
    public bool isCartSent;

    [Header("UI")]
    public FZButton cartButton;
    public Image timeImage;
    public GameObject collectGroup;
    public FZASimple[] wheels;
    public FZASimple cart;

    int totalTime = 300;
    Item[] items;

    private void Start()
    {
        items = new Item[] { TerrainManager.Instance.Iron.resource, TerrainManager.Instance.Gold.resource, TerrainManager.Instance.Diamond.resource };
    }

    public IEnumerator Timer()
    {
        foreach (var wheel in wheels)
        {
            wheel.Activate();
        }
        cart.Activate();

        isCartSent = true;
        timeImage.fillAmount = 0;
        int time = 0;
        while (time < totalTime) //5 min
        {
            yield return new WaitForSecondsRealtime(1);
            time++;
            if (totalTime - time > 60)
                cartButton.GetComponent<UIElement>().flyoutText = Mathf.Round((totalTime - time) / 60f).ToString() + " min";
            else
                cartButton.GetComponent<UIElement>().flyoutText = Mathf.Round(totalTime - time).ToString() + " sec";

            timeImage.fillAmount = time / 300f;
        }
        TimerFinish();
        cartButton.GetComponent<UIElement>().flyoutText = null;
    }

    public void EnableCart()
    {
        cartButton.gameObject.SetActive(true);
        FZSave.Bool.Set("isCartEnabled" + ID, true);
        isCartEnabled = true;
        Instance.sendButton.interactable = true;
    }

    void TimerFinish()
    {
        foreach (var wheel in wheels)
        {
            wheel.Deactivate();
        }
        cart.Deactivate();

        timeImage.fillAmount = 0;
        cartButton.interactable = true;
        Instance.cartArrived.SetActive(true);
        cartButton.buttonImage.sprite = Instance.cartFullSprite;
        isCartSent = false;

        Image itemImage1 = collectGroup.transform.GetChild(0).GetComponent<Image>();
        Text itemText1 = itemImage1.transform.GetChild(0).GetComponent<Text>();

        Image itemImage2 = collectGroup.transform.GetChild(1).GetComponent<Image>();
        Text itemText2 = itemImage2.transform.GetChild(0).GetComponent<Text>();

        Item item1 = GetItem();
        itemImage1.sprite = item1.sprite;
        itemText1.text = item1.quantity.ToString();

        Item item2 = GetItem();
        itemImage2.sprite = item2.sprite;
        itemText2.text = item2.quantity.ToString();

        CollectItems.Add(item1);
        CollectItems.Add(item2);
    }

    private Item GetItem()
    {
        int random = Random.Range(0, 60);

        if (random < 10)  // 10%
        {
            Item item3 = items[2];
            item3.quantity = Random.Range(1, 3);
            return item3;
        }
        else if (random < 30)  // 20%
        {
            Item item2 = items[1];
            item2.quantity = Random.Range(1, 7);
            return item2;
        }
        else  // 30%
        {
            Item item1 = items[0];
            item1.quantity = Random.Range(1, 12);
            return item1;
        }
    }

    public void Collect()
    {
        foreach (var item in CollectItems)
        {
            ItemsManager.Manager.Add_ItemToInventory(item);
        }
        collectGroup.SetActive(true);
        CollectItems.Clear();

        cartButton.buttonImage.sprite = Instance.cartSprite;
        Instance.sendButton.interactable = true;
        cartButton.interactable = false;
    }
}
