 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform camera;
    private Vector3 lastCamPos;
    private float multiplier = 0.7f; 
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").transform;
        lastCamPos = camera.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = camera.position - lastCamPos;
        if (this.gameObject.name == "Clouds")
        {
            transform.position += delta * multiplier;
        }
        else
        {
            transform.position += delta;
        }
        lastCamPos = camera.position;
    }
}
