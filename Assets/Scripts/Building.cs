using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static BuildingsManager;

public class Building : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    public new string name;
    public FZPanel panel;

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenBuildingPanel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Drill.isOnLand)
        {
            Highlight();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnHighlight();
    }

    #region Helpers
    public void OpenBuildingPanel()
    {
        FZPanelsManager.Manager.OpenPanel(panel);
    }

    public void Highlight()
    {
        UIManager.isMouseOverUI = true;
        Manager.buildingsFlyout.GetComponentInChildren<Text>().text = name;
        Manager.buildingsFlyout.SetActive(true);

        GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color.ChangeAlpha(0.95f);
        transform.parent.localScale = new Vector2(1.025f, 1.025f);
        transform.localScale = new Vector2(0.975f, 0.975f);

    }

    public void UnHighlight()
    {
        UIManager.isMouseOverUI = false;
        Manager.buildingsFlyout.SetActive(false);

        GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color.ChangeAlpha(1);
        transform.parent.localScale = new Vector2(1f, 1f);
        transform.localScale = new Vector2(1f, 1f);
    }
    #endregion
}
