using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Profile;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CreateMenuController : MonoBehaviour
{
    private static string profileName;
    private static ProfileType profile;
    // Start is called before the first frame update
    void Awake()
    {
        profileName = "";
        profile = new ProfileType();
    }

    public void OnWrite(GameObject inputField){
        InputField input = inputField.GetComponent<InputField>();
        profileName = input.text;
    }

    public void MenuButtonPush(){
        Debug.Log("menu");
        SceneManager.LoadScene(0);
    }

    public void CreateButtonPush(GameObject inputField){
        string[] filePaths = Directory.GetFiles("./Assets/SaveProfiles","*.json");
        List<string> fileNames = new List<string>();
        foreach (string f in filePaths){
            fileNames.Add(Path.GetFileNameWithoutExtension(f));
        }


        if(fileNames.Contains(profileName)){
            InputField input = inputField.GetComponent<InputField>();
            input.placeholder.GetComponent<Text>().text = "Profil already exist"; 
        }
        else if(String.IsNullOrEmpty(profileName)){
            InputField input = inputField.GetComponent<InputField>();
            input.placeholder.GetComponent<Text>().text = "Must enter a name";
        }
        else{
            profile.Create(profileName);
            string jsonStr = JsonConvert.SerializeObject(profile);
            File.WriteAllText(@"./Assets/SaveProfiles/"+profileName+".json", jsonStr);
            SceneManager.LoadScene(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
