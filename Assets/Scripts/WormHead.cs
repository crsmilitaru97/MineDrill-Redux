using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MobsManager;
using static UnityEngine.UI.GridLayoutGroup;

public class WormHead : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite headSprite;
    public Sprite cornerSprite;
    public Sprite bodySprite;
    public Sprite tailSprite;
    public Vector2 dest;
    public WormPart prevPart;
    public static List<Vector2> poses = new List<Vector2>();
    public List<Transform> BodyParts = new List<Transform>();

    void Start()
    {
        dest = transform.position;
    }

    void Update()
    {
        //Move
        transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * 1f);

        //Arrive
        if (Vector3.Distance(transform.position, dest) == 0)
        {
            var newDest = GetNextPos(transform.position);
            if (dest != newDest)
                transform.rotation = GetRotation(Vector2Int.RoundToInt(dest), Vector2Int.RoundToInt(newDest));
            //if (prevPart != null)
            //    prevPart.ChangeDest(dest);
            //

            if (BodyParts.Count > 0)
            {
                BodyParts.Last().GetComponent<WormPart>().dest = dest;
                BodyParts.Insert(0, BodyParts.Last());
                BodyParts.RemoveAt(BodyParts.Count - 1);

                for (int i = 0; i < BodyParts.Count; i++)
                {
                    BodyParts[i].GetComponent<SpriteRenderer>().sprite = GetSprite(i - 1 < 0 ? BodyParts[i] : BodyParts[i - 1], BodyParts[i], i + 1 < BodyParts.Count() ? BodyParts[i + 1] : BodyParts[i]);
                }
            }

            BodyParts.First().GetComponent<SpriteRenderer>().sprite = bodySprite;
            BodyParts.Last().GetComponent<SpriteRenderer>().sprite = tailSprite;



            dest = newDest;

            // Save current position (gap will be here)

            // Do we have a Tail?

        }
    }

    Vector2 GetNextPos(Vector2 pos)
    {
        Vector2 nextPos = Vector2Int.RoundToInt(pos);

        nextPos = Vector2Int.RoundToInt(pos);
        if (FZRandom.Should(50))
            nextPos.x = pos.x + FZRandom.RandomBetween(GetX(pos, true), GetX(pos, false));
        else
            nextPos.y = pos.y + FZRandom.RandomBetween(pos.y > -TerrainManager.sizeY ? -1 : 1, pos.y < -3 ? 1 : -1);

        poses.Add(nextPos);
        return nextPos;
    }

    int GetX(Vector2 pos, bool minus)
    {
        if (minus)
        {
            if (pos.x > -TerrainManager.sizeX)
            {
                if (!BodyParts.Any(e => e.transform.position.x == pos.x - 1))
                    return -1;
                else
                    return 1;
            }
        }
        else
        {
            if (pos.x < TerrainManager.sizeX)
            {
                if (!BodyParts.Any(e => e.transform.position.x == pos.x + 1))
                    return 1;
                else
                    return -1;
            }
        }
        return -1;
    }

    Sprite GetSprite(Transform prev, Transform current, Transform next)
    {
        Vector2 posPrev = prev.position;
        Vector2 posNext = next.position;
        Vector2 dest = current.position;

        if (posPrev.x < dest.x && posNext.y < dest.y
         || posPrev.x > dest.x && posNext.y < dest.y
             || posPrev.x < dest.x && posNext.y > dest.y
             || posPrev.x > dest.x && posNext.y > dest.y)
        {
            return cornerSprite;

        }

        else
            return bodySprite;
    }

    Quaternion GetRotation(Vector2 pos, Vector2 nextPos)
    {
        if (nextPos.x > pos.x)
            return Quaternion.Euler(0, 0, -90);
        else if (nextPos.x < pos.x)
            return Quaternion.Euler(0, 0, 90);
        else if (nextPos.y < pos.y)
            return Quaternion.Euler(0, 0, 180);

        return Quaternion.identity;
    }
}