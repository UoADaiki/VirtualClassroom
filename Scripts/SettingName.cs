using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SettingName : MonoBehaviour
{   
    public Text nameText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    // Setting the user name
    [PunRPC]
    void SetName(string name){
        nameText.text = name;
    }
}
