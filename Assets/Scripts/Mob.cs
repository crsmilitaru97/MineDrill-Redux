using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MobsManager;

public class Mob : MonoBehaviour
{
    public int life;
    public int type; //Fish, Spider, Worm
    public float speed;

    bool isUIShown = false;
    bool isDrillDest = true;
    int maxLife;
    GameObject lifeUI;
    Vector2 dest;
    Quaternion spiderRotation;
    Quaternion wormRotation;


    void Start()
    {
        maxLife = life;

        if (type == 0)
            dest = Drill.Instance.transform.position;
        if (type == 1)
            dest = transform.position;
        if (type == 2)
        {
            dest = Drill.Instance.transform.position;
            dest.x = -TerrainManager.sizeX - 5;
        }
    }

    private void LateUpdate()
    {
        if (lifeUI != null)
        {
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
            Vector2 WorldObject_ScreenPosition = new Vector2(
            (ViewportPosition.x * Manager.canvas.sizeDelta.x) - (Manager.canvas.sizeDelta.x * 0.5f),
            (ViewportPosition.y * Manager.canvas.sizeDelta.y) - (Manager.canvas.sizeDelta.y * 0.5f));
            lifeUI.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        }
    }

    void Update()
    {
        //Move
        transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * speed);

        //Rotate
        if (type == 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(FZMath.AngleBetweenTwoPoints(dest, transform.position), Vector3.forward);
            transform.rotation = Quaternion.Euler(transform.rotation.x, dest.x < transform.position.x ? 180 : 0, transform.rotation.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 200);
        }
        else if (type == 1)
        {
            transform.GetChild(0).transform.rotation = spiderRotation;
        }
        else if (type == 2)
        {
            wormRotation = Quaternion.Euler(0, 0, dest.x < 0 ? 90 : -90);
            transform.rotation = wormRotation;
        }


        //Arrive
        if (Vector3.Distance(transform.position, dest) == 0)
        {
            if (type == 0) //Fish
            {
                if (isDrillDest)
                {
                    transform.GetChild(1).GetComponent<Animator>().SetTrigger("Attack");
                    isDrillDest = false;
                    dest = new Vector3(dest.x + FZRandom.RandomBetween(-20, 20), dest.y + Random.Range(-5, 5));
                }
                else
                {
                    isDrillDest = true;
                    dest = Drill.Instance.transform.position;
                }
            }
            else if (type == 1) //Spider
            {
                var availableDirections = CheckDirectionsInCave(transform.position);
                dest = availableDirections.RandomItem();

                spiderRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                if (dest.x < transform.position.x)
                    spiderRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                else if (dest.y < transform.position.y)
                    spiderRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                else if (dest.y > transform.position.y)
                    spiderRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
            else if (type == 2) //Worm
            {
                //wormRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                //if (dest.x < transform.position.x)
                //    wormRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                //else if (dest.y < transform.position.y)
                //    wormRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                //else if (dest.y > transform.position.y)
                //    wormRotation = Quaternion.Euler(new Vector3(0, 0, 90));

                if (Drill.Instance.transform.position.y < -2)
                {
                    if (dest.x < 0)
                    {
                        dest = Drill.Instance.transform.position;
                        dest.x = TerrainManager.sizeX + 5;
                    }
                    else
                    {
                        dest = Drill.Instance.transform.position;
                        dest.x = -TerrainManager.sizeX - 5;
                    }
                }
                else
                {
                    if (dest.x < 0)
                    {
                        dest = Drill.Instance.transform.position;
                        dest.x = TerrainManager.sizeX + 5;
                        dest.y = Random.Range(-4, -12);
                    }
                    else
                    {
                        dest = Drill.Instance.transform.position;
                        dest.x = -TerrainManager.sizeX - 5;
                        dest.y = Random.Range(-4, -12);
                    }
                }
            }
        }
    }

    private List<Vector3> CheckDirectionsInCave(Vector2 pos)
    {
        List<Vector3> poses = new List<Vector3>();

        if (TerrainManager.Terrain[new Vector2(pos.x, pos.y - 1)].ignorable)
            poses.Add(new Vector2(pos.x, pos.y - 1));
        if (TerrainManager.Terrain[new Vector2(pos.x, pos.y + 1)].ignorable)
            poses.Add(new Vector2(pos.x, pos.y + 1));
        if (TerrainManager.Terrain[new Vector2(pos.x - 1, pos.y)].ignorable)
            poses.Add(new Vector2(pos.x - 1, pos.y));
        if (TerrainManager.Terrain[new Vector2(pos.x + 1, pos.y)].ignorable)
            poses.Add(new Vector2(pos.x + 1, pos.y));

        return poses;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            if (!isUIShown)
            {
                isUIShown = true;
                lifeUI = Instantiate(Manager.mobLifeUI, Manager.canvas);
            }
            GetComponent<FZShake2DEffect>().TriggerShake(0.1f);
            life--;
            Debug.Log("Hit Insect");

            if (life <= 0) //Death
            {
                if (type == 0)
                    totalMob0--;
                if (type == 2)
                    totalMob2--;

                Destroy(lifeUI);
                Destroy(gameObject);

                Manager.AddKey(type);
            }
            else
            {
                Image image = lifeUI.transform.GetChild(0).GetComponent<Image>();
                image.fillAmount = (float)life / maxLife;
            }
        }

        if (collision.tag == "Drill")
        {
            Drill.Instance.ModifyLifeAmount();
        }
    }
}