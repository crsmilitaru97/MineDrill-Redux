using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //        0 - ARRR!
        //1 - It's just the beginning
        //2 - Gone deeper
        //3 - Insanely deep
        //4 - Not much left now
        //5 - Extremely advanced
        //6 - Evolved
        //7 - Oil found
        //8 - 100 items crafted
        //10 - Green energy
        //11 - Plumber
        //12 - Don't be stingy
        //13 - Traveler
        //16 - Light up the darkness
        //17 - K - BOOM!
        //18 - Resource collector
        //        Find your first treasure.
        //Reach 100 meters in depth.
        //Reach 500 meters in depth.
        //Reach 1000 meters in depth.
        //Reach 2000 meters in depth.
        //Unlock all the advancements.
        //Fully upgrade all the machine specs.
        //Find oil and pump it to the surface.
        //Craft 100 items.
        //Build solar panels and use the electric engine.
        //Create 5 pipes.
        //Achieve a 1000c balance.
        //Move 2000 squares.
        //Place 10 lights.
        //Place a dynamite and wait for the explosion.
        //Mine a number of 500 blocks.
    }

    public static void CheckDepthAch(int y)
    {
        if (y == 20)
            Claim(1);
        if (y == 40)
            Claim(2);
        if (y == 60)
            Claim(3);
    }

    public static void Claim(int i)
    {

    }
}
