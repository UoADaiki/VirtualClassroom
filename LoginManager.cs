/*
    The manipulation of login panel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoginManager : MonoBehaviourPunCallbacks
{
    [Header("For UserNameSetPanel")]
    [SerializeField] GameObject userNameSetPanel;
    [SerializeField] private InputField userName;
    [SerializeField] private List<Toggle> toggles;  
    private bool isTeacher = true;

    [Header("For SelectWayPanel")]
    [SerializeField] GameObject selectWayPanel;


    [Header("For CreateRoomPanel")]
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] private InputField createRoomCode;

    [Header("For JoinRoomPanel")]
    [SerializeField] GameObject joinRoomPanel;
    [SerializeField] private InputField joinRoomCode;

    [Header("For Mainmenu")]
    [SerializeField] GameObject menuButtons;
    [SerializeField] GameObject cameraButton;
    [SerializeField] GameObject shareImageFunction;
    [SerializeField] GameObject voiceNoteFunction;


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the master server is connected 
    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby();
    }

    // When the user joins the lobby
    public override void OnJoinedLobby() {
        Debug.Log("Join the lobby");
        userNameSetPanel.SetActive(true);
    }
    
    // When "CreateRoomButton" is pushed
    public void OnCreateRoomButtonPushed(){
        // Deside the role of the room
        RoomOptions roomoption = new RoomOptions(){
            IsVisible = false,
            MaxPlayers = 10
        };

        if(createRoomCode.text.Length == 6 ){
            PhotonNetwork.CreateRoom(createRoomCode.text, roomoption, TypedLobby.Default);
        }   
    }  

    // When "JoinRoomButton" is pushed
    public void OnJoinRoomButtonPushed(){
        if(joinRoomCode.text.Length == 6){
            PhotonNetwork.JoinRoom(joinRoomCode.text);
        }
    } 

    // When the user succeed to join the room
    public override void OnJoinedRoom(){
        Debug.Log("Join the room");

        // Set the user name
        if(userName.text != ""){
            PhotonNetwork.LocalPlayer.NickName = userName.text;
        }
        else{
            PhotonNetwork.LocalPlayer.NickName = "DefaultName";
        }

        // Generate the avatar
        if(isTeacher == true){
            GameObject teacher = PhotonNetwork.Instantiate("TeacherAvatar", new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity, 0);
            teacher.GetPhotonView().RPC("SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }
        else{
            GameObject student = PhotonNetwork.Instantiate("StudentAvatar", new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity, 0);
            student.GetPhotonView().RPC("SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }


        // When the user is in CreateRoomPanel
        if(createRoomPanel.activeSelf == true){
            createRoomPanel.SetActive(false);
        }
        // When the user is in JoinRoomPanel
        else if(joinRoomPanel.activeSelf == true){
            joinRoomPanel.SetActive(false);
        }
        menuButtons.SetActive(true);
        if(isTeacher == true){
            cameraButton.SetActive(true);
            shareImageFunction.SetActive(true);
        }
        else{
            voiceNoteFunction.SetActive(true);
        }
    }

    void OnPhotonJoinRoomFailed() {
        Debug.Log("Failed to join the room");
        joinRoomCode.text = string.Empty;
    }

    // When status toggles are changed
    /*
    toggle[0] -> Teacher
    toggle[1] -> Student
    */
    public void ChangeStatus(){
        if(toggles[0].isOn){
            isTeacher = true;
        }
        else isTeacher = false;
    }

    // When "Next button" is pushed
    public void ToSelectWayPanel(){
        userNameSetPanel.SetActive(false);
        selectWayPanel.SetActive(true);
    }

    // When "Create the room button" is pushed in SelectWayPanel
    public void ToCreateRoomPanel(){
        selectWayPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    // When "Join the room button" is pushed in SelectWayPanel
    public void ToJoinRoomPanel(){
        selectWayPanel.SetActive(false);
        joinRoomPanel.SetActive(true);
    }

    // When "Back button" is pushed in SelectWayPanel
    public void BackToUserNamePanel(){
        selectWayPanel.SetActive(false);
        userNameSetPanel.SetActive(true);
    }

    // When "Back button" is pushed in CreateRoomPanel or JoinRoomPanel
    public void BacktoSelectWayPanel(){
        // When the user is in CreateRoomPanel
        if(createRoomPanel.activeSelf == true){
            createRoomPanel.SetActive(false);
            selectWayPanel.SetActive(true);
        }
        // When the user is in JoinRoomPanel
        else if(joinRoomPanel.activeSelf == true){
            joinRoomPanel.SetActive(false);
            selectWayPanel.SetActive(true);
        }
    }

    // When "Logout Button" is pushed
    public void OnLogoutButtonPushed(){
        PhotonNetwork.LeaveRoom();
    }

    // When the user leave the room
    public override void OnLeftRoom() {
        Debug.Log("Left the room");
        if(menuButtons != null){
            menuButtons.SetActive(false);      
            if(isTeacher == true){
                cameraButton.SetActive(false);
                shareImageFunction.SetActive(false);
            }
            else{
                voiceNoteFunction.SetActive(false);
            }
        }
    }
/*
    public bool isTeacherStatus(){
        if(isTeacher == true){
            return true;
        }
        else{
            return false;
        }
    }
*/
}
