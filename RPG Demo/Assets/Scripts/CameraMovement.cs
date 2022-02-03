using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Camera cam;

    public float cameraSpeed = 1f;
    
    float offsetX;
    float offsetY;

    private void Start()
    {
        offsetY = cam.transform.position.y;
        offsetX = cam.transform.position.x;
    }

    void Update()
    {
        if(target.transform.position.y > (cam.orthographicSize + cam.transform.position.y))
        {
            offsetY += (cam.orthographicSize * 2);
        }
        if (target.transform.position.y < (cam.transform.position.y - cam.orthographicSize))
        {
            offsetY -= (cam.orthographicSize * 2);
        }
        if (target.transform.position.x > (cam.transform.position.x + (cam.orthographicSize * cam.aspect)))
        {
            offsetX += ((cam.orthographicSize * cam.aspect) * 2);
        }
        if (target.transform.position.x < (cam.transform.position.x - (cam.orthographicSize * cam.aspect)))
        {
            offsetX -= ((cam.orthographicSize * cam.aspect) * 2);
        }


        if (offsetY > cam.transform.position.y)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, (cam.transform.position.y + (0.1f * cameraSpeed)), cam.transform.position.z);
        }
        if (offsetY < cam.transform.position.y)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, (cam.transform.position.y - (0.1f * cameraSpeed)), cam.transform.position.z);
        }
        if (offsetX > cam.transform.position.x)
        {
            cam.transform.position = new Vector3((cam.transform.position.x + (0.15f * cameraSpeed)), cam.transform.position.y, cam.transform.position.z);
        }
        if (offsetX < cam.transform.position.x)
        {
            cam.transform.position = new Vector3((cam.transform.position.x - (0.15f * cameraSpeed)), cam.transform.position.y, cam.transform.position.z);
        }
    }
}
