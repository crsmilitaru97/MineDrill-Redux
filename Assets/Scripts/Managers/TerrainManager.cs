using System.Collections.Generic;
using UnityEngine;
using static Block;
using static BlockShadow;
using static Constants;

public class TerrainManager : MonoBehaviour
{
    public Transform terrainTransform;

    public GameObject torchLight;
    public GameObject treasure;
    public GameObject[] ruins;
    public GameObject defaultBlock;
    public GameObject waterLevel;
    [Header("Tiles")]
    public Sprite seaweed;
    public Sprite[] caveTileSprites;
    [Header("Blocks")]
    public BlockType Grass;
    public BlockType Dirt;
    public BlockType HardDirt;
    public BlockType Rock;
    public BlockType HardRock;
    public BlockType Scrap;
    public BlockType Coal;
    public BlockType Sand;
    public BlockType Gold;
    public BlockType Copper;
    public BlockType Iron;
    public BlockType Bauxite;
    public BlockType Manganese;
    public BlockType Diamond;
    public BlockType Titanium;
    public BlockType Ruby;
    public BlockType Lava;
    public BlockType Roots;
    [Header("Special Destroyables")]
    public DestroyableSprite[] winterGrass;

    public static Dictionary<Vector2, int> TerrainBP = new Dictionary<Vector2, int>();
    public static Dictionary<Vector2, Block> Terrain = new Dictionary<Vector2, Block>();
    public static Dictionary<Vector2, int> ObjectsBP = new Dictionary<Vector2, int>();

    public static int sizeX = 21;
    public static int sizeY = 250;
    int part = sizeY / 5;

    public static int waterStart;
    enum ObjectsID { Treasure, Ruin, WallLight, TorchLight };
    enum TilesID { Cave = -1, Wall = -2 };

    BlockType[] blocks;
    public static TerrainManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        blocks = new BlockType[] { Grass, Dirt, HardDirt, Rock, HardRock, Scrap, Coal, Sand, Gold, Copper, Iron, Bauxite, Manganese, Diamond, Titanium, Ruby, Lava, Roots };

