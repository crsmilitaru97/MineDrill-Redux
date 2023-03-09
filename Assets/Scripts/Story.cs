using System.Collections;
using UnityEngine;

public class Story : MonoBehaviour
{
    public GameObject bottomBar, topBar;
    public GameObject rocketLaunchParticles;
    public Animator rocketAnim;
    public GameObject finalGroup;
    public GameObject cosmosGroup;
    public GameObject[] tutorials;
    public GameObject skipText;
    public GameObject tutorialGroup;

    public static bool tutorial;

    public static Story Instance;
    private void Awake() => Instance = this;

    private void Start()
    {
        GetSave();

        if (tutorial)
        {
            FZSave.Bool.Set(FZSave.Constants.HasSave, false);
            CameraLogic.canMove = false;
            tutorialGroup.SetActive(true);
            FZStory.ShowMessageOnce(new FZStory.Message { text = Messages.startStory });
            StartCoroutine(TutorialTimer());
        }
    }

    void GetSave()
    {
        if (GameManager.isNewGame)
        {
            tutorial = FZSave.Bool.Get(FZSave.Constants.HasSave, true);
        }
        else
            FZSave.Delete(FZSave.Constants.HasSave);
    }

    public static class Messages
    {
        public static string[] introStory = { "Aaaaaaaaaaaaaaa!", "Our rocket broke down and now we are crashing down the earth!", "Good landing!  So glad we survived. " };


        public static string startStory = "Nice surroundings! Let's see what we got here.";
        public static string afterTutorial = "Now let's start drilling. Maybe I will find some resources to rebuild my rocket.";
        public static string[] oreUseful = { "This ore might be useful later.", "I'm keeping that!", "Another ore added to my cargo." };
        public static string[] enterWater = { "Splash!" };
        public static string[] revive = { "I'm alive?! That was so strange... Maybe it was all a dream." };
        public static string[] noFuel = { "No fuel remained... I should get back on land to the Service to fill up." };
        public static string[] leaveLand = { "Wish me luck this time!", "I'm going on an adventure again!" };
        public static string[] arriveOnland = { "Finally! I see the sun light again.", "Now let's see what we can do with the ores.", "Maybe now we can craft some items." };
        public static string[] hurt = { "Ouch! That hurt!" };
        public static string[] rocket1part = { "Yes! The rocket is taking shape." };
        public static string[] rocket2part = { "One more part and we are ready to launch. So excited!" };
        public static string[] rocket3part = { "Finally! It's time to fly back home." };
        public static string cargoFull = "I don't have enough space for that item. Cargo is full!";
        public static string finalStory = "It was a nice adventure but now it's time to fly back home. Goodbye Earth!";


        public static string[] tutorial = { "First lets learn how to move. Press this keys to control the drill.",
                                            "This is your inventory space where you can find your collected items. Cargo can deposit up to 10 items.",
                                            "This are the drill indicators. Fuel is consumed when drilling blocks and oxygen when staying underwater.",
                                            "Here you can acces the powerups buyed from the Store.",
                                            "Coins earned from selling items are displayed here. You can use them to buy powerups or duplicate items.",
                                            "There are a lot of buildings around here. You can click on them to learn more about their use." };
    }

    public IEnumerator LaunchTimer()
    {
        cosmosGroup.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        bottomBar.SetActive(false);
        topBar.SetActive(false);
        yield return new WaitForSecondsRealtime(1);
        FZStory.TryShowMessagesWithDelay(100, 1f, Messages.finalStory);
        yield return new WaitForSecondsRealtime(4);
        rocketLaunchParticles.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        rocketAnim.SetTrigger("Launch");
        yield return new WaitForSecondsRealtime(10);
        finalGroup.SetActive(true);
    }

    public IEnumerator TutorialTimer()
    {
        yield return new WaitForSeconds(5);

        for (int i = 0; i < tutorials.Length; i++)
        {
            tutorials[i].SetActive(true);
            FZStory.TryShowMessagesWithDelay(100, 0, Messages.tutorial[i]);
            yield return new WaitForSeconds(6);
            tutorials[i].SetActive(false);
        }
        FinishTutorial();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && skipText.activeSelf)
        {
            FinishTutorial();
        }
    }

    public void FinishTutorial()
    {
        skipText.SetActive(false);
        StopAllCoroutines();
        foreach (var tut in tutorials)
        {
            tut.SetActive(false);
        }
        tutorialGroup.SetActive(false);
        FZStory.TryShowMessagesWithDelay(100, 1f, Messages.afterTutorial);

        CameraLogic.canMove = true;
    }
}
