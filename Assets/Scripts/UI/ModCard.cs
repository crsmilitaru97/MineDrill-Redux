using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class ModCard : MonoBehaviour
{
    private ModType mod;

    public Image iconImage;
    public Text nameText;
    public Image borderImage;
    public GameObject lockGroup;

    public ModType Mod
    {
        get => mod; set
        {
            mod = value;

            iconImage.sprite = mod.sprite;
            nameText.text = mod.CD;
            borderImage.color = Color.red;
            lockGroup.SetActive(mod.locked);
        }
    }
}
