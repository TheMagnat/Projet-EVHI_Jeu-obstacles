using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{

    public GameObject obstaclePrefab;
    public GameObject projectilePrefab;
    private GameObject groundObj;

    


    private float timeSinceSpawn = 0f;
    private float timeProjectileSinceSpawn = 0f;
    private float timeSinceStart = 0f;

    //Obstacles
    public float spawnDistance = 100f;


    private float minSpawnRate = 15f;
    private float maxSpawnRate = 5f;
    private float minProjectileSpawnRate = 12f;
    private float maxProjectileSpawnRate = 6f;



    private float minSpace = 10f;
    private float maxSpace = 4f;


    private float minSpeed = 10f;
    private float maxSpeed = 20f;
    private float minProjectileSpeed = 15f;
    private float maxProjectileSpeed = 35f;

    private float timeForMaxSpeed = 60f;


    private float width;
    private float height;

    private float halfWidth;
    private float halfHeight;

    private float stepX;
    private float stepZ;


    private UserHandler userHandler;

    private List<Vector3> indexToSpawnPosition;


    private Cardiaque cardiaque;


    // Start is called before the first frame update
    void Start()
    {

        timeSinceSpawn = 100f;

        groundObj = GameObject.FindGameObjectWithTag("Ground");

        userHandler = GameObject.FindGameObjectWithTag("UserHandler").GetComponent<UserHandler>();

        cardiaque = GameObject.FindGameObjectWithTag("Cardiaque").GetComponent<Cardiaque>();


        width = groundObj.transform.localScale.x;
        height = groundObj.transform.localScale.z;

        halfWidth = width / 2;
        halfHeight = height / 2;

        stepX = width / 3;
        stepZ = height / 3;

        timeForMaxSpeed /= MainMenuController.currentProfile.BaseLevel();

        indexToSpawnPosition = new List<Vector3>
            { new Vector3(-1f, 0f, 0f), new Vector3(-1f, 0f, 1f), new Vector3(0f, 0f, 1f), new Vector3(1f, 0f, 1f), new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, -1f), new Vector3(0f, 0f, -1f), new Vector3(-1f, 0f, -1f) };


    }

    // Update is called once per frame
    void Update()
    {

        float BL = MainMenuController.currentProfile.BaseLevel();

        float meanPouls = MainMenuController.currentProfile.getPoulsMean();

        float CA = 1f;
        if (meanPouls > 10)
        {
            CA = meanPouls / (float) cardiaque.currentLevel;
        }

        timeSinceStart += Time.deltaTime;

        timeSinceSpawn += Time.deltaTime;
        timeProjectileSinceSpawn += Time.deltaTime;

        float spawnRate = difficultyInterpolation(minSpawnRate, maxSpawnRate) / (BL * CA);
        float projectileSpawnRate = difficultyInterpolation(minSpawnRate, maxSpawnRate) * BL * CA;
       

        if (timeSinceSpawn >= spawnRate)
        {
            timeSinceSpawn = 0f;

            //Select spawn position
            int selectedSpawnPos = selectProbaFromDict(userHandler.directionCamera, MainMenuController.currentProfile.getCameraMean());

            if (Random.value <= 0.7f)
            {

                //Select target position
                int selectedTarget = selectProbaFromDict(userHandler.strategiePlacement, MainMenuController.currentProfile.getPlacementMean());

                int x = (int)((selectedTarget - 1) % 3) - 1;
                int z = (int)((selectedTarget - 1) / 3) - 1;

                GameObject newInstance = Instantiate(obstaclePrefab, Vector3.Normalize(indexToSpawnPosition[selectedSpawnPos - 1]) * spawnDistance, Quaternion.identity);

                ObstacleHandler OH = newInstance.GetComponent<ObstacleHandler>();
                OH.target = new Vector3((halfWidth / (1.1f + 0.9f * Random.value)) * x, 0f, (halfHeight / (1.1f + 0.9f * Random.value)) * z);

                OH.speed = difficultyInterpolation(minSpeed, maxSpeed) * BL * CA;
                OH.space = difficultyInterpolation(minSpace, maxSpace) / (BL * CA);
            }
            else
            {

                timeSinceSpawn = spawnRate / 2f;

                //Select strategy evitement
                int selectedEvitement = selectProbaFromDictNoReverse(userHandler.strategieEvitement, MainMenuController.currentProfile.getEvitementMean());

                Vector3 spawnPosition = indexToSpawnPosition[selectedSpawnPos - 1];
                spawnPosition.y += 1 + Random.value * 2;

                GameObject newInstance = Instantiate(projectilePrefab, Vector3.Normalize(indexToSpawnPosition[selectedSpawnPos - 1]) * spawnDistance, Quaternion.identity);

                ProjectileHandler PH = newInstance.GetComponent<ProjectileHandler>();
                PH.speed = difficultyInterpolation(minProjectileSpeed, maxProjectileSpeed) * BL * CA;
                PH.dodgeMode = selectedEvitement;
            }

        }
    }

    int selectProbaFromDict(Dictionary<int, int> dict, Dictionary<int, int> meanDict)
    {
        float total = 0;
        int max = 0;

        float[] save = new float[dict.Count];
        foreach (var item in dict)
        {

            if (meanDict.Count > 0)
            {
                save[item.Key - 1] = (int) (item.Value * 0.6f + meanDict[item.Key] * 0.4f);
            }
            else
            {
                save[item.Key - 1] = item.Value;
            }

            

            if (save[item.Key - 1] > max)
            {
                max = item.Value;
            }

        }

        for (int index = 0; index < save.Length; index++)
        {
            save[index] = max - save[index] + 1;
            total += save[index];
        }

        for (int index = 0; index < save.Length; index++)
        {
            if (index == 0)
            {
                save[index] = save[index] / total;
            }
            else
            {
                save[index] = save[index - 1] + (save[index] + 1) / total;
            }

        }

        float randomValue = Random.value;
        int selected = 1;
        for (int index = 0; index < save.Length; index++)
        {

            if (randomValue < save[index])
            {
                selected = index + 1;
                break;
            }
        }
        return selected;
    }

    int selectProbaFromDictNoReverse(Dictionary<int, int> dict, Dictionary<int, int> meanDict)
    {
        float total = 0;

        float[] save = new float[dict.Count];
        foreach (var item in dict)
        {

            if (meanDict.Count > 0)
            {
                save[item.Key - 1] = (int)(item.Value * 0.6f + meanDict[item.Key] * 0.4f) + 1;
            }
            else
            {
                save[item.Key - 1] = item.Value + 1;
            }


            total += save[item.Key - 1];

        }


        for (int index = 0; index < save.Length; index++)
        {
            if (index == 0)
            {
                save[index] = save[index] / total;
            }
            else
            {
                save[index] = save[index - 1] + save[index] / total;
            }

        }

        float randomValue = Random.value;
        int selected = 1;
        for (int index = 0; index < save.Length; index++)
        {

            if (randomValue < save[index])
            {
                selected = index + 1;
                break;
            }
        }
        return selected;
    }

    float difficultyInterpolation(float min, float max)
    {
        //Debug.Log(min + " " + max + " " + (timeSinceStart / timeForMaxSpeed) + " " + (min + (max - min) * Mathf.Min(1f, timeSinceStart / timeForMaxSpeed)) );
        return min + (max - min) * Mathf.Min(1f, timeSinceStart / timeForMaxSpeed);
    }

}
