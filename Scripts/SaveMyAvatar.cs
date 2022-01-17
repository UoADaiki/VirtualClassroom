using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class SaveMyAvatar : MonoBehaviour, IPunInstantiateMagicCallback
{
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info){
        /*
        if(info.Sender.IsLocal){
            info.Sender.TagObject = this.gameObject;
        }
        else{
            Debug.Log("Other player is generated.");
        }
        */
        info.Sender.TagObject = this.gameObject;
    }
    
}
