using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform leader;
    Camera cam;
    float camWidth;
    float camHeight;
    void Start()
    {
        leader = GameObject.Find("Player").transform;
        cam = GetComponent<Camera>();
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        float x = leader.position.x;
        float y = leader.position.y;
        x = Mathf.Clamp(x, camWidth / 2, WorldGenerator.worldWidth - camWidth / 2);
        y = Mathf.Clamp(y, camHeight / 2, WorldGenerator.worldHeight - camHeight / 2);
        transform.position = new Vector3(x, y, -10);
    }
}
