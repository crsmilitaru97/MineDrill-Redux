using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BlockShadow;
using static Drill;

public class Drill : MonoBehaviour
{
    public GameObject drillBlade;
    [Header("Fuel")]
    public Image fuelImage;
    public Text fuelText;
    public GameObject noFuelSign;
    [Header("Oxygen")]
    public GameObject oxygenGroup;
    public Image oxygenImage;
    public Text oxygenText;
    public GameObject noOxygenSign;
    [Header("Mods")]
    public GameObject[] lifeHearts;
    public GameObject shieldGroup;
    public Button shield;
    public Image shieldFill;
    public GameObject laserGun;
    public GameObject laserBeam;
    [Header("UI")]
    public Text depthText;
    public Image vignette;
    public GameObject hurt;
    [Header("Upgrades")]
    public float speed;
    public int fuelMax;
    public float drillSpeed;
    public int cargoMaxQuantity;
    [Header("Mods")]
    public bool isLaserMod = false;
    public bool isOxygenTankMod = false;
    public bool isShieldMod = false;
    [Header("Stats")]
    public float fuelAmount;
    public float oxygenAmount;
    public int drillLife;
    public FZPanel deathPanel;
    public FZPanel[] buildingsPanels;
    public GameObject pauseWhite;

    public Camera mainCamera;


    public static int maxLife = 3;
    public static int oxygenMax = 30;
    public static bool up, down, right, left;
    public static bool canMove = true, itsMoving, itsDrilling;
    public static bool isOnLand = true;

    public static Vector3Int roundPos;
    public Vector2 dest;
    public static int drillingDirection = 0;
    bool noFuel, noOxygen;
    bool isShieldLoaded;

    public static Drill Instance;

    private void Awake()
    {
        Instance = this;
        direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
    }

