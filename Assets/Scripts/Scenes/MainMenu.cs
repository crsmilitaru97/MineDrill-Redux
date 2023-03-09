using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button loadButton, newButton;
    public EventSystem evSystem;

    void Start()
    {
        bool hasSave = FZSave.Bool.Get(FZSave.Constants.HasSave, true);
        loadButton.interactable = hasSave;
        evSystem.firstSelectedGameObject = hasSave ? loadButton.gameObject : newButton.gameObject;

        GameManager.Instance.CreateTerrain();
    }

    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            ExitGame();
        }
    }

    public void LoadGame()
    {
        GameManager.isNewGame = false;
        SceneManager.LoadScene(Constants.Scenes.World);
        //SceneManager.UnloadScene(Constants.Scenes.Menu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        GameManager.isNewGame = true;
        SceneManager.LoadScene(Constants.Scenes.Intro);
        //SceneManager.UnloadScene(Constants.Scenes.Menu);
    }
}