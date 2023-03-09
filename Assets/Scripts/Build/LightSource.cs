using System.Linq;
using UnityEngine;
using static BlockShadow;

public class LightSource : MonoBehaviour
{
    [Range(1, 4)]
    public int lightIntensity = 4;
    void Start()
    {
        Vector2 pos = transform.position;
        LightAround(pos);

        var PlacedLights = FZSave.Vectors.GetVector2Array("PlacedLights").ToList();
        if (!PlacedLights.Contains(pos))
        {
            PowerupsManager.PlacedLights.Add(new Vector2(transform.position.x, transform.position.y));
            FZSave.Vectors.SetVector2Array("PlacedLights", PowerupsManager.PlacedLights.ToArray());
        }
    }

    void LightAround(Vector2 pos)
    {
        LightPos(pos, LightLevel.fullLight);

        LightPos(new Vector2(pos.x, pos.y + 1), LightLevel.fullLight);
        LightPos(new Vector2(pos.x, pos.y - 1), LightLevel.fullLight);
        LightPos(new Vector2(pos.x + 1, pos.y), LightLevel.fullLight);
        LightPos(new Vector2(pos.x - 1, pos.y), LightLevel.fullLight);

        if (lightIntensity < 2)
            return;

        LightPos(new Vector2(pos.x - 2, pos.y), LightLevel.intense);
        LightPos(new Vector2(pos.x + 2, pos.y), LightLevel.intense);
        LightPos(new Vector2(pos.x - 1, pos.y + 1), LightLevel.intense);
        LightPos(new Vector2(pos.x - 1, pos.y - 1), LightLevel.intense);
        LightPos(new Vector2(pos.x + 1, pos.y + 1), LightLevel.intense);
        LightPos(new Vector2(pos.x + 1, pos.y - 1), LightLevel.intense);
        LightPos(new Vector2(pos.x, pos.y + 2), LightLevel.intense);
        LightPos(new Vector2(pos.x, pos.y - 2), LightLevel.intense);

        if (lightIntensity < 3)
            return;

        LightPos(new Vector2(pos.x - 3, pos.y), LightLevel.medium);
        LightPos(new Vector2(pos.x + 3, pos.y), LightLevel.medium);
        LightPos(new Vector2(pos.x - 1, pos.y + 2), LightLevel.medium);
        LightPos(new Vector2(pos.x - 1, pos.y - 2), LightLevel.medium);
        LightPos(new Vector2(pos.x + 1, pos.y + 2), LightLevel.medium);
        LightPos(new Vector2(pos.x + 1, pos.y - 2), LightLevel.medium);
        LightPos(new Vector2(pos.x - 2, pos.y + 1), LightLevel.medium);
        LightPos(new Vector2(pos.x - 2, pos.y - 1), LightLevel.medium);
        LightPos(new Vector2(pos.x + 2, pos.y + 1), LightLevel.medium);
        LightPos(new Vector2(pos.x + 2, pos.y - 1), LightLevel.medium);
        LightPos(new Vector2(pos.x, pos.y + 3), LightLevel.medium);
        LightPos(new Vector2(pos.x, pos.y - 3), LightLevel.medium);

        if (lightIntensity < 4)
            return;

        LightPos(new Vector2(pos.x - 4, pos.y), LightLevel.low);
        LightPos(new Vector2(pos.x + 4, pos.y), LightLevel.low);
        LightPos(new Vector2(pos.x - 1, pos.y + 3), LightLevel.low);
        LightPos(new Vector2(pos.x - 1, pos.y - 3), LightLevel.low);
        LightPos(new Vector2(pos.x + 1, pos.y + 3), LightLevel.low);
        LightPos(new Vector2(pos.x + 1, pos.y - 3), LightLevel.low);
        LightPos(new Vector2(pos.x - 2, pos.y + 2), LightLevel.low);
        LightPos(new Vector2(pos.x - 2, pos.y - 2), LightLevel.low);
        LightPos(new Vector2(pos.x + 2, pos.y + 2), LightLevel.low);
        LightPos(new Vector2(pos.x + 2, pos.y - 2), LightLevel.low);
        LightPos(new Vector2(pos.x - 3, pos.y + 1), LightLevel.low);
        LightPos(new Vector2(pos.x - 3, pos.y - 1), LightLevel.low);
        LightPos(new Vector2(pos.x + 3, pos.y + 1), LightLevel.low);
        LightPos(new Vector2(pos.x + 3, pos.y - 1), LightLevel.low);
        LightPos(new Vector2(pos.x, pos.y + 4), LightLevel.low);
        LightPos(new Vector2(pos.x, pos.y - 4), LightLevel.low);
    }

    void LightPos(Vector2 pos, int lightLevel)
    {
        var shadow = TerrainManager.GetBlock(pos).shadow;
        if (shadow != null)
        {
            shadow.SetNew(lightLevel);
        }
    }
}
