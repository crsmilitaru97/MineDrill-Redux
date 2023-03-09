using UnityEngine;

public class TeleportStation : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        FZAudio.Manager?.PlaySound(Sounds.Instance.teleport);
        Drill.Instance.dest = new Vector2(-6, 0);
        Drill.Instance.transform.position = new Vector2(-6, 0);
    }
}
