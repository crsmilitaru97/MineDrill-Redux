using UnityEngine;

public class WormPart : MonoBehaviour
{
    public Vector2 dest;
    public WormPart nextPart;
    public WormPart prevPart;
    public float speed = 1;
    public Sprite corner;
    public Sprite body;

    void Start()
    {
        dest = transform.position;
    }

    void Update()
    {
        //Move
        //transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * speed);
        transform.position = dest;

        //Arrive
        //if (Vector3.Distance(transform.position, dest) == 0)
        //{
        //    if (prevPart != null)
        //        prevPart.ChangeDest(dest);
        //}
    }

    //public void ChangeDest(Vector2 newDest)
    //{
    //    if (prevPart != null)
    //        prevPart.dest = dest;

    //    dest = newDest;
    //    transform.rotation = GetRotation(transform.position, dest);
    //    GetComponent<SpriteRenderer>().sprite = GetSprite();
    //}


    //Quaternion GetRotation(Vector2 pos, Vector2 nextPos)
    //{
    //    if (nextPos.x > pos.x)
    //        return Quaternion.Euler(0, 0, -90);
    //    else if (nextPos.x < pos.x)
    //        return Quaternion.Euler(0, 0, 90);
    //    else if (nextPos.y < pos.y)
    //        return Quaternion.Euler(0, 0, 180);

    //    return Quaternion.identity;
    //}

}