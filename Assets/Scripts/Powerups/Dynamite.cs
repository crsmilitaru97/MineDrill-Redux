using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    List<Block> BlocksToDestroy = new List<Block>();

    private void Start()
    {
        StartCoroutine(TimerTillExplosion());
    }

    private IEnumerator TimerTillExplosion()
    {
        int i = 0;
        while (i < 10)
        {
            yield return new WaitForSeconds(0.2f);
            transform.localScale = new Vector2(transform.localScale.x == 1 ? 0.9f : 1, transform.localScale.y == 1 ? 0.9f : 1);
            i++;
        }
        Camera.main.GetComponent<FZShake2DEffect>().TriggerShake(0.15f);
        GetComponent<SpriteRenderer>().enabled = false;

        Explode();
    }

    private void Explode()
    {
        FZAudio.Manager?.PlaySound(Sounds.Instance.explosion);

        Vector2 pos = transform.position;

        for (int y = (int)pos.y - 2; y <= pos.y + 2; y++)
        {
            for (int x = (int)pos.x - 3; x <= pos.x + 3; x++)
            {

                if (((x == pos.x - 3 && y == pos.y - 2) || (x == pos.x - 3 && y == pos.y + 2)
                    || (x == pos.x + 3 && y == pos.y - 2) || (x == pos.x + 3 && y == pos.y + 2)) == false)
                {
                    AddBlockToList(new Vector2(x, y));
                }
            }
        }
        StartCoroutine(DestroyAnim());
    }

    private void AddBlockToList(Vector2 pos)
    {
        Block block = TerrainManager.GetBlock(pos);
        if (block != null)
        {
            BlocksToDestroy.Add(block);
        }
    }

    private IEnumerator DestroyAnim()
    {
        int i = 10;
        while (i > 0)
        {
            yield return new WaitForSeconds(0.05f);
            foreach (var block in BlocksToDestroy)
            {
                block.transform.localScale = new Vector3(block.transform.localScale.x - 0.1f, block.transform.localScale.y - 0.1f, block.transform.localScale.z);
            }
            i--;
        }
        foreach (var block in BlocksToDestroy)
        {
            if (!string.IsNullOrEmpty(block.resourceToDrop.CD))
            {
                ItemsManager.Manager.Add_ToCargo(block.resourceToDrop);
            }
            TerrainManager.DestroyBlock(block.gameObject);
        }

        Destroy(this);
    }
}
