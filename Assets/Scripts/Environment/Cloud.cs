using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 destinationPosition;

    float speed;
    bool directionSwitch = false;

    void Start()
    {
        speed = Random.Range(0.1f, 0.3f);

        Initiate();
    }

    void LateUpdate()
    {
        if (Drill.roundPos.y > -10)
        {
            if (transform.position.x == destinationPosition.x)
            {
                directionSwitch = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (transform.position.x == startPosition.x)
            {
                directionSwitch = false;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if (directionSwitch)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPosition.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(destinationPosition.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
            }
        }
    }

    private void Initiate()
    {
        if (FZRandom.Should(10))
        {
            Destroy(gameObject);
        }
        var size = Random.Range(0.8f, 1.1f);
        transform.localScale = new Vector3(size, Random.Range(size - 0.1f, size + 0.1f), 1);

        if (FZRandom.Should(40))
        {
            directionSwitch = true;
        }

        if (FZRandom.Should(40))
        {
            var otherCloud = transform.parent.GetChild(Random.Range(2, transform.childCount));
            var oldPos = transform.position;
            transform.position = otherCloud.position;
            otherCloud.transform.position = oldPos;
        }
    }
}