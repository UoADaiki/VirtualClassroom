using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB;
using Photon.Pun;
using Photon.Realtime;

public class ShareManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject shareImagePanel;

    [SerializeField] Text filepathText;
    private string filepathName;

    [SerializeField] Image leftImage;
    [SerializeField] Image rightImage;

    [SerializeField] private List<Toggle> toggles;
    private bool isLeftSide = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPushShareButton(){
        if(shareImagePanel.activeSelf == false){
            shareImagePanel.SetActive(true);
        }
        else{
            shareImagePanel.SetActive(false);
        }
    }

    public void OnSelectFilePath(){
        var Filter = new []{
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };

        var filepath = StandaloneFileBrowser.OpenFilePanel("Open File", "", Filter, false);
        if(string.IsNullOrEmpty(filepath[0])){
            return;
        }

        FileInfo fileinfo = new FileInfo(filepath[0]);
        long filesize = fileinfo.Length / 1024;  // conversion B -> KB 
        if(filesize >= 500){
            // More than 500KB image can not be loaded
            filepathText.text = "This image size is too big.";
            return;
        }

        filepathName = filepath[0];

        filepathText.text = "Path:\n" + filepathName;
    }

    public void OnUploadImage(){ 
        byte[] bytes = File.ReadAllBytes(filepathName);
        
        photonView.RPC("ChangeImage", RpcTarget.AllBuffered, bytes, isLeftSide);
    }


    // Syncronize the changing og image
    [PunRPC]
    void ChangeImage(byte[] bytes, bool isLeft){

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        Rect rect = new Rect(0f, 0f, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

        if(isLeft == true){
            leftImage.sprite = sprite;
        }
        else{
            rightImage.sprite = sprite;
        }    
    }
   
    // When board toggles are changed
    /*
    toggle[0] -> Left
    toggle[1] -> Right
    */
    public void ChangeBoardSide(){
        if(toggles[0].isOn){
            isLeftSide = true;
        }
        else isLeftSide = false;
    }
}
