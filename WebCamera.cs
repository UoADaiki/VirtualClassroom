using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamera : MonoBehaviour
{   
    // width and height are constant value
    private int width = 1;
    private int height = 1;
    private int fps = 40;  
    
    WebCamTexture webCamTexture;
    [SerializeField] GameObject cube;  // Camera object
    [SerializeField] Texture offCamera;

    // Start is called before the first frame update
    void Start()
    {
        webCamTexture = new WebCamTexture(width, height, fps); // width, height, framerate
        cube.GetComponent<MeshRenderer>().material.mainTexture = offCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.V)){        // When you press left Alt(option) + V key,
            if(webCamTexture.isPlaying == false){  // if your camera is off,
                cube.GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
                webCamTexture.Play();   // turn on your camera.
            }
            else{                       // if your camera is on,
                cube.GetComponent<MeshRenderer>().material.mainTexture = offCamera;
                webCamTexture.Stop();   // turn off your camera.
            }
        }
        
    }
}
