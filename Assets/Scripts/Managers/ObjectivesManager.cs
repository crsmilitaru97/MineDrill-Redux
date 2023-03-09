using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class ObjectivesManager : MonoBehaviour
{
    public List<Objective> Objectives = new List<Objective>();
    [Header("level")]
    public Text[] levelTexts;
    public FZProgressBar levelProgressBar;
    [Header("Objectives UI")]
    public Transform scrollView;
    public Text totalObjectivesText;
    public FZPanel objectivesPanel;
    [Header("Unlocks")]
    public GameObject[] buildingsAddonsUI;
    public GameObject[] locksUI;

    [Header("Other")]
    public GameObject objectivePrefab;
    public Transform canvas;

    public static ObjectiveUI onScreenObjectiveUI;

    public static int level;
    public static int levelProgress;
    public static int levelProgressMax = 100;
    public static int unlockedNumber = 0;
    List<ObjectiveUI> ObjectivesUI = new List<ObjectiveUI>();

    public static ObjectivesManager Manager;

    private void Awake()
    {
        Manager = this;
    }

    void Start()
    {
        UnlockObjective(Objectives[0]);
        GetSave();
        foreach (var objective in Objectives)
        {
            var objectiveUI = Instantiate(objectivePrefab, scrollView).GetComponent<ObjectiveUI>();
            objectiveUI.objective = objective;
            objectiveUI.GetComponent<Button>().interactable = false;
            ObjectivesUI.Add(objectiveUI);
        }
        RefreshObjectives();
    }

    void GetSave()
    {
        if (!GameManager.isNewGame)
        {
            levelProgress = FZSave.Int.Get("LevelProgress", 0);
            levelProgressBar.SetProgress(levelProgressMax, levelProgress);

            level = FZSave.Int.Get(FZSave.Constants.Level, 1);
            foreach (var levelText in levelTexts)
            {
                levelText.text = level.ToString();
            }

            foreach (var addon in buildingsAddonsUI)
            {
                addon.gameObject.SetActive(false);
            }

            foreach (var objective in Objectives)
            {
                objective.currentValue = FZSave.Int.Get("Objective" + objective.CD + "Current", 0);
                objective.claimed = FZSave.Bool.Get("Objective" + objective.CD + "Claimed", false);
                objective.toBeClaimed = FZSave.Bool.Get("Objective" + objective.CD + "ToBeClaimed", false);
                objective.locked = FZSave.Bool.Get("Objective" + objective.CD + "Locked", true);
            }
        }
        else
        {
            level = 1;
            FZSave.Delete(FZSave.Constants.Level);
            levelProgressBar.SetProgress(levelProgressMax, 0);

            foreach (var levelText in levelTexts)
            {
                levelText.text = level.ToString();
            }

            foreach (var addon in buildingsAddonsUI)
            {
                addon.gameObject.SetActive(false);
            }

            foreach (var objective in Objectives)
            {
                FZSave.Delete("Objective" + objective.CD + "Current");
                FZSave.Delete("Objective" + objective.CD + "Claimed");
                FZSave.Delete("Objective" + objective.CD + "ToBeClaimed");
                FZSave.Delete("Objective" + objective.CD + "Locked");
            }
        }
    }

    void ShowObjectiveOnScreen(int ID)
    {
        var objective = Objectives[ID];
        onScreenObjectiveUI = Instantiate(objectivePrefab, canvas).GetComponent<ObjectiveUI>();
        onScreenObjectiveUI.ShowObjectiveData(objective);
        onScreenObjectiveUI.gameObject.AddComponent<FZShadow>();
        if (!objective.toBeClaimed)
            StartCoroutine(Timer(onScreenObjectiveUI.gameObject));
    }

    void RefreshObjectives()
    {
        totalObjectivesText.text = unlockedNumber.ToString() + "/" + Objectives.Count.ToString();

        foreach (var objectiveUI in ObjectivesUI)
        {
            objectiveUI.ShowObjectiveData(objectiveUI.objective);
        }
    }

    public void UpdateObjective(int ID, int plusValue)
    {
        var objective = Objectives[ID];

        if (objective.locked)
        {
            return;
        }

        objective.currentValue += plusValue;
        if (objective.currentValue == objective.maxValue)
        {
            objective.toBeClaimed = true;
            FZSave.Bool.Set("Objective" + objective.CD + "ToBeClaimed", objective.toBeClaimed);
        }
        ShowObjectiveOnScreen(ID);
        RefreshObjectives();
    }

    public void UnlockObjective(Objective objective)
    {
        objective.locked = false;
        FZSave.Bool.Set("Objective" + objective.CD + "Locked", objective.locked);
        RefreshObjectives();
    }

    public void ClaimObjective(Objective objective)
    {
        unlockedNumber++;
        objective.toBeClaimed = false;
        objective.claimed = true;
        FZSave.Bool.Set("Objective" + objective.CD + "Claimed", objective.claimed);

        levelProgress += 25;
        if (levelProgress == levelProgressMax)
        {
            UnlockNewLevel();
        }
        levelProgressBar.SetProgress(levelProgressMax, levelProgress);
        FZSave.Int.Set("LevelProgress", levelProgress);
        FZSave.Int.Set(FZSave.Constants.Level, level);

        foreach (var obj in Objectives)
        {
            if (obj.unlockNbr <= unlockedNumber)
                UnlockObjective(obj);
        }
        RefreshObjectives();
        Destroy(onScreenObjectiveUI.gameObject);
    }

    private IEnumerator Timer(GameObject objectiveUI)
    {
        yield return new WaitForSeconds(3);
        Destroy(objectiveUI);
    }

    void UnlockNewLevel()
    {
        level++;
        foreach (var levelText in levelTexts)
        {
            levelText.text = level.ToString();
        }
        locksUI[level - 2].SetActive(true);
        levelProgress = 0;
    }
}