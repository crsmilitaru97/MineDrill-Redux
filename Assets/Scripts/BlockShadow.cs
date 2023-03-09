using UnityEngine;

public class BlockShadow : MonoBehaviour
{
    public int shadowLevel = 5;
    public Block block;
    public bool canLight = true;
    int prevLevel;

    public static class LightLevel
    {
        public const int fullLight = 1;
        public const int intense = 2;
        public const int medium = 3;
        public const int low = 4;
        public const int veryLow = 4;
        public const int noLight = 5;
    }

    public void SetNew(int level)
    {
        prevLevel = shadowLevel;
        shadowLevel = level;

        SetUp();
    }

    public void Modify(int level)
    {
        prevLevel = shadowLevel;

        if (shadowLevel + level > 1)
            SetNew(shadowLevel + level);
        else
            SetNew(LightLevel.fullLight);
    }


    public void ModifyBack()
    {
        SetNew(prevLevel);
    }

    void SetUp()
    {
        var color = GetComponent<SpriteRenderer>().color;
        color.a = 1;
        if (shadowLevel == LightLevel.fullLight)
            color.a = !block.ignorable ? 0 : 0f;
        if (shadowLevel == LightLevel.intense)
            color.a = !block.ignorable ? 0.2f : 0f;
        if (shadowLevel == LightLevel.medium)
            color.a = !block.ignorable ? 0.4f : 0.2f;
        if (shadowLevel == LightLevel.low)
            color.a = !block.ignorable ? 0.6f : 0.4f;
        if (shadowLevel == LightLevel.veryLow)
            color.a = !block.ignorable ? 0.8f : 0.6f;
        if (shadowLevel == LightLevel.noLight)
            color.a = !block.ignorable ? 1 : 0.8f;


        GetComponent<SpriteRenderer>().color = color;
    }
}
