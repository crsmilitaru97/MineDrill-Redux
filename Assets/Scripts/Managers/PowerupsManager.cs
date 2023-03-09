using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Constants;

public class PowerupsManager : MonoBehaviour
{
    public Powerup[] powerups;
    public GameObject placingPrefab;
    public GameObject lightSource;

    public Slot[] bottomPowerupSlots;
    public Slot[] marketPowerupSlots;

    ControlsMap controlsMap;

    public static List<Powerup> BuyedPowerups = new List<Powerup>();
    public static List<Vector2> PlacedLights = new List<Vector2>();

    public static PowerupsManager Manager;

    private void Awake()
    {
        Manager = this;
    }

    private void Start()
    {
        GetSave();
        InstatiateMarketPowerups();

        controlsMap = new ControlsMap();
        controlsMap.Powerups.Enable();
        controlsMap.Powerups.Powerup1.performed += e => UsePowerup(bottomPowerupSlots[0]);
        controlsMap.Powerups.Powerup2.performed += e => UsePowerup(bottomPowerupSlots[1]);
        controlsMap.Powerups.Powerup3.performed += e => UsePowerup(bottomPowerupSlots[2]);
        controlsMap.Powerups.Powerup4.performed += e => UsePowerup(bottomPowerupSlots[3]);
    }

    void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            var powerups = FZSave.Dictionary.GetStringIntPair("Powerups");
            foreach (var pwpsave in powerups)
            {
                var pwp = GetPowerupFromCD(pwpsave.Key);
                pwp.quantity = pwpsave.Value;
                BuyedPowerups.Add(pwp);
                ShowPowerupsOnUI();
            }

            PlacedLights = FZSave.Vectors.GetVector2Array("PlacedLights").ToList();
            foreach (var lightPos in PlacedLights)
            {
                Instantiate(lightSource, lightPos, Quaternion.identity);
            }
        }
        else
        {
            FZSave.Delete("Powerups");
            FZSave.Delete("PlacedLights");
        }
    }

    public void UsePowerup(Slot slot)
    {
        //slot.SelectSlot();
        if (slot.powerup.needsPlacing)
        {
            GameObject obj = Instantiate(placingPrefab);
            var po = obj.GetComponent<PlaceObject>();
            po.prefab = slot.powerup.prefab;
            obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = slot.powerup.sprite;
        }
        else
        {
            if (slot.powerup.CD == "Fuel Can")
            {
                FZAudio.Manager?.PlaySound(Sounds.Instance.fuelCan);
                Service.Instance.FillTank();
                RemoveFromBuyedPowerupsList(slot.powerup);
                SlotsManager.Manager.selectedSlot = null;
            }

            if (slot.powerup.CD == "Boost")
            {
                FZAudio.Manager?.PlaySound(Sounds.Instance.fuelCan);
                Service.Instance.FillTank();
                RemoveFromBuyedPowerupsList(slot.powerup);
                SlotsManager.Manager.selectedSlot = null;
            }
        }
    }

    void InstatiateMarketPowerups()
    {
        for (int i = 0; i < marketPowerupSlots.Length; i++)
        {
            marketPowerupSlots[i].powerup = powerups[i].Cloned();
            marketPowerupSlots[i].RefreshSlotUI();
        }
    }

    public void AddToBuyedPowerupsList(Powerup powerup)
    {
        Debug.Log("Add: " + powerup.CD);

        Powerup already = BuyedPowerups.Find(e => e.CD == powerup.CD);
        if (already != null)
        {
            already.quantity += 1;
        }
        else
        {
            powerup.quantity = 1;
            BuyedPowerups.Add(powerup);
        }
        SavePowerups();
        ShowPowerupsOnUI();
    }
    public void RemoveFromBuyedPowerupsList(Powerup powerup)
    {
        Powerup already = BuyedPowerups.Find(e => e.CD == powerup.CD);
        already.quantity -= 1;
        if (already.quantity <= 0)
        {
            BuyedPowerups.Remove(already);
        }
        SavePowerups();
        ShowPowerupsOnUI();
    }

    void ShowPowerupsOnUI()
    {
        for (int i = 0; i < bottomPowerupSlots.Length; i++)
        {
            if (i < BuyedPowerups.Count)
            {
                bottomPowerupSlots[i].powerup = BuyedPowerups[i].Cloned();
            }
            else
            {
                bottomPowerupSlots[i].powerup.CD = null;
                bottomPowerupSlots[i].powerup.quantity = 0;
            }
            bottomPowerupSlots[i].RefreshSlotUI();
        }
    }

    void SavePowerups()
    {
        Dictionary<string, int> powerups = new Dictionary<string, int>();
        foreach (var pwp in BuyedPowerups)
        {
            powerups.Add(pwp.CD, pwp.quantity);
        }
        FZSave.Dictionary.SetStringIntPair("Powerups", powerups);
    }

    public static Powerup GetPowerupFromCD(string CD)
    {
        return Manager.powerups.FirstOrDefault(e => e.CD == CD);
    }
}
