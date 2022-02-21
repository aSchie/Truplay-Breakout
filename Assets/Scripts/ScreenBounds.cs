using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    private Vector2 screenBounds;

    private float objWidth;
    private float objHeight;


    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        // only need half the height and width of the object since the point is at center
        objWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    void Update()
    {
        // capture our position
        Vector3 objPosition = transform.position;

        // clamp ourselves within the screen space (had to find which coords were supposed to be negative because of the ortho view)

        objPosition.x = Mathf.Clamp(objPosition.x, -screenBounds.x + objWidth, screenBounds.x - objWidth);
        objPosition.y = Mathf.Clamp(objPosition.y, -screenBounds.y + objHeight, screenBounds.y - objHeight);

        // set our position to the clamped (if needed) position.
        transform.position = objPosition;

    }
}
