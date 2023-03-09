using UnityEngine;

public class LandscapeElement : MonoBehaviour
{
    [SerializeField]
    Color springColor;

    [SerializeField]
    Color summerColor;

    [SerializeField]
    Color autumnColor;

    [Header("Winter")]
    [SerializeField]
    Color winterColor;
    [SerializeField]
    Sprite winterSprite; 
    [SerializeField]
    bool winterDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        if (winterDestroy)
        {
            Destroy(gameObject);
            return;
        }

        GetComponent<SpriteRenderer>().color = winterColor;
        if (winterSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = winterSprite;
            if (GetComponent<FZATree2D>() !=null)
            GetComponent<FZATree2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
