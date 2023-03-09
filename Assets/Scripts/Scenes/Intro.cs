using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Story;

public class Intro : MonoBehaviour
{
    public GameObject skipText;
    public Transform parachute;

    bool landed = false;

    void Start()
    {
        StartCoroutine(Timer());
        GameManager.Instance.CreateTerrain();
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(11);
        FZStory.TryShowMessagesWithDelay(100, 0, Messages.introStory[0]);
        yield return new WaitForSeconds(4);
        FZStory.TryShowMessagesWithDelay(100, 0, Messages.introStory[1]);
        yield return new WaitForSeconds(16);
        SceneManager.LoadSceneAsync(Constants.Scenes.World);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && skipText.activeSelf)
        {
            skipText.SetActive(false);
            SceneManager.LoadSceneAsync(Constants.Scenes.World);
        }

        if (parachute.position.y == 0.45f && !landed)
        {
            landed = true;
            FZStory.TryShowMessagesWithDelay(100, 0, Messages.introStory[2]);
        }
    }
}