        //Add terrain resources to items list
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].ID = i;
            if (blocks[i].resource.IsNotEmpty())
            {
                ItemsManager.Manager.Items.Add(blocks[i].resource.Cloned());
            }
        }
    }

    void Start()
    {
        GetSave();
    }

    void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            TerrainBP = FZSave.Dictionary.GetVector2IntPair(FZSave.Constants.Terrain);
            TransformBP();
        }
        else
        {
            GenerateTerrain();
            FZSave.Dictionary.SetVector2IntPair(FZSave.Constants.Terrain, TerrainBP);
        }
    }

    void GenerateTerrain()
    {
        TerrainBP.Clear();

        GenerateMargins();
        GenerateResources();
        GenerateCaves();

        TransformBP();

        FZSave.Bool.Set(FZSave.Constants.HasSave, true);
    }

    #region Generate
    void GenerateMargins()
    {
        //First layers
        for (int x = -sizeX; x <= sizeX; x++)
        {
            TerrainBP.Add(new Vector2(x, -1), Grass.ID);
        }

        int extra = 22;
        //Left
        for (int x = -sizeX - 1; x > -sizeX - extra; x--)
        {
            TerrainBP.Add(new Vector2(x, -1), Grass.ID);
            for (int y = -2; y > -5; y--)
            {
                TerrainBP.Add(new Vector2(x, y), Dirt.ID);
            }
        }

        //Right
        for (int x = sizeX + 1; x < sizeX + extra; x++)
        {
            TerrainBP.Add(new Vector2(x, -1), Grass.ID);
            for (int y = -2; y > -5; y--)
            {
                TerrainBP.Add(new Vector2(x, y), Dirt.ID);
            }
        }

        //Bottom
        for (int x = -sizeX - extra; x < sizeX + extra; x++)
        {
            TerrainBP.Add(new Vector2(x, -sizeY - 1), Lava.ID);
            TerrainBP.Add(new Vector2(x, -sizeY - 2), Lava.ID);
            TerrainBP.Add(new Vector2(x, -sizeY - 3), Lava.ID);
        }
    }

    void GenerateCaves()
    {
        for (float y = 0.5f; y < 4.5f; y += 0.5f)
        {
            if (y != 2 && y != 2.5f && y != 3) //Water level
            {
                CreateCave(new Vector2(Random.Range(-sizeX + 5, sizeX - 5), Random.Range(Mathf.RoundToInt(-y * part), Mathf.RoundToInt(-(y + 0.5f) * part))));
            }
        }
    }

    void GenerateResources()
    {
        LoadResource(new Zone(2, 4, Dirt, Dirt), new Resource(Scrap.ID, 5), new Resource(Roots.ID, 10));
        LoadResource(new Zone(4, part, Dirt, Rock), new Resource(Scrap.ID, 35), new Resource(Iron.ID, 40), new Resource(Coal.ID, 50));
        LoadResource(new Zone(part, 2 * part, Dirt, Rock), new Resource(Iron.ID, 50), new Resource(Manganese.ID, 70), new Resource(Coal.ID, 50));
        LoadResource(new Zone(2 * part, 3 * part, HardDirt, Sand), new Resource(Gold.ID, 60), new Resource(Copper.ID, 70)); //Water level
        LoadResource(new Zone(3 * part, 4 * part, HardRock, HardRock), new Resource(Diamond.ID, 40), new Resource(Bauxite.ID, 70));
        LoadResource(new Zone(4 * part, 5 * part, HardRock, HardRock), new Resource(Ruby.ID, 40), new Resource(Titanium.ID, 70));
    }
    #endregion


    #region Classes
    class Resource
    {
        public int ID;
        public int number;

        public Resource(int ID, int number)
        {
            this.ID = ID;
            this.number = number;
        }
    }

    class Zone
    {
        public int firstY;
        public int lastY;
        public BlockType filler;
        public BlockType fillerPlus;

        public Zone(int firstY, int lastY, BlockType filler, BlockType fillerPlus)
        {
            this.firstY = firstY;
            this.lastY = lastY;
            this.filler = filler;
            this.fillerPlus = fillerPlus;
        }
    }
    #endregion

    #region Helpers
    void LoadResource(Zone zone, Resource res1, Resource res2, Resource res3 = null)
    {
        List<Vector2> blocksAvailableInZone = new List<Vector2>();

        for (int y = -zone.firstY; y > -zone.lastY; y--)
        {
            for (int x = -sizeX; x <= sizeX; x++)
            {
                blocksAvailableInZone.Add(new Vector2(x, y));
            }
        }

        for (int i = 0; i < res1.number; i++)
        {
            Vector2 pos = blocksAvailableInZone.RandomItem();
            while (!blocksAvailableInZone.Contains(pos))
            {
                pos = blocksAvailableInZone.RandomItem();
            }
            blocksAvailableInZone.Remove(pos);
            TerrainBP.Add(pos, res1.ID);
        }
        for (int i = 0; i < res2.number; i++)
        {
            Vector2 pos = blocksAvailableInZone.RandomItem();
            while (!blocksAvailableInZone.Contains(pos))
            {
                pos = blocksAvailableInZone.RandomItem();
            }
            blocksAvailableInZone.Remove(pos);
            TerrainBP.Add(pos, res2.ID);
        }
        if (res3 != null)
        {
            for (int i = 0; i < res3.number; i++)
            {
                Vector2 pos = blocksAvailableInZone.RandomItem();
                while (!blocksAvailableInZone.Contains(pos))
                {
                    pos = blocksAvailableInZone.RandomItem();
                }
                blocksAvailableInZone.Remove(pos);
                TerrainBP.Add(pos, res3.ID);
            }
        }

        foreach (var remainedPos in blocksAvailableInZone)
        {
            LoadFiller(zone, remainedPos);
        }
    }

    void LoadFiller(Zone zone, Vector2 pos)
    {
        if (FZRandom.Should(5))
            TerrainBP.Add(pos, zone.fillerPlus.ID);
        else
            TerrainBP.Add(pos, zone.filler.ID);
    }

    void CreateCave(Vector2 pos)
    {
        int rowXLeft = (int)Random.Range(pos.x - 5, pos.x - 2);
        int rowXRight = (int)Random.Range(pos.x + 2, pos.x + 5);
        for (int i = rowXLeft; i < rowXRight; i++)
        {
            if (TerrainBP.ContainsKey(new Vector3(i, pos.y + 1, 0)))
                TerrainBP[new Vector3(i, pos.y + 1, 0)] = (int)TilesID.Cave;
        }

        int rowXLeft2 = (int)Random.Range(pos.x - 6, pos.x - 3);
        int rowXRight2 = (int)Random.Range(pos.x + 3, pos.x + 6);
        for (int i = rowXLeft2; i < rowXRight2; i++)
        {
            if (TerrainBP.ContainsKey(new Vector3(i, pos.y, 0)))
                TerrainBP[new Vector3(i, pos.y, 0)] = (int)TilesID.Cave;
        }

        int rowXLeft3 = (int)Random.Range(pos.x - 7, pos.x - 4);
        int rowXRight3 = (int)Random.Range(pos.x + 4, pos.x + 7);
        for (int i = rowXLeft3; i < rowXRight3; i++)
        {
            if (TerrainBP.ContainsKey(new Vector3(i, pos.y - 1, 0)))
                TerrainBP[new Vector3(i, pos.y - 1, 0)] = (int)TilesID.Cave;
        }

        // 1 Treasure
        var treasurePos = new Vector2(rowXLeft3 + Random.Range(0, rowXRight3 - rowXLeft3 - 1), pos.y - 1);
        ObjectsBP.Add(treasurePos, (int)ObjectsID.Treasure);

        //Ruins
        var ruinPos = treasurePos;
        while (ruinPos == treasurePos)
        {
            ruinPos = new Vector2(rowXLeft3 + Random.Range(0, rowXRight3 - rowXLeft3 - 1), pos.y - 1);
        }
        ObjectsBP.Add(new Vector2(ruinPos.x, pos.y - 1), (int)ObjectsID.Ruin);

        // 1 Light in center
        Instantiate(torchLight, pos, Quaternion.identity);
        ObjectsBP.Add(pos, (int)ObjectsID.TorchLight);
    }

    void TransformBP()
    {
        int part = sizeY / 5;
        waterStart = -2 * part;
        Terrain.Clear();
        foreach (var BP in ObjectsBP)
        {
            if (BP.Value == (int)ObjectsID.Treasure) // Treasure
            {
                Instantiate(treasure, BP.Key, Quaternion.identity);
                continue;
            }
            if (BP.Value == (int)ObjectsID.Ruin) // Ruins and spawnpoint
            {
                Instantiate(ruins.RandomItem(), BP.Key, Quaternion.identity);
                MobsManager.Manager.spawnPoints.Add(new SpawnPoint { pos = BP.Key });
                continue;
            }
        }
        foreach (var BP in TerrainBP)
        {
            GameObject obj = Instantiate(defaultBlock, BP.Key, Quaternion.identity, terrainTransform);
            Block block = obj.GetComponent<Block>();

            GetBlockDataFromBP(block, BP.Value);
            if (BP.Key.x < -sizeX || BP.Key.x > sizeX)
                block.shadow.canLight = false;
            //Terrain.Clear();
            Terrain.Add(BP.Key, block);

            if (BP.Key.y >= -5)
                block.shadow.SetNew(Mathf.Abs((int)BP.Key.y));
            if (BP.Key.y == -sizeY - 1)
                block.shadow.SetNew(LightLevel.fullLight);
            if (BP.Key.y == -sizeY - 2)
                block.shadow.SetNew(LightLevel.fullLight);
            if (BP.Key.y == -sizeY - 3)
                block.shadow.SetNew(LightLevel.intense);

        }
        Instantiate(waterLevel, new Vector3(0, waterStart + 0.5f, 1), Quaternion.identity);
    }

    void GetBlockDataFromBP(Block block, int ID)
    {
        block.ID = ID;
        if (ID < 0)
        {
            if (ID == (int)TilesID.Cave)
                block.destroyable.sprite = caveTileSprites.RandomItem();
            //if (ID == (int)TilesID.Wall)
            //    block.destroyable.sprite = wallTileSprites[FZRandom.GetFromList(wallTileSprites.Length)];

            block.ignorable = true;
            return;
        }
        else
        {
            if (ID == Grass.ID)
            {
                block.destroyable = winterGrass.RandomItem();
            }
            else
            block.destroyable = blocks[ID].destroyables.RandomItem();
        }
        block.ignorable = false;
        block.maxLife = blocks[ID].strength;
        block.resourceToDrop = blocks[ID].resource;
    }
    #endregion

    #region Static
    public static Block GetBlock(Vector3 pos)
    {
        if (Terrain.ContainsKey(pos))
        {
            return Terrain[pos];
        }

        return null;
    }

    public static void DestroyBlock(GameObject block)
    {
        if (block == null)
            return;

        //TerrainBP.Remove(block.transform.position);
        Terrain[block.transform.position].ignorable=true;
        Terrain[block.transform.position].GetComponent<SpriteRenderer>().enabled = false;

        //Destroy(block);

        FZSave.Dictionary.SetVector2IntPair(FZSave.Constants.Terrain, TerrainBP);
    }
    #endregion
}
