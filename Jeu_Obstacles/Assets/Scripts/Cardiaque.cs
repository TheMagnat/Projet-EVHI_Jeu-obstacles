using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardiaque : MonoBehaviour
{

    public double currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        double start = Random.value * 20 + 70;

        currentLevel = start;
    }

    // Update is called once per frame
    void Update()
    {
        currentLevel += Random.value * 3.0 - 1.5;
    }


    public double getCurrentPouls()
    {
        return currentLevel;
    }

}
