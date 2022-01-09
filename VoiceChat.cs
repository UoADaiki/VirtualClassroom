using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class VoiceChat : MonoBehaviour
{   
    [SerializeField] private Text muteButtonText;
    [SerializeField] public Recorder recorder;
    private bool isMute;

    // Start is called before the first frame update
    void Start()
    {
       isMute = true;
       muteButtonText.text = "Unmute";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPushMicMuteButton(){
        if(isMute == true){
            // Mute -> Unmute
            recorder.TransmitEnabled = true;
            isMute = false;
            muteButtonText.text = "Mute";
        }
        else{
            // Unmute -> Mute
            recorder.TransmitEnabled = false;
            isMute = true;
            muteButtonText.text = "Unmute";
        }
    }
}
