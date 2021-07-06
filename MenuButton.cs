/*
The manipulation of user menu button
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    GameObject menuPanel;
    bool menuFlag;
    // Start is called before the first frame update
    void Start()
    {   
        menuPanel = GameObject.Find("MenuPanel");
        menuPanel.SetActive(false);
        menuFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenuClick(){
        if(menuFlag == true){   // When the menu is open,
            menuPanel.SetActive(false);    // menu is close.
            menuFlag = false;
        }
        else{                   // When the menu is close,
            menuPanel.SetActive(true);      // menu is open.
            menuFlag = true;
        }
    }
}
