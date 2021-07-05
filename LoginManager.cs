using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{   
    // Setting gameobjects from the inspecter window
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject createPanel;

    public static bool isTeacher;  
    

    // Start is called before the first frame update
    void Start()
    {
        isTeacher = true;
        StartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The process when BackButton is pushed
    public void StartMenu(){
        startPanel.SetActive(true);
        createPanel.SetActive(false);
    }

    // The process when CreateButton is pushed at CreatePanel or JoinPanel 
    public void CreateMenu(){
        startPanel.SetActive(false);
        createPanel.SetActive(true);
    }

    // The process when CreateButton is pushed at CreatePanel
    public void CreateRoom(){
        isTeacher = true;
        SceneManager.LoadScene("Classroom");
    }

    // Send the user status(teacher or student) for Classroom scene
    public static bool checkTeacher(){
        return isTeacher;
    }

}
