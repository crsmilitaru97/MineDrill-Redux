using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Constants;

public class Block : MonoBehaviour
{
    public int ID;
    public Item resourceToDrop;
    public int maxLife = 10;
    public int currentLife = 10;
    public bool ignorable;
    public DestroyableSprite destroyable;
    public BlockShadow shadow;

    
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = destroyable.sprite;
    }

    public void BeginDrilling()
    {
        LoadDestroyableParts();
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(DrillTimer());
    }

    List<GameObject> blockParts = new List<GameObject>();
    void LoadDestroyableParts()
    {
        int i = 0;
        for (float y = 0.375f; y >= -0.375f; y -= 0.250f)
        {
            for (float x = -0.375f; x <= 0.375f; x += 0.250f)
            {
                GameObject part = new GameObject();
                part.transform.parent = transform;
                part.transform.localPosition = new Vector2((float)x, (float)y);
                blockParts.Add(part);

                var spr = part.AddComponent<SpriteRenderer>();
                spr.sortingOrder = 11;
                spr.sprite = destroyable.parts[i];
                i++;
            }
        }
    }

    public IEnumerator DrillTimer()
    {
        FZAudio.Manager?.PlaySound(Sounds.Instance.digIn.RandomItem());

        int c;
        float speed = maxLife / Drill.Instance.drillSpeed / 10;

        if (Drill.drillingDirection == 1)
        {
            c = 1;
            while (c < 14)
            {
                int i = 10;
                while (i > 0)
                {
                    yield return new WaitForSecondsRealtime(speed);
                    ChangePartSize(c);
                    ChangePartSize(c + 1);
                    i -= 1;
                }

                i = 10;
                while (i > 0)
                {
                    yield return new WaitForSecondsRealtime(speed);
                    ChangePartSize(c - 1);
                    ChangePartSize(c + 2);
                    i -= 1;
                }

                c += 4;
            }
        }
        if (Drill.drillingDirection == 2)
        {
            c = 4;
            while (c < 8)
            {
                int i = 10;
                while (i > 0)
                {
                    yield return new WaitForSecondsRealtime(speed);
                    ChangePartSize(c);
                    ChangePartSize(c + 4);
                    i -= 1;
                }

                i = 10;
                while (i > 0)
                {
                    yield return new WaitForSecondsRealtime(speed);
                    ChangePartSize(c - 4);
                    ChangePartSize(c + 8);
                    i -= 1;
                }

                c += 1;
            }
        }
        if (Drill.drillingDirection == 3)
        {
            c = 7;
            while (c > 3)
            {
                int i = 10;
                while (i > 0)
                {
                    yield return new WaitForSecondsRealtime(speed);
                    ChangePartSize(c);
                    ChangePartSize(c + 4);
                    i -= 1;
                }

                i = 10;
                while (i > 0)
                {
                    yield return new WaitForSecondsRealtime(speed);
                    ChangePartSize(c - 4);
                    ChangePartSize(c + 8);
                    i -= 1;
                }

                c -= 1;
            }
        }
        FinishDrilling();
    }

    void FinishDrilling()
    {
        FZAudio.Manager?.PlaySound(Sounds.Instance.digOut.RandomItem());

        Drill.itsDrilling = false;

        if (!string.IsNullOrEmpty(resourceToDrop.CD))
        {
            if (ItemsManager.totalCargoItems + resourceToDrop.quantity <= Drill.Instance.cargoMaxQuantity)
            {
                FZStory.TryShowMessagesWithDelay(5, 0.2f, Story.Messages.oreUseful);
                if (resourceToDrop.CD == "Scrap")
                {
                    var scrapItems = ItemsManager.Manager.Items.Where(e => e.CD == "Rubber" || e.CD == "Plastic").ToArray();
                    ItemsManager.Manager.Add_ToCargo(scrapItems[Random.Range(0, scrapItems.Length)]);
                    ObjectivesManager.Manager.UpdateObjective(0, 1);
                }
                else
                {
                    ItemsManager.Manager.Add_ToCargo(resourceToDrop);
                }
            }
            else
            {
                FZStory.TryShowMessagesWithDelay(100, 0, Story.Messages.cargoFull);
            }

        }
        foreach (var item in blockParts)
        {
            Destroy(item);
        }
        Drill.Instance.CheckModsAndParts();
        Drill.Instance.drillBlade.GetComponent<Animator>().SetBool("Drilling", false);
        TerrainManager.DestroyBlock(gameObject);
    }

    void AnimateBlade()
    {
        var blade = Drill.Instance.transform.GetChild(1);
        if (blade.localScale.x == 0.15f)
            blade.localScale = new Vector3(-0.15f, blade.localScale.y, blade.localScale.z);

        else
            blade.localScale = new Vector3(0.15f, blade.localScale.y, blade.localScale.z);
    }

    void ChangePartSize(int c)
    {
        AnimateBlade();
        if (Drill.drillingDirection == 1)
            Drill.Instance.transform.position = new Vector3(Drill.Instance.transform.position.x, Drill.Instance.transform.position.y - 1f / (8 * 20f), Drill.Instance.transform.position.z);
        if (Drill.drillingDirection == 2)
            Drill.Instance.transform.position = new Vector3(Drill.Instance.transform.position.x + 1f / (8 * 20f), Drill.Instance.transform.position.y, Drill.Instance.transform.position.z); if (Drill.drillingDirection == 3)
            Drill.Instance.transform.position = new Vector3(Drill.Instance.transform.position.x - 1f / (8 * 20f), Drill.Instance.transform.position.y, Drill.Instance.transform.position.z);
        if (transform.localScale.x - 0.1f < 0.1f)
            Destroy(blockParts[c]);
        else
            blockParts[c].transform.localScale = new Vector2(blockParts[c].transform.localScale.x - 0.1f, blockParts[c].transform.localScale.y - 0.1f);
    }
}
