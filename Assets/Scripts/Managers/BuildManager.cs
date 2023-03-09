using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class BuildManager : MonoBehaviour
{
    public List<BuildItem> BuildItems = new List<BuildItem>();
    public FZButton[] buildSlots;
    public GameObject buildGroup;
    public BuildItem selectedItem;
    public GameObject placingPrefab;

    void Start()
    {
        for (int i = 0; i < BuildItems.Count; i++)
        {
            buildSlots[i].buttonImage.sprite = BuildItems[i].sprite;
            buildSlots[i].GetComponent<UIElement>().flyoutText = BuildItems[i].CD;
        }
    }

    public void SelectBuildItem(int ID)
    {
        selectedItem = BuildItems[ID];

        GameObject obj = Instantiate(placingPrefab);
        var po = obj.GetComponent<PlaceObject>();
        po.prefab = selectedItem.prefab;
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = selectedItem.sprite;
        buildGroup.SetActive(true);
    }
}
