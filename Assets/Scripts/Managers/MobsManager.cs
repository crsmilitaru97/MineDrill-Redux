using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class MobsManager : MonoBehaviour
{
    //Fish, Spider, Worm
    public Key[] Keys;
    public Mob[] mobs;
    public FZButton[] keyButtons;
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public RectTransform canvas;
    public static int totalMob0, totalMob1, totalMob2;
    public GameObject mobLifeUI;

    bool isDrillInWater = false;
    bool isDrillOnLand = true;

    public static MobsManager Manager;

    private void Awake()
    {
        Manager = this;
    }

    void Start()
    {
        GetSave();
        StartCoroutine(Timer());
    }

    void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            foreach (var key in Keys)
            {
                key.quantity = FZSave.Int.Get(key.CD, 0);
            }
        }
        else
        {
            foreach (var key in Keys)
            {
                FZSave.Delete(key.CD);
            }
        }
        ShowKeysUI();
    }

    public void ArrivedOnLand()
    {
        isDrillOnLand = true;
    }

    public void LeaveLand()
    {
        isDrillOnLand = false;
    }

    public void EnterWater()
    {
        isDrillInWater = true;
        StartCoroutine(WaterTimer());
    }

    public void ExitWater()
    {
        isDrillInWater = false;
        StopCoroutine(WaterTimer());
    }

    private IEnumerator WaterTimer()
    {
        while (isDrillInWater)
        {
            yield return new WaitForSeconds(Random.Range(5, 20));
            if (isDrillInWater)
            {
                if (totalMob0 < 4)
                {
                    totalMob0++;
                    Instantiate(mobs[0], new Vector3(FZRandom.RandomBetween(-24, 24), Drill.Instance.transform.position.y + Random.Range(-5, 5), 0), Quaternion.identity);
                }
            }
        }
    }

    private IEnumerator Timer()
    {
        foreach (var spw in spawnPoints)
        {
            if (spw.mob == null)
            {
                spw.mob = Instantiate(mobs[1], spw.pos, Quaternion.identity);
            }
        }

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(120, 180));

            if (!isDrillInWater)
            {
                if (!isDrillOnLand)
                {
                    if (totalMob2 < 3 && FZRandom.Should(80))
                    {
                        totalMob2++;
                        var dest = Drill.Instance.transform.position;
                        dest.x = TerrainManager.sizeX + 5;
                        Mob worm = Instantiate(mobs[2], dest, Quaternion.identity).GetComponent<Mob>();
                        worm.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                }
            }

            foreach (var spw in spawnPoints)
            {
                if (spw.mob == null)
                {
                    for (int i = 0; i < Random.Range(1, 3); i++)
                    {
                        spw.mob = Instantiate(mobs[1], spw.pos, Quaternion.identity);
                    }
                }
            }
        }
    }

    public void AddKey(int ID)
    {
        Keys[ID].quantity++;
        ItemsManager.Manager.AddItemsUIGroup(new Item { sprite = Keys[ID].keySprite, quantity = 1 });
        FZSave.Int.Set(Keys[ID].CD, Keys[ID].quantity);
        ShowKeysUI();
    }

    public void RemoveKey(int ID)
    {
        Keys[ID].quantity--;
        FZSave.Int.Set(Keys[ID].CD, Keys[ID].quantity);
        ShowKeysUI();
    }

    void ShowKeysUI()
    {
        for (int i = 0; i < Manager.Keys.Length; i++)
        {
            keyButtons[i].buttonText.text = Manager.Keys[i].quantity.ToString();
            keyButtons[i].buttonImage.sprite = Manager.Keys[i].keySprite;
        }
    }
}
