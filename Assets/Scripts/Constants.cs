using System;
using UnityEngine;

public static class Constants
{
    public static bool IsNotEmpty(this Item item)
    {
        return !string.IsNullOrEmpty(item.CD);
    }

    public static Item GetRecipeItem(this Recipe recipe)
    {
        var item = ItemsManager.GetItemFromCD(recipe.CD);
        item.quantity = recipe.quantity;
        return item;
    }

    [Serializable]
    public class Key
    {
        public int ID;
        public string CD;
        public Sprite keySprite;
        public int quantity;
        public Sprite holeSprite;
    }

    [Serializable]
    public class SpawnPoint
    {
        public Vector2 pos;
        public Mob mob;
    }

    [Serializable]
    public class BlockType
    {
        [HideInInspector]
        public int ID;
        public int strength;
        public DestroyableSprite[] destroyables;
        public Item resource;
    }

    [Serializable]
    public class DestroyableSprite
    {
        public Sprite sprite;
        public Sprite[] parts;
    }

    [Serializable]
    public class Recipe
    {
        public string CD;
        public int quantity = 1;
    }

    [Serializable]
    public class Item
    {
        public string CD;
        public int price;
        [Multiline]
        public string description;
        public Sprite sprite;
        public Recipe craft1;
        public Recipe craft2;
        public bool burned = false;
        public int quantity = 1;
        public bool canDuplicate = true;
    }

    [Serializable]
    public class RocketPart
    {
        public string CD;
        public int ID;
        public Sprite sprite;
        public Recipe[] recipes;
        public bool isCrafted = false;
    }

    [Serializable]
    public class Objective
    {
        public string title;
        public string CD;
        public Item item;
        public int maxValue;
        public int currentValue;
        public bool locked = true;
        public bool toBeClaimed = false;
        public bool claimed = false;
        public int unlockNbr;
    }

    [Serializable]
    public class BuildItem
    {
        public string CD;
        [Multiline]
        public string description;
        public int price;
        public Sprite sprite;
        public GameObject prefab;
    }

    [Serializable]
    public class Achievement
    {
        public string title;
        [Multiline]
        public string description;
        public string ID;
    }

    [Serializable]
    public class Powerup
    {
        public string CD;
        public int price;
        [Multiline]
        public string description;
        public Sprite sprite;
        public bool needsPlacing;
        public GameObject prefab;

        public int quantity = 0;
    }

    [Serializable]
    public class Part
    {
        public string CD;
        public string description;
        public Sprite sprite;
        public Recipe[] crafts1;
        public Recipe[] crafts2;

        internal int level = 1;
        internal bool isMaxedOut = false;
    }

    [Serializable]
    public class ModType
    {
        public string CD;
        public string description;
        public Sprite sprite;
        public Recipe craft1;
        public Recipe craft2;
        public Recipe craft3;

        internal bool locked = true;
        internal bool crafted = false;
        internal bool assigned = false;
    }

    #region Clone
    public static Item Cloned(this Item item)
    {
        return new Item
        {
            CD = item.CD,
            price = item.price,
            description = item.description,
            sprite = item.sprite,
            craft1 = item.craft1,
            craft2 = item.craft2,
            quantity = item.quantity,
            burned = item.burned,
            canDuplicate = item.canDuplicate
        };
    }

    public static Powerup Cloned(this Powerup powerup)
    {
        return new Powerup
        {
            CD = powerup.CD,
            price = powerup.price,
            sprite = powerup.sprite,
            description = powerup.description,
            quantity = powerup.quantity,
            needsPlacing = powerup.needsPlacing,
            prefab = powerup.prefab
        };
    }
    public static RocketPart Cloned(this RocketPart rocketPart)
    {
        return new RocketPart
        {
            CD = rocketPart.CD,
            ID = rocketPart.ID,
            sprite = rocketPart.sprite,
            recipes = rocketPart.recipes,
            isCrafted = rocketPart.isCrafted
        };
    }

    public static Part Cloned(this Part part)
    {
        return new Part
        {
            CD = part.CD,
            level = part.level,
            description = part.description,
            crafts1 = part.crafts1,
            crafts2 = part.crafts2,
            sprite = part.sprite,
            isMaxedOut = part.isMaxedOut
        };
    }
    #endregion

    public static class Scenes
    {
        public const int Splash = 0;
        public const int Menu = 1;
        public const int Intro = 2;
        public const int World = 3;
    }
}
