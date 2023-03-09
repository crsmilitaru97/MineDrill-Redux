using UnityEngine;

public class Landscape : MonoBehaviour
{
    public bool followMouse;

    void Update()
    {
        if (Camera.main == null)
            return;

        if (followMouse)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x / 10f, transform.position.y, transform.position.z), Time.deltaTime);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Camera.main.transform.position.x / 3f, transform.position.y, transform.position.z), Time.deltaTime);

    }
}
