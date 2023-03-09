using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject flyoutPrefab;
    public GameObject flyout;

    public static bool isMouseOverUI;
    public Transform canvas;
    public Color goldColor;

    public static UIManager Manager;

    private void Awake()
    {
        Manager = this;
    }

    void Start()
    {
        flyout = Instantiate(Manager.flyoutPrefab, Manager.canvas);
        flyout.SetActive(false);
    }
}
