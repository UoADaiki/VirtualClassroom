using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject teacherNamePanel;
    public static bool mode;   //  check teacher or student

    // Start is called before the first frame update
    void Start()
    {
        mode = LoginManager.checkTeacher();
        StartPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the classroom scene is started, this panel is opened at first.
    public void StartPanel(){
        // if user is teacher, this open the panel for teacher
        if(mode == true){
            teacherNamePanel.SetActive(true);
        }
    }

    public void PressOKButton(){
        teacherNamePanel.SetActive(false);
    }
}
