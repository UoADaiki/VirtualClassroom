using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AvatarController : MonoBehaviourPunCallbacks
{
    ///// About Avatar Moving /////
    public float speed;

    ///// About User Camera /////
    [SerializeField] Camera userCamera;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine == true){
            userCamera.depth += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine == true){

            if (Input.GetKey ("up")) {
			    transform.position += transform.forward * speed * Time.deltaTime;
		    }
		    if (Input.GetKey ("down")) {
			    transform.position -= transform.forward * speed * Time.deltaTime;
		    }
		    if (Input.GetKey("right")) {
			    //transform.position += transform.right * speed * Time.deltaTime;
                transform.Rotate(0.0f, 1.0f, 0.0f);
		    }
		    if (Input.GetKey ("left")) {
			    //transform.position -= transform.right * speed * Time.deltaTime;
                transform.Rotate(0.0f, -1.0f, 0.0f);
		    }
        }   
    }

}
