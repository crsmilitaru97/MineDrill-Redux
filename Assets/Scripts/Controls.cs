using System;
using System.IO;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public FZPanel storagePanel;
    public FZPanel cargoPanel;
    ControlsMap controlsMap;
    public Slot[] powerupSlots;

    private void Start()
    {
        controlsMap = new ControlsMap();
        controlsMap.Anytime.Enable();
        controlsMap.Anytime.OpenInventory.performed += e => OpenInventory();
    }

    void OpenInventory()
    {
        if (Drill.isOnLand)
            FZPanelsManager.Manager.OpenPanel(storagePanel);
        else
            FZPanelsManager.Manager.OpenPanel(cargoPanel);
    }

    void Update()
    {
        if (Story.Instance.skipText.activeSelf)
        {
            Story.Instance.FinishTutorial();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\MineDrill Redux");
            string date = DateTime.Now.ToString();
            date = date.Replace("/", "-");
            date = date.Replace(" ", "_");
            date = date.Replace(":", "-");
            ScreenCapture.CaptureScreenshot(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\MineDrill Redux\\MDR " + date + ".jpg");
        }

        //Cheats
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log("Cheat: 100 coins");
                ItemsManager.Manager.ModifyCoins(100);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Cheat: Hit Drill");
                Drill.Instance.ModifyLifeAmount();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Cheat: Full");
                ItemsManager.Manager.FullTheStorage();
            }
        }
    }
}
