using System;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    public Building[] buildings;
    ControlsMap controlsMap;
    int selectedBuildingID = 0;
    public CameraLogic mainCamera;
    Building selectedBuilding;
    public GameObject buildingsFlyout;

    public static BuildingsManager Manager;

    private void Awake()
    {
        Manager = this;
    }

    void Start()
    {
        Drill.Instance.DrillOnLand += DrillOnLand;
        Drill.Instance.DrillLeaveLand += DrillLeaveLand;

        controlsMap = new ControlsMap();

        controlsMap.OnLand.SelectBuildingMinus.performed += e => Minus();
        controlsMap.OnLand.SelectBuildingPlus.performed += e => Plus();
        controlsMap.OnLand.OpenPanel.performed += e => Open();
    }

    void Minus()
    {
        if (selectedBuildingID > 0)
        {
            buildings[selectedBuildingID].Highlight();
            selectedBuildingID--;
            SelectBuilding();
        }
    }

    void Plus()
    {
        if (selectedBuildingID < (Rocket.Instance.launchPad.activeSelf ? buildings.Length : buildings.Length - 1))
        {
            buildings[selectedBuildingID].UnHighlight();
            selectedBuildingID++;
            SelectBuilding();
        }
    }

    void SelectBuilding()
    {
        selectedBuilding = buildings[selectedBuildingID];
        mainCamera.building = selectedBuilding.transform;
        mainCamera.zoomOnBuilding = selectedBuilding.transform;
        selectedBuilding.Highlight();
    }

    void Open()
    {
        selectedBuilding.OpenBuildingPanel();
    }

    void DrillOnLand(object source, EventArgs eventArgs)
    {
        controlsMap.OnLand.Enable();
    }

    void DrillLeaveLand(object source, EventArgs eventArgs)
    {
        controlsMap.OnLand.Disable();
    }
}
