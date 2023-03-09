using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UIManager;

public class UIElement : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public string flyoutText;
    public bool canShowFlyout = true;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        UnSelect();
    }

    #region Helpers
    public void Select()
    {
        isMouseOverUI = true;

        if (!string.IsNullOrEmpty(flyoutText))
        {
            Manager.flyout.GetComponent<RectTransform>().position = new Vector2(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y - 45);
            Manager.flyout.GetComponentInChildren<Text>().text = flyoutText;

            Manager.flyout.SetActive(canShowFlyout);
        }
    }

    public void UnSelect()
    {
        isMouseOverUI = false;

        if (Manager.flyout != null)
        {
            Manager.flyout.SetActive(false);
        }
    }
    #endregion
}
