using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{

    public GameObject obstaclePrefab;
    private GameObject groundObj;

    


    private float timeSinceSpawn = 0f;
    private float timeSinceStart = 0f;

    //Obstacles
    public float spawnDistance = 100f;


    private float minSpawnRate = 10f;
    private float maxSpawnRate = 4f;



    private float minSpace = 10f;
    private float maxSpace = 1f;


    private float minSpeed = 10f;
    private float maxSpeed = 30f;
    private float timeForMaxSpeed = 60f;


    private float width;
    private float height;

    private float halfWidth;
    private float halfHeight;

    private float stepX;
    private float stepZ;


    private UserHandler userHandler;

    private List<Vector3> indexToSpawnPosition;


    // Start is called before the first frame update
    void Start()
    {
        groundObj = GameObject.FindGameObjectWithTag("Ground");

        userHandler = GameObject.FindGameObjectWithTag("UserHandler").GetComponent<UserHandler>();

        width = groundObj.transform.localScale.x;
        height = groundObj.transform.localScale.z;

        halfWidth = width / 2;
        halfHeight = height / 2;

        stepX = width / 3;
        stepZ = height / 3;


        indexToSpawnPosition = new List<Vector3>
            { new Vector3(-1f, 0f, 0f), new Vector3(-1f, 0f, 1f), new Vector3(0f, 0f, 1f), new Vector3(1f, 0f, 1f), new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, -1f), new Vector3(0f, 0f, -1f), new Vector3(-1f, 0f, -1f) };


    }

    // Update is called once per frame
    void Update()
    {

        timeSinceStart += Time.deltaTime;
        timeSinceSpawn += Time.deltaTime;

        float spawnRate = difficultyInterpolation(minSpawnRate, maxSpawnRate);

        if (timeSinceSpawn >= spawnRate)
        {
            timeSinceSpawn = 0f;

            //Select spawn position
            int selectedSpawnPos = selectProbaFromDict(userHandler.directionCamera);

            //Select target position
            int selectedTarget = selectProbaFromDict(userHandler.strategiePlacement);
            selectedTarget = 9;
            int x = (int) ((selectedTarget - 1) % 3) - 1;
            int z = (int) ((selectedTarget - 1) / 3) - 1;

            GameObject newInstance = Instantiate(obstaclePrefab, Vector3.Normalize(indexToSpawnPosition[selectedSpawnPos-1]) * spawnDistance, Quaternion.identity);

            ObstacleHandler OH = newInstance.GetComponent<ObstacleHandler>();
            OH.target = new Vector3((halfWidth/(1.1f + 0.9f*Random.value)) * x, 0f, (halfHeight/(1.1f + 0.9f*Random.value)) * z);
            OH.speed = difficultyInterpolation(minSpeed, maxSpeed);
            OH.space = difficultyInterpolation(minSpace, maxSpace);

        }
    }

    int selectProbaFromDict(Dictionary<int, int> dict)
    {
        float total = 0;
        int max = 0;

        float[] save = new float[dict.Count];
        foreach (var item in dict)
        {
            save[item.Key - 1] = item.Value;

            if (item.Value > max)
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

    float difficultyInterpolation(float min, float max)
    {
        return min + (max - min) * Mathf.Min(1f, timeSinceSpawn / timeForMaxSpeed);
    }

}
