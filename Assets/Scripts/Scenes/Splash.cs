using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class Splash : MonoBehaviour
{
    public float time;

    private void Start()
    {
        GameManager.isNewGame = FZSave.Bool.Get(FZSave.Constants.IsFirstPlay, true);
        if (GameManager.isNewGame)
        {
            FZSave.Bool.Set(FZSave.Constants.IsFirstPlay, false);
            StartCoroutine(LoadSplash(Scenes.Intro));
        }
        else
        {
            StartCoroutine(LoadSplash(Scenes.Menu));
        }
        GameManager.Instance.CreateTerrain();
    }

    private IEnumerator LoadSplash(int sceneID)
    {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadSceneAsync(sceneID);
    }
}
