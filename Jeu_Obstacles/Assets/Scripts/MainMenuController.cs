using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Profile;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;

    private List<GameObject> profilesButton;
    private static List<string> fileNames;
    public static ProfileType currentProfile;

    // Start is called before the first frame update
    void Start()
    {
        //******* Initialisation
        fileNames = new List<string>();
        profilesButton = new List<GameObject>();
        currentProfile = new ProfileType();

        //******* Get All Profiles Name
        string[] filePaths = Directory.GetFiles("./Assets/SaveProfiles","*.json");
        foreach (string f in filePaths){
            fileNames.Add(Path.GetFileNameWithoutExtension(f));
        }

        //******* Display Button for each profiles
        foreach(string profil in fileNames){
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            profilesButton.Add(button);
            button.SetActive(true);

            button.GetComponent<ProfilButton>().SetText(profil);
            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }

    public void ProfilButtonPush(){
        //******** Change color of Selected Profil
        GameObject p = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        ColorBlock cb = p.GetComponent<Button>().colors;
        cb.normalColor = Color.red;
        cb.selectedColor = Color.red;
        p.GetComponent<Button>().colors = cb;
        int fileIndex = profilesButton.IndexOf(p);

        foreach(GameObject pB in profilesButton){
            if (!GameObject.ReferenceEquals(p,pB)){
                cb = pB.GetComponent<Button>().colors;
                cb.normalColor = Color.white;
                cb.selectedColor = Color.white;
                pB.GetComponent<Button>().colors = cb;
            }
        }

        //******** Load Data of Selected Profile
        string path = "./Assets/SaveProfiles/"+fileNames[fileIndex]+".json";
        string jsonString = File.ReadAllText (path);
        currentProfile.Populate(jsonString);
        currentProfile.UpdateData();
    }

    public void QuitButtonPush(){
        Debug.Log("quit");
        Application.Quit();
    }

    public void CreateProfilButtonPush(){
        Debug.Log("create");
        SceneManager.LoadScene(1);
    }

    public void PlayButtonPush(Text p){
        Debug.Log("play");
        if(String.IsNullOrEmpty(currentProfile.name)){
            string text = p.text;
            p.text = text + "\nPls select profil";
        }
        else{
            SceneManager.LoadScene(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
