using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public GameObject prefab;
    bool canPlace;

    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(Mathf.Round(pos.x) + 0.4f, Mathf.Round(pos.y) - 0.4f, 0);
        var roundedPosition = Vector3Int.RoundToInt(transform.position);

        if (transform.position.y <= -1 && TerrainManager.GetBlock(roundedPosition) == null)
        {
            canPlace = true;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            canPlace = false;
            GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            Instantiate(prefab, roundedPosition, Quaternion.identity);
            var powerup = SlotsManager.Manager.selectedSlot.powerup;
            powerup.quantity = 1;
            PowerupsManager.Manager.RemoveFromBuyedPowerupsList(powerup);
            SlotsManager.Manager.selectedSlot = null;
            Destroy(gameObject);
        }

        if (Input.GetMouseButtonDown(1))
        {
            SlotsManager.Manager.selectedSlot.UnselectUI();
            SlotsManager.Manager.selectedSlot = null;
            Destroy(gameObject);
        }

    }
}
