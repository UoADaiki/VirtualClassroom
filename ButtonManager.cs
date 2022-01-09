using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ButtonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Slider zoomSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ズームボタン用のメソッド
    public void OnPushZoomButton(){
        /*
        ZoomUserCamera.instance.ZoomCameraButton();
        */

        if(zoomSlider.gameObject.activeSelf == false){
            zoomSlider.gameObject.SetActive(true);
        }
        else zoomSlider.gameObject.SetActive(false);
    }

    // ズームスライダー用のメソッド
    public void OnChangeZoomValue(){
        ZoomUserCamera.instance.ZoomCameraSlider(zoomSlider.value);
    }

}
