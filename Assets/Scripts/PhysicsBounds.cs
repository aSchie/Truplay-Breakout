using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBounds : MonoBehaviour
{
    [SerializeField] private GameObject bottomLeftBounds;
    [SerializeField] private GameObject topRightBounds;

    void Start()
    {
        ClampBoundariesToScreenSize();
    }


    void ClampBoundariesToScreenSize()
    { 
        ClampTopRightBounds();
        ClampBottomLeftBounds();
    }

    private void ClampBottomLeftBounds()
    {
        bottomLeftBounds.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
    }

    private void ClampTopRightBounds()
    {
        topRightBounds.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
    }
}
