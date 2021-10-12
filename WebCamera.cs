/*
This program is used for User's camera.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WebCamera : MonoBehaviourPunCallbacks
{   
    // width and height are constant value
    private int width = 256;
    private int height = 256;
    private int fps = 40;  
    
    WebCamTexture webCamTexture;
    [SerializeField] Renderer cube;  // Camera object
    [SerializeField] Texture offCamera;

    // Start is called before the first frame update
    void Start()
    {
        webCamTexture = new WebCamTexture(width, height, fps); // width, height, framerate
        //cube.GetComponent<MeshRenderer>().material.mainTexture = offCamera;
        cube.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if(photonView.IsMine == true){   
            if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.V)){        // When you press left Alt(option) + V key,
                photonView.RPC("OnOffCamera", RpcTarget.AllBuffered);
            }
        }
         
    }

    // Switch web camera ON/OFF 
    [PunRPC]
    public void OnOffCamera(){      
        if(webCamTexture.isPlaying == false){  // if your camera is off,
            cube.enabled = true;      
            cube.GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
            webCamTexture.Play();   // turn on your camera.
        }
        else{                       // if your camera is on,
            //cube.GetComponent<MeshRenderer>().material.mainTexture = offCamera;
            webCamTexture.Stop();   // turn off your camera.
            cube.enabled = false;
        }
        
    }

    public void cameraButton(){
        if(photonView.IsMine == true){   
            photonView.RPC("OnOffCamera", RpcTarget.AllBuffered);          
        }
    }

}
