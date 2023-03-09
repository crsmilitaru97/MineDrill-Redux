using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    public GameObject rocket;
    public Drill drill;
    [Range(0f, 3f)]
    public float smoothSpeed;

    int maxBorderX = 14;
    Camera cam;
    bool right, left;
    bool isMouse;
    public static bool zoomToRocket;
    public static bool canMove = true;
    public bool zoomOnBuilding;
    public Transform building;

    int speed = 2;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    Vector3 pos;

    void LateUpdate()
    {
        if (!canMove)
            return;

        if (zoomOnBuilding)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(building.position.x , transform.position.y, -10), Time.deltaTime * 100);

            if (transform.position == building.position)
            {
                zoomOnBuilding = false;
            }
            return;
        }

        if (zoomToRocket)
        {
            pos = rocket.transform.position;
            pos.z = -10;
            if (pos.y > 50)
            {
                pos.x = 25.5f;
                speed = 3;
            }

            if (pos.y > 90)
            {
                speed = 5;
            }

            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * smoothSpeed * speed);
            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, 6.5f, Time.deltaTime * smoothSpeed);
            return;
        }
        else
            pos = drill.transform.position;

        if (Drill.isOnLand && isMouse)
        {
            if (Drill.up || Drill.down || Drill.left || Drill.right)
            {
                isMouse = false;
                return;
            }
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (left && transform.position.x > -17)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(mousePos.x, 3, -10), Time.deltaTime * smoothSpeed * 2);
            }

            if (right && transform.position.x < 17)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(mousePos.x, 3, -10), Time.deltaTime * smoothSpeed * 2);
            }
        }
        else
        {
            if (pos.x > -maxBorderX && pos.x < maxBorderX)
                transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, Drill.isOnLand ? 3 : pos.y, -10), Time.deltaTime * drill.speed * smoothSpeed);
            else
            {
                if (pos.x > 0 && pos.x < maxBorderX + 100)
                    transform.position = Vector3.Lerp(transform.position, new Vector3(maxBorderX, Drill.isOnLand ? 3 : pos.y, -10), Time.deltaTime * drill.speed * smoothSpeed);
                else if (pos.x < 0 && pos.x > -maxBorderX - 100)
                    transform.position = Vector3.Lerp(transform.position, new Vector3(-maxBorderX, Drill.isOnLand ? 3 : pos.y, -10), Time.deltaTime * drill.speed * smoothSpeed);
                else
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Drill.isOnLand ? 3 : pos.y, -10), Time.deltaTime * drill.speed * smoothSpeed);
            }
        }
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && cam.orthographicSize > 4)
        {
            cam.orthographicSize -= .5f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && cam.orthographicSize < 8)
        {
            cam.orthographicSize += .5f;
        }
    }

    public void Right(bool val)
    {
        isMouse = true;
        right = val;
    }

    public void Left(bool val)
    {
        isMouse = true;
        left = val;
    }
}