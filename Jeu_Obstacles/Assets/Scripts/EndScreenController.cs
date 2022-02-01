using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EndScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (MainMenuController.currentProfile != null) {
            string jsonStr = JsonConvert.SerializeObject(MainMenuController.currentProfile);
            File.WriteAllText(@"./Assets/SaveProfiles/"+MainMenuController.currentProfile.name+".json", jsonStr);
        }
    }
    public void QuitButtonPush(){
        Debug.Log("quit");
        Application.Quit();
    }

    public void MenuButtonPush(){
        Debug.Log("menu");
        SceneManager.LoadScene(0);
    }

    public void PlayButtonPush(){
        Debug.Log("play");
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
