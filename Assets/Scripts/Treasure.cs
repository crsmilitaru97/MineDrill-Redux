using UnityEngine;

public class Treasure : MonoBehaviour
{
    public int keyHoleID;
    public bool unlocked;

    private void Start()
    {
        keyHoleID = Random.Range(0, MobsManager.Manager.Keys.Length);
    }

    public void OnMouseDown()
    {
        if (!TreasureManager.Instance.treasurePanel.opened)
        {
            TreasureManager.Instance.selectedTreasure = this;
            TreasureManager.Instance.OpenTreasure();
        }
    }
}
