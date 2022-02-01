using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{


    private GameObject playerObj;
    private UserHandler userHandler;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        userHandler = GameObject.FindGameObjectWithTag("UserHandler").GetComponent<UserHandler>();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(MainMenuController.currentProfile.strategiePlacement[0][1]);

        if (playerObj.transform.position.y <= -10)
        {
            //End of game
            userHandler.pushToProfile();
            SceneManager.LoadScene(3);
            //playerObj.transform.position = new Vector3(0f, 50f, 0f);
        }
    }
}
