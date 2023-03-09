using UnityEngine;
using UnityEngine.UI;
using static Constants;
using static ObjectivesManager;

public class ObjectiveUI : MonoBehaviour
{
    public Text titleText;
    public FZProgressBar progressBar;
    public Slot slot;
    public GameObject claimGroup;
    public GameObject disabledGroup;
    public GameObject lockedGroup;
    public Objective objective;

    public void ShowObjectiveData(Objective objective)
    {
        this.objective = objective;

        titleText.text = objective.title;
        slot.isViewOnly = true;
        slot.item = objective.item;
        progressBar.SetProgress(objective.maxValue, objective.currentValue);
        claimGroup.SetActive(objective.toBeClaimed && !objective.claimed);
        lockedGroup.SetActive(objective.locked);
        disabledGroup.SetActive(objective.claimed);
    }

    public void ClaimObjective()
    {
        Manager.ClaimObjective(objective);

        claimGroup.SetActive(false);
        disabledGroup.SetActive(true);
    }

    public void OpenObjectivePanel()
    {
        FZPanelsManager.Manager.OpenPanel(Manager.objectivesPanel);
    }
}
