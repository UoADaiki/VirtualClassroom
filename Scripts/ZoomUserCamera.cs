using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ZoomUserCamera : MonoBehaviourPunCallbacks
{
    [SerializeField] Camera userCamera;
    private GameObject mainCamera;
    private Camera cameraComp;
    private bool isOnZoom;

    public static ZoomUserCamera instance;


    // Start is called before the first frame update
    void Start()
    {   
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraComp = mainCamera.GetComponent<Camera>();
        if(cameraComp == null){
            Debug.Log("ZoomUserCamera - Camera component is not found");
            return;
        }
        isOnZoom = false;
    }

    // Update is called once per frame
    void Update()
    {   
        //zoomLevel = zoomSlider.value;
        //userCamera.fieldOfView = zoomSlider.value;
    }

    void Awake(){
        if(instance == null){
            instance = this;
        }
    }

    // ボタンでズーム変更用
    public void ZoomCameraButton(){
        if(photonView.IsMine == true){
            if(isOnZoom == false){
                userCamera.fieldOfView = 20;
                isOnZoom = true;
            }
            else{
                userCamera.fieldOfView = 60;
                isOnZoom = false;
            }
        }
    }

    // スライダーでズーム変更用
    public void ZoomCameraSlider(float zoomLevel){
        if(photonView.IsMine == true){
            //userCamera.fieldOfView = zoomLevel;
            cameraComp.fieldOfView = zoomLevel;
        }
    }
}
