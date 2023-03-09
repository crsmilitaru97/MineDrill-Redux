using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<int> MenuGrassBlockIDs = new List<int>();
    public Sprite[] grassSprites;

    public static bool isNewGame = true;
    int size = 42;

    public static GameManager Instance;
    private void Awake() => Instance = this;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        for (int x = -size; x <= size; x++)
        {
            int ID = Random.Range(0, grassSprites.Length);
            MenuGrassBlockIDs.Add(ID);
        }
    }

    public void CreateTerrain()
    {
        int i = 0;
        for (int x = -size; x <= size; x++)
        {
            var block = new GameObject();
            block.transform.position = new Vector2(x, -1);
            var sprRend = block.AddComponent<SpriteRenderer>();
            sprRend.sprite = grassSprites[MenuGrassBlockIDs[i]];
            sprRend.sortingOrder = 100;
            i++;
        }
    }
}
