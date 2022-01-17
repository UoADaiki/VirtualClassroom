using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using agora_gaming_rtc;

public class VideoController : MonoBehaviourPunCallbacks
{
    [Header("Agora Properties")]
    [SerializeField] private string appID = "";
    [SerializeField] private string channel = "unity3d";
    private string originalChannel;
    private IRtcEngine mRtcEngine;
    private uint myUID = 0;
    CLIENT_ROLE_TYPE myClientRole;

    [Header("Player Video Panel Properties")]
    [SerializeField] private GameObject userVideoPrefab;
    private int Offset = 100;
    private Button cameraButton;

    [Header("Participants status")]
    public bool isTeacher;
    private bool isMovedFlag;
    private bool isOnCamera;

    // Start is called before the first frame update
    void Start()
    {
        if(!photonView.IsMine){
            return;
        }

        if(mRtcEngine != null){
            IRtcEngine.Destroy();
        }

        originalChannel = channel;

        mRtcEngine = IRtcEngine.GetEngine(appID);

        // Setting callback functions
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccessHandler;
        mRtcEngine.OnUserJoined = OnUserJoinedHandler;
        mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;
        mRtcEngine.OnUserOffline = OnUserOfflineHandler;
        mRtcEngine.OnClientRoleChanged = OnClientRoleChangedHandler;


        // Calling the video
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();

        // Audio is OFF (because photon voice is used)
        mRtcEngine.DisableAudio();


        // Only teacher can use camera       
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        if(isTeacher == true){
            mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        }
        else{
            mRtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
        }
        

        isMovedFlag = false;
        isOnCamera = false;
        mRtcEngine.JoinChannel(channel, null, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        if(!photonView.IsMine){   
            Debug.Log("This is not me.");
            return;
        }

        myUID = uid;

        Debug.LogFormat("I: {0} joined channel: {1}.", uid.ToString(), channelName);
        
        
        if(myClientRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER){
            CreateUserVideoSurface(uid, true);
        } 
    }

    private void OnUserJoinedHandler(uint uid, int elapsed)
    {
        if(!photonView.IsMine){
            return;
        }          
        CreateUserVideoSurface(uid, false);
    }

    private void OnLeaveChannelHandler(RtcStats stats)
    {
        if(!photonView.IsMine){
            return;
        }       
    }

    private void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        if(!photonView.IsMine){
            return;
        }          
    }

    private void OnClientRoleChangedHandler(CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
    {
        myClientRole = newRole;
    }

    private void CreateUserVideoSurface(uint uid, bool isLocalUser)
    {   
        // Create Gameobject holding video surface and update properties

        GameObject Teacher = GameObject.FindWithTag("Teacher");
        //GameObject Teacher = GameObject.Find("TeacherAvatar(Clone)");
        if(Teacher == null){
            return;
        }       
        Vector3 teacherPos = Teacher.transform.position;
        Vector3 SpawnPos = new Vector3(teacherPos.x + (0.5f * Teacher.transform.forward.x), teacherPos.y + 1.0f, teacherPos.z + (0.5f * Teacher.transform.forward.z)); 
        GameObject newUserVideo = (GameObject)Instantiate(userVideoPrefab, SpawnPos, Teacher.transform.rotation); 

      
        if(isLocalUser == true){
            cameraButton = GameObject.FindWithTag("CameraButton").GetComponent<Button>();
            cameraButton.onClick.AddListener( () => OnPushCameraButton());
        }
         
        if (newUserVideo == null)
        {
            Debug.LogError("CreateUserVideoSurface() - newUserVideoIsNull");
            return;
        }

        newUserVideo.name = uid.ToString();
       
        if(Teacher != null){
            newUserVideo.transform.parent = Teacher.transform;
        }


        // Update our VideoSurface to reflect new users
        VideoSurface newVideoSurface = newUserVideo.GetComponent<VideoSurface>();
        if (newVideoSurface == null)
        {
            Debug.LogError("CreateUserVideoSurface() - VideoSurface component is null on newly joined user");
        }

        if (isLocalUser == false)
        {
            newVideoSurface.SetForUser(uid);
        }
        newVideoSurface.SetEnable(false);
        newVideoSurface.SetGameFps(30);
        
    }

    public void OnPushCameraButton(){
        if(cameraButton == null){
            Debug.Log("Camera Button is not found");
            return;
        }

        Text cameraButtonText = cameraButton.GetComponentInChildren<Text>();
        if(isOnCamera == true){
            // Camera ON -> OFF
            cameraButtonText.text = "Camera ON";
            isOnCamera = false;
            photonView.RPC("SetONOFFCamera", RpcTarget.AllBuffered, true);
        }
        else{
            // Camera OFF -> ON
            cameraButtonText.text = "Camera OFF";
            isOnCamera = true;
            photonView.RPC("SetONOFFCamera", RpcTarget.AllBuffered, false);
        }
    }

    [PunRPC]
    void SetONOFFCamera(bool isOn){
        GameObject cameraObject = GameObject.FindWithTag("CameraObj");
        if(cameraObject == null){
            Debug.Log("SetONOFFCamera() - Camera Object is not found");
            return;
        }

        VideoSurface cameraSurface = cameraObject.GetComponent<VideoSurface>();
        if(cameraSurface == null){
            Debug.Log("SetONOFFCamera() - Camera Surface is not found");
            return;
        }

        if(isOn == true){
            cameraSurface.SetEnable(false);
        }
        else{
            cameraSurface.SetEnable(true);
        }
    }


    private void TerminateAgoraEngine()
    {
        if(mRtcEngine != null)
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine = null;
            IRtcEngine.Destroy();
        }  
    }
/*
    private IEnumerator OnLeftRoom()
    {
        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while (PhotonNetwork.CurrentRoom != null || PhotonNetwork.IsConnected == false)
            yield return 0;

        TerminateAgoraEngine();
    }
*/
    public override void OnLeftRoom() {
        TerminateAgoraEngine();
    }

    private void OnApplicationQuit(){
        TerminateAgoraEngine();
    }
}
