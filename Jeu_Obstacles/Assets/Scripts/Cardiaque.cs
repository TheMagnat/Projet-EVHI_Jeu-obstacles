using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardiaque : MonoBehaviour
{

    public double currentLevel;
    private GameObject barre;

    // Start is called before the first frame update
    void Start()
    {
        barre = GameObject.FindGameObjectWithTag("barre");

        double start = Random.value * 20 + 70;

        currentLevel = start;
    }

    // Update is called once per frame
    void Update()
    {
        currentLevel += Random.value * 3.0 - 1.5;

        if(currentLevel <= 60)
        {
            currentLevel += 3;
        }
        else if (currentLevel >= 150)
        {
            currentLevel -= 3;
        }

        barre.transform.localScale = new Vector3(barre.transform.localScale.x, (float) (currentLevel * 2f) / 100f, barre.transform.localScale.z);
    }


    public double getCurrentPouls()
    {
        return currentLevel;
    }

}
