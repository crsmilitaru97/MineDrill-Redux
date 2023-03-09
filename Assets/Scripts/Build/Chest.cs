using UnityEngine;

public class Chest : MonoBehaviour
{
    public int ID;
    public static int lastID;

    void Start()
    {
        lastID ++;
        ID = lastID;
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        ItemsManager.Manager.LoadChestItems(ID);
        FZPanelsManager.Manager.OpenPanel(ItemsManager.Manager.chestPanel);
    }
}
