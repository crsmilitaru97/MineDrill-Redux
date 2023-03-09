using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Constants;

public class ModsManager : MonoBehaviour
{
    public ModType[] Mods;

    [Header("Upgrades")]
    public Part enginePart;
    public Part cargoPart;
    public Part fuelTankPart;
    public Part drillBladePart;
    public Slot[] serviceUpgradeModSlots;

    [Header("Mods")]
    public GameObject modCardPrefab;
    public Transform modList;
    public static List<ModType> AssignedMods = new List<ModType>();

    public static ModsManager Manager;

    private void Awake()
    {
        Manager = this;
    }

    void Start()
    {
        GetSave();
        InstatiateMarketMods();
    }

    void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            enginePart.level = FZSave.Int.Get(enginePart.CD, 1);
            cargoPart.level = FZSave.Int.Get(cargoPart.CD, 1);
            fuelTankPart.level = FZSave.Int.Get(fuelTankPart.CD, 1);
            drillBladePart.level = FZSave.Int.Get(drillBladePart.CD, 1);

            enginePart.isMaxedOut = enginePart.level > enginePart.crafts1.Length;
            cargoPart.isMaxedOut = cargoPart.level > cargoPart.crafts1.Length;
            fuelTankPart.isMaxedOut = fuelTankPart.level > fuelTankPart.crafts1.Length;
            drillBladePart.isMaxedOut = drillBladePart.level > drillBladePart.crafts1.Length;

            List<string> AssignedModsCDs = FZSave.String.GetList("AssignedModsCDs");
            foreach (var modCD in AssignedModsCDs)
            {
                var mod = Mods.FirstOrDefault(e => e.CD == modCD);
                mod.assigned = true;
                AssignedMods.Add(mod);
            }
        }
        else
        {
            FZSave.Delete(enginePart.CD);
            FZSave.Delete(cargoPart.CD);
            FZSave.Delete(fuelTankPart.CD);
            FZSave.Delete(drillBladePart.CD);

            FZSave.Delete("AssignedModsCDs");
        }
    }

    void AddDrillPart(int i, Part part)
    {
        serviceUpgradeModSlots[i].part = part.Cloned();
        serviceUpgradeModSlots[i].RefreshSlotUI();
    }

    void InstatiateMarketMods()
    {
        AddDrillPart(0, enginePart);
        AddDrillPart(1, cargoPart);
        AddDrillPart(2, fuelTankPart);
        AddDrillPart(3, drillBladePart);

        Mods[0].locked = true;

        foreach (ModType mod in Mods)
        {
            var modCard = Instantiate(modCardPrefab, modList).GetComponent<ModCard>();
            modCard.Mod = mod;
        }

        List<string> AssignedModsCDs = new List<string>();
        foreach (var mod in AssignedMods)
        {
            AssignedModsCDs.Add(mod.CD);
        }
        FZSave.String.SetList("AssignedModsCDs", AssignedModsCDs);
        Refresh();
    }

    void Refresh()
    {
        //foreach (var modSlot in serviceModSlots)
        //{
        //    modSlot.RefreshSlotUI();
        //}
        foreach (var upgradeSlot in serviceUpgradeModSlots)
        {
            upgradeSlot.RefreshSlotUI();
        }
    }

    public void UpgradePart(Part part)
    {
        if (part.CD == enginePart.CD)
        {
            enginePart.level += 1;
            enginePart.isMaxedOut = enginePart.level > enginePart.crafts1.Length;
            FZSave.Int.Set(enginePart.CD, enginePart.level);
        }
        if (part.CD == cargoPart.CD)
        {
            cargoPart.level += 1;
            cargoPart.isMaxedOut = cargoPart.level > cargoPart.crafts1.Length;
            FZSave.Int.Set(cargoPart.CD, cargoPart.level);
        }
        if (part.CD == fuelTankPart.CD)
        {
            fuelTankPart.level += 1;
            fuelTankPart.isMaxedOut = fuelTankPart.level > fuelTankPart.crafts1.Length;
            FZSave.Int.Set(fuelTankPart.CD, fuelTankPart.level);
        }
        if (part.CD == drillBladePart.CD)
        {
            drillBladePart.level += 1;
            drillBladePart.isMaxedOut = drillBladePart.level > drillBladePart.crafts1.Length;
            FZSave.Int.Set(drillBladePart.CD, drillBladePart.level);
        }
        Drill.Instance.CheckModsAndParts();
        InstatiateMarketMods();
    }

    public void CraftMod(ModType craftedMod)
    {
        var mod = Mods.FirstOrDefault(e => e.CD == craftedMod.CD);
        mod.assigned = true;
        AssignedMods.Add(mod);
        Drill.Instance.CheckModsAndParts();
        InstatiateMarketMods();
    }
}
