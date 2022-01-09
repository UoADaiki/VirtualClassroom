using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RecordManager : MonoBehaviour
{
    private string micDevice;  // The name of microphone
    private float maxTime;     // The max time of recording

    public AudioClip recordClip;
    public AudioClip newCrip;

    private bool isRecording;
    [SerializeField] GameObject recordingText;

    // Start is called before the first frame update
    void Start()
    {
        maxTime = 60; 
        isRecording = false; 
    }

    // Update is called once per frame
    void Update()
    {   

    }

    // When the recording is started
    public void StartRecord(){
        // If the microphone is not found, the process is stopped
        if(Microphone.devices.Length == 0){
            Debug.Log("Microphone is not found.");
            return;
        }

        micDevice = Microphone.devices[0];

        recordClip = Microphone.Start(micDevice, false, (int)maxTime, 16000);
        
    }

    public void StopRecord(){
        int position = Microphone.GetPosition(micDevice);

        Microphone.End(micDevice);

        Debug.Log("Default recording time: " + recordClip.length);

        float[] soundData = new float[recordClip.samples * recordClip.channels];
        recordClip.GetData(soundData, 0); 

        float[] newData = new float[position * recordClip.channels];

        for(int i = 0; i < newData.Length; i++){
            newData[i] = soundData[i];
        }

        newCrip = AudioClip.Create(recordClip.name, position, recordClip.channels, recordClip.frequency, false);
        newCrip.SetData(newData, 0);

        AudioClip.Destroy(recordClip);
        recordClip = newCrip;

        SaveRecord();

        Debug.Log("Fixed recording time: " + newCrip.length);

    }

    public void SaveRecord(){
        string filepath;
        byte[] bytes = WavUtility.FromAudioClip(recordClip, out filepath, true);

    }

    public void OnPushVoiceNoteButton(){
    // If the button is pushed, the recording is started
        if(isRecording == false){
            StartRecord();
            isRecording = true;
            recordingText.SetActive(true);
        }
        else{
            StopRecord();
            isRecording = false;
            recordingText.SetActive(false);
        }

    }
}