    void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            foreach (var lf in lifeHearts)
            {
                lf.SetActive(false);
            }
            drillLife = FZSave.Int.Get("DrillLife", 3);
            for (int i = 0; i < drillLife; i++)
            {
                lifeHearts[i].SetActive(true);
            }
        }
        else
        {
            RegenerateAllLifes();
        }
    }

    public void RegenerateAllLifes()
    {
        drillLife = 3;
        FZSave.Delete("DrillLife");
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            lifeHearts[i].SetActive(true);
        }
        Service.Instance.damageLevelText.text = Math.Round(((float)drillLife / (float)maxLife) * 100).ToString() + "%";
        Service.Instance.repairButton.SetButton(0, false);
    }

    void Start()
    {
        CheckModsAndParts();
        ChangeFuelAmount(fuelMax);
        ChangeOxygenAmount(oxygenMax);
        dest = transform.position;
        vignette.gameObject.SetActive(true);

        GetSave();
        Service.Instance.repairButton.SetButton((maxLife - drillLife) * 50, drillLife != maxLife);

        if (DrillOnLand != null)
            DrillOnLand(this, null);
    }

    public void CheckModsAndParts()
    {
        speed = 1.5f + ModsManager.Manager.enginePart.level * 0.5f;
        fuelMax = ModsManager.Manager.fuelTankPart.level * 25;
        drillSpeed = ModsManager.Manager.drillBladePart.level * 10;
        cargoMaxQuantity = ModsManager.Manager.cargoPart.level * 10;

        isShieldMod = ModsManager.AssignedMods.Any(e => e.CD == "Shield");
        isLaserMod = ModsManager.AssignedMods.Any(e => e.CD == "Laser Gun");
        isOxygenTankMod = ModsManager.AssignedMods.Any(e => e.CD == "Oxygen Tank");

        shieldGroup.gameObject.SetActive(isShieldMod);
        StartCoroutine(ShieldReloadTimer());
        laserGun.transform.parent.gameObject.SetActive(isLaserMod);
        oxygenMax = isOxygenTankMod ? 60 : 30;

        ItemsManager.Manager.cargoMaxText.text = cargoMaxQuantity.ToString();
    }

    Vector3 direction;

    private void LateUpdate()
    {
        vignette.color = vignette.color.ChangeAlpha(transform.position.y < 0 ? Mathf.Abs(transform.position.y) / 10f : 0);

        if (dest.y == 0 && isOnLand == false)
        {
            ArrivedOnLand();
        }
        else if (dest.y < 0)
        {
            if (isOnLand)
                LeaveLand();

            if (FZMath.IsBetween((int)dest.y, TerrainManager.waterStart, TerrainManager.waterStart - 50))
                EnterWater();
            else
                ExitWater();

            depthText.text = (roundPos.y * 10).ToString() + "m";
        }
    }

    void Update()
    {
        up = Input.GetAxis("Vertical") > 0;
        down = Input.GetAxis("Vertical") < 0;
        left = Input.GetAxis("Horizontal") < 0;
        right = Input.GetAxis("Horizontal") > 0;

        if (isLaserMod)
        {
            Vector3 direction = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Quaternion rotation = Quaternion.AngleAxis(FZMath.AngleBetweenTwoPoints(transform.position, direction), Vector3.forward);
            Transform lgTran = laserGun.transform;
            lgTran.rotation = Quaternion.Slerp(lgTran.rotation, rotation, 1);

            if (Input.GetMouseButtonDown(0) && !UIManager.isMouseOverUI && !FZPanelsManager.isPanelActive)
            {
                FZAudio.Manager?.PlaySound(Sounds.Instance.laserShoot);
                Bullet beam = Instantiate(laserBeam, new Vector3(lgTran.position.x, lgTran.position.y, 0), lgTran.rotation).GetComponent<Bullet>();
                beam.dest = new Vector3(direction.x, direction.y, 0);
            }
        }

        roundPos = Vector3Int.RoundToInt(transform.position);

        if (canMove)
        {
            if (Vector3.Distance(transform.position, dest) == 0)
            {
                itsMoving = false;

                if (up && !isOnLand && TerrainManager.GetBlock(new Vector2(roundPos.x, roundPos.y + 1)) == null)
                {
                    direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
                    drillingDirection = 0;
                    dest.y = roundPos.y + 1;
                }
                if (down)
                {
                    direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                    drillingDirection = 1;
                    dest.y = roundPos.y - 1;
                }
                if (right && transform.position.x < TerrainManager.sizeX)
                {
                    direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90);
                    drillingDirection = 2;
                    dest.x = roundPos.x + 1;
                }
                if (left && transform.position.x > -TerrainManager.sizeX)
                {
                    direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90);
                    drillingDirection = 3;
                    dest.x = roundPos.x - 1;
                }
            }
            else
            {
                if (!itsMoving)
                {
                    if (TerrainManager.Terrain.ContainsKey(dest))
                    {
                        if (TerrainManager.Terrain[dest].maxLife < 100)
                        {
                            if (noFuel)
                            {
                                itsMoving = false;
                                dest = Vector2Int.RoundToInt(transform.position);
                            }
                            else
                            {
                                itsMoving = true;
                                DrillBlock(dest);
                            }
                        }
                        else
                        {
                            itsMoving = false;
                            dest = Vector2Int.RoundToInt(transform.position);
                        }
                    }
                    else
                    {
                        itsMoving = true;
                    }
                }

            }
            Quaternion targetRotation = Quaternion.Euler(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
        }

        if (itsMoving && !itsDrilling)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * speed);
        }
    }

    void DrillBlock(Vector3 dest)
    {
        SetTransparencyAround();

        itsDrilling = true;
        drillBlade.GetComponent<Animator>().SetBool("Drilling", true);
        ChangeFuelAmount(fuelAmount - 1);
        var block = TerrainManager.GetBlock(dest);
        CheckModsAndParts();
        speed = drillSpeed * block.maxLife;
        block.BeginDrilling();
        //transparency.Remove(block);
    }

    public void ModifyLifeAmount()
    {
        if (isShieldMod && isShieldLoaded)
        {
            hurt.GetComponent<Image>().color = FZColors.ChangeAlpha(Color.gray, 0.5f);
            StartCoroutine(ShieldReloadTimer());
        }
        else
        {
            hurt.GetComponent<Image>().color = FZColors.ChangeAlpha(Color.red, 0.5f);


            drillLife--;
            lifeHearts[drillLife].SetActive(false);

            if (drillLife == 0)
            {
                FZPanelsManager.Manager.OpenFixedPanel(deathPanel);
                pauseWhite.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                FZStory.TryShowMessagesWithDelay(15, 0.25f, Story.Messages.hurt);
            }
            Service.Instance.damageLevelText.text = Math.Round(((float)drillLife / (float)maxLife) * 100).ToString() + "%";
            Service.Instance.repairButton.SetButton((maxLife - drillLife) * 50, drillLife != maxLife);

            FZSave.Int.Set("DrillLife", drillLife);
        }

        hurt.SetActive(true);
        FZAudio.Manager?.PlaySound(Sounds.Instance.hurt);
        if (drillLife != 0)
        {
            Camera.main.GetComponent<FZShake2DEffect>().TriggerShake(0.1f);
        }
    }

    bool firstNoFuel = true;
    public void ChangeFuelAmount(float fuelAmountVal)
    {
        fuelAmount = fuelAmountVal;
        fuelImage.fillAmount = fuelAmount / fuelMax;
        fuelText.text = fuelAmount.ToString();

        noFuel = fuelAmount == 0;
        noFuelSign.SetActive(noFuel);
        Service.Instance.fillButton.interactable = fuelAmount < fuelMax;
        if (noFuel)
            FZStory.TryShowMessagesWithDelay(firstNoFuel ? 100 : 10, 0.5f, Story.Messages.noFuel);
        firstNoFuel = false;
    }

    public void ChangeOxygenAmount(float oxygenAmountVal)
    {
        oxygenAmount = oxygenAmountVal;
        if (oxygenAmount < 0)
        {
            Debug.Log("Oxygen level - " + oxygenAmount);
            if (oxygenAmount == -1 || oxygenAmount == -6 || oxygenAmount == -11)
                ModifyLifeAmount();
        }
        else
        {
            oxygenImage.fillAmount = oxygenAmount / oxygenMax;
            oxygenText.text = oxygenAmount.ToString() + "s";
        }
        noOxygen = oxygenAmount <= 0;
        noOxygenSign.SetActive(noOxygen);
    }

    List<BlockShadow> shadows = new List<BlockShadow>();
    void SetTransparencyAround()
    {
        foreach (BlockShadow shadow in shadows)
        {
            shadow.ModifyBack();
        }
        shadows.Clear();

        GetShadow((Vector3)roundPos)?.Modify(-4);

        GetShadow(new Vector3(roundPos.x + 1, roundPos.y, roundPos.z))?.Modify(-4);
        GetShadow(new Vector3(roundPos.x - 1, roundPos.y, roundPos.z))?.Modify(-4);
        GetShadow(new Vector3(roundPos.x, roundPos.y + 1, roundPos.z))?.Modify(-4);
        GetShadow(new Vector3(roundPos.x, roundPos.y - 1, roundPos.z))?.Modify(-4);

        GetShadow(new Vector3(roundPos.x + 1, roundPos.y + 1, roundPos.z))?.Modify(-3);
        GetShadow(new Vector3(roundPos.x + 1, roundPos.y - 1, roundPos.z))?.Modify(-3);
        GetShadow(new Vector3(roundPos.x - 1, roundPos.y + 1, roundPos.z))?.Modify(-3);
        GetShadow(new Vector3(roundPos.x - 1, roundPos.y - 1, roundPos.z))?.Modify(-3);

        GetShadow(new Vector3(roundPos.x + 2, roundPos.y, roundPos.z))?.Modify(-2);
        GetShadow(new Vector3(roundPos.x - 2, roundPos.y, roundPos.z))?.Modify(-2);
        GetShadow(new Vector3(roundPos.x, roundPos.y + 2, roundPos.z))?.Modify(-2);
        GetShadow(new Vector3(roundPos.x, roundPos.y - 2, roundPos.z))?.Modify(-2);

        GetShadow(new Vector3(roundPos.x + 2, roundPos.y + 1, roundPos.z))?.Modify(-1);
        GetShadow(new Vector3(roundPos.x + 2, roundPos.y - 1, roundPos.z))?.Modify(-1);
        GetShadow(new Vector3(roundPos.x - 2, roundPos.y + 1, roundPos.z))?.Modify(-1);
        GetShadow(new Vector3(roundPos.x - 2, roundPos.y - 1, roundPos.z))?.Modify(-1);
        GetShadow(new Vector3(roundPos.x + 1, roundPos.y + 2, roundPos.z))?.Modify(-1);
        GetShadow(new Vector3(roundPos.x - 1, roundPos.y + 2, roundPos.z))?.Modify(-1);
        GetShadow(new Vector3(roundPos.x + 1, roundPos.y - 2, roundPos.z))?.Modify(-1);
        GetShadow(new Vector3(roundPos.x - 1, roundPos.y - 2, roundPos.z))?.Modify(-1);
    }

    BlockShadow GetShadow(Vector3 pos)
    {
        if (TerrainManager.Terrain.ContainsKey(pos))
        {
            var shadow = TerrainManager.Terrain[pos].shadow;
            if (shadow.canLight)
            {
                shadows.Add(shadow);
                return shadow;
            }
        }

        return null;
    }


    #region Deep Events     
    public delegate void DrillOnLandEventHandler(object source, EventArgs args);
    public event DrillOnLandEventHandler DrillOnLand;
    void ArrivedOnLand()
    {
        isOnLand = true;
        depthText.gameObject.SetActive(!isOnLand);
        ItemsManager.Manager.ArriveOnLand();
        direction = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

        FZStory.TryShowMessagesWithDelay(10, 0.25f, Story.Messages.arriveOnland);

        MobsManager.Manager.ArrivedOnLand();
        foreach (var panel in buildingsPanels)
        {
            panel.canOpen = true;
        }

        if (DrillOnLand != null)
            DrillOnLand(this, null);
    }

    public delegate void DrillLeaveLandEventHandler(object source, EventArgs args);
    public event DrillLeaveLandEventHandler DrillLeaveLand;
    void LeaveLand()
    {
        isOnLand = false;
        depthText.gameObject.SetActive(!isOnLand);
        ItemsManager.Manager.LeaveLand();
        ChangeOxygenAmount(oxygenMax);

        FZStory.TryShowMessagesWithDelay(10, 0.5f, Story.Messages.leaveLand);

        MobsManager.Manager.LeaveLand();
        foreach (var panel in buildingsPanels)
        {
            panel.canOpen = false;
        }
        if (DrillLeaveLand != null)
            DrillLeaveLand(this, null);
    }

    void EnterWater()
    {
        if (isConsumingOxygen)
            return;
        speed = speed - 1f;
        FZAudio.Manager?.PlaySound(Sounds.Instance.enterWater);
        isConsumingOxygen = true;
        oxygenGroup.SetActive(true);
        StartCoroutine(ConsumeOxygen());
        MobsManager.Manager.EnterWater();
        FZStory.TryShowMessagesWithDelay(10, 0.25f, Story.Messages.enterWater);
    }

    void ExitWater()
    {
        if (!isConsumingOxygen)
            return;
        speed = speed + 1f;
        FZAudio.Manager?.PlaySound(Sounds.Instance.enterWater);
        ChangeOxygenAmount(oxygenMax);
        isConsumingOxygen = false;
        oxygenGroup.SetActive(false);
        MobsManager.Manager.ExitWater();
    }
    #endregion

    private bool isConsumingOxygen;
    private IEnumerator ConsumeOxygen()
    {
        while (isConsumingOxygen)
        {
            yield return new WaitForSeconds(1);
            ChangeOxygenAmount(oxygenAmount - 1);
        }
    }

    private IEnumerator ShieldReloadTimer()
    {
        isShieldLoaded = false;
        shield.interactable = false;
        shieldFill.fillAmount = 0;
        while (!isShieldLoaded)
        {
            yield return new WaitForSeconds(2);
            shieldFill.fillAmount += 0.1f;

            if (shieldFill.fillAmount == 1)
            {
                isShieldLoaded = true;
                shield.interactable = true;
            }
        }
    }

    public void Revive()
    {
        dest = new Vector2(-6, 0);
        Instance.transform.position = new Vector2(-6, 0);
        vignette.color = vignette.color.ChangeAlpha(0);
        pauseWhite.SetActive(false);
        Time.timeScale = 1;
        FZPanelsManager.Manager.CloseAllPanels();
        Service.Instance.RepairDrill();
        FZStory.TryShowMessagesWithDelay(100, 0.25f, Story.Messages.revive);
    }
}
