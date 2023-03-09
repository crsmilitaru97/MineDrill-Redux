using UnityEngine;
using UnityEngine.UI;

public class TreasureManager : MonoBehaviour
{
    public FZButton[] keyButtons;
    public Slot[] rewardSlots;
    public Image keyHoleImage;
    public GameObject openSubpanel;
    public FZPanel treasurePanel;
    [HideInInspector]
    public Treasure selectedTreasure;

    public static TreasureManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenTreasure()
    {
        //Closed
        if (!selectedTreasure.unlocked)
        {
            for (int i = 0; i < MobsManager.Manager.Keys.Length; i++)
            {
                keyButtons[i].buttonText.text = MobsManager.Manager.Keys[i].quantity.ToString();
                keyButtons[i].buttonImage.sprite = MobsManager.Manager.Keys[i].keySprite;
            }
            keyHoleImage.sprite = MobsManager.Manager.Keys[selectedTreasure.keyHoleID].holeSprite;
            ChooseRewards();
        }
        else
        {
            openSubpanel.SetActive(true);
        }

        FZPanelsManager.Manager.OpenPanel(treasurePanel);
    }

    public void CollectRewards()
    {
        foreach (Slot slot in rewardSlots)
        {
            ItemsManager.Manager.Add_ToCargo(slot.item);
            slot.ClearAndDisable();
        }
        TerrainManager.DestroyBlock(selectedTreasure.gameObject);
        FZPanelsManager.Manager.ClosePanel(treasurePanel);
    }

    void ChooseRewards()
    {
        foreach (Slot slot in rewardSlots)
        {
            slot.item = ItemsManager.Manager.Items[Random.Range(0, ItemsManager.Manager.Items.Count)].Cloned();
            slot.item.quantity = Random.Range(0, 10 - slot.item.price);
            slot.RefreshSlotUI();
        }
    }

    public void SelectKey(int ID)
    {
        if (ID == selectedTreasure.keyHoleID)
        {
            if (MobsManager.Manager.Keys[ID].quantity > 0)
            {
                //keyButtons[ID].GetComponent<Animator>().SetTrigger("Use");
                MobsManager.Manager.RemoveKey(ID);
                openSubpanel.SetActive(true);
                selectedTreasure.unlocked = true;
            }
        }
        else
        {
            //keyButtons[ID].GetComponent<Animator>().SetTrigger("Brake");
        }
    }
}
