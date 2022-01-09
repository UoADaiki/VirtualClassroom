using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class CalcSpatial : MonoBehaviourPun
{   
    //private GameObject[] avatars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetSpatialInfo();
    }

    public void GetSpatialInfo(){
        if(photonView.IsMine != true){
            return;
        }

        foreach(Player player in PhotonNetwork.PlayerListOthers){
            GameObject otherplayer = player.TagObject as GameObject;
            if(otherplayer == null){
                Debug.Log("GetSpatialInfo() - Other Player is not found");
                return;
            }
            //Debug.Log(otherplayer.transform.position);

            float dist = CalcDistance(this.gameObject.transform.position, otherplayer.transform.position);
            Debug.Log("Distance: "  + dist);

            float azi = CalcAzimuth(this.gameObject.transform, otherplayer.transform);
            Debug.Log("Azimuth: " + azi);

        
            float ori = CalcOrientation(this.gameObject.transform, otherplayer.transform);
            Debug.Log("Orientation: " + ori);
            
            //Debug.Log("OtherRight: " + otherplayer.transform.forward);
            //Debug.Log("MyRight: " + this.gameObject.transform.forward);
            
            //Debug.Log("AngleY: " + otherplayer.transform.rotation.eulerAngles.y);
        }   

    }

    public float CalcDistance(Vector3 speakerPos, Vector3 listenerPos){
        float dist = Vector3.Distance(listenerPos, speakerPos);
        return dist;
    }

    public float CalcAzimuth(Transform speakerTrans, Transform listenerTrans){
        Vector3 diff = listenerTrans.position - speakerTrans.position;
        Vector3 axis = Vector3.Cross(speakerTrans.forward, diff);
        float angle = Vector3.Angle(speakerTrans.forward, diff) * (axis.y < 0 ? 1 : -1); // counterclockwise
        float azi = Mathf.Repeat(angle, 360);

        return azi;
    }

    public float CalcOrientation(Transform speakerTrans, Transform listenerTrans){
        /*
        float ori = Quaternion.Angle(speakerTrans.rotation, listenerTrans.rotation);
        if(listenerTrans.right.z < 0.0f){
            ori = 360 - ori;
        }
        */

        float angle = listenerTrans.rotation.eulerAngles.y - speakerTrans.rotation.eulerAngles.y;
        float ori = Mathf.Repeat(angle, 360);
        ori = 360 - ori;
        return ori;
    }


}
