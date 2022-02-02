using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using System.Linq;

public class UserHandler : MonoBehaviour
{

    private GameObject playerObj;
    private GameObject cameraObj;
    private GameObject groundObj;
    private Cardiaque cardiaque;

    private float width;
    private float height;

    private float halfWidth;
    private float halfHeight;

    private float stepX;
    private float stepZ;

    public float mesureDelay = 5f;

    private float timeSinceLast = 0f;
    private float timeSinceStart = 0f;

    public Dictionary<int, int> strategiePlacement;
    public Dictionary<int, int> strategieEvitement;
    public Dictionary<int, int> directionCamera;
    public List<double> pouls;

    private bool doneSave = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        groundObj = GameObject.FindGameObjectWithTag("Ground");

        cardiaque = GameObject.FindGameObjectWithTag("Cardiaque").GetComponent<Cardiaque>();

        width = groundObj.transform.localScale.x;
        height = groundObj.transform.localScale.z;

        halfWidth = width / 2;
        halfHeight = height / 2;

        stepX = width / 3;
        stepZ = height / 3;

        strategiePlacement = new Dictionary<int, int>{
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 0 },
            { 8, 0 },
            { 9, 0 }
        };

        strategieEvitement = new Dictionary<int, int>{
            { 1, 0 },
            { 2, 0 },
            { 3, 0 }
        };

        directionCamera = new Dictionary<int, int>{
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 0 },
            { 8, 0 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        timeSinceLast += Time.deltaTime;

        if (timeSinceLast >= mesureDelay)
        {
            timeSinceLast = 0;

            //Placement
            int x = Mathf.Clamp((int)((playerObj.transform.position.x + halfWidth) / stepX), 0, 2);
            int z = Mathf.Clamp((int)((playerObj.transform.position.z + halfHeight) / stepZ), 0, 2);

            int position = x + 3 * z + 1;

            ++strategiePlacement[position];

            //Orientation
            int orient = 0;

            if (cameraObj.transform.forward.z < 0) { orient = 1; }
            else { orient = 0; }


            int xOrient = (int)Math.Round((cameraObj.transform.forward.x + 1f) * 2);
            int orientation = (xOrient + (4 - xOrient) * orient * 2) % 8;

            ++directionCamera[orientation+1];


            //Pouls
            pouls.Add( cardiaque.getCurrentPouls() );

        }
    }

    public void pushToProfile()
    {

        if (!doneSave)
        {

            doneSave = true;

            MainMenuController.currentProfile.strategiePlacement.Add(strategiePlacement);
            MainMenuController.currentProfile.strategieEvitement.Add(strategieEvitement);
            MainMenuController.currentProfile.directionCamera.Add(directionCamera);
            MainMenuController.currentProfile.durees.Add(timeSinceStart);

            if (pouls.Count == 0)
            {
                MainMenuController.currentProfile.pouls.Add(MainMenuController.currentProfile.getPoulsMean());
            }
            else
            {
                MainMenuController.currentProfile.pouls.Add(pouls.Average());
            }

            MainMenuController.currentProfile.dates.Add(DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss"));

        }

    }

}
